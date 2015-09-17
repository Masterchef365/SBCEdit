using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;

public class Serialization : MonoBehaviour { //Serialisation, parsing, and pathway tracking

	public GameObject startPoint;
	public GameObject worldCanvas;

	public GameObject branchPrefab;
	public GameObject attributePrefab;
	public GameObject dataPrefab;

	public GameObject lineCam;
	public GameObject connectCallObject;

	public List<GameObject> allNodes;

	public string directory = "";

	public string enclosingTypeSetter = "";

	//public void Start () {

	//}

	public void makeNewAttributeNode () {
		GameObject newNode = (GameObject)Instantiate(attributePrefab);
		SetAttributeNode nodeSettings = newNode.GetComponent<SetAttributeNode>();
		nodeSettings.setUpNode(lineCam, connectCallObject);
		nodeSettings.setEnlosingType(enclosingTypeSetter);
		newNode.transform.SetParent(worldCanvas.transform, false);
		newNode.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
		allNodes.Add(newNode);
	}

	public void makeNewBranchNode () {
		GameObject newNode = (GameObject)Instantiate(branchPrefab);
		SetBranchNode nodeSettings = newNode.GetComponent<SetBranchNode>();
		nodeSettings.setUpNode(lineCam, connectCallObject);
		nodeSettings.setEnlosingType(enclosingTypeSetter);
		newNode.transform.SetParent(worldCanvas.transform, false);
		newNode.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
		allNodes.Add(newNode);
	}

	public void makeNewDataNode () {
		GameObject newNode = (GameObject)Instantiate(dataPrefab);
		SetDataNode nodeSettings = newNode.GetComponent<SetDataNode>();
		nodeSettings.setUpNode(connectCallObject);
		nodeSettings.setEnlosingType(enclosingTypeSetter);
		newNode.transform.SetParent(worldCanvas.transform, false);
		newNode.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
		allNodes.Add(newNode);
	}

	public void exitProgram () {
		saveFile();
		Application.Quit();
	}

	public void openFile () {
		deleteAllNodes();
		readDocument(directory);
	}

	public void setDirectory (string dir) {
		directory = dir;
	}

	public void typeEncloserSetter (string type) {
		enclosingTypeSetter = type;
	}

	public void saveFile () {
		string contents = createXMLFromNodeConfigurations(startPoint);
		System.IO.File.WriteAllText(directory, contents);
	}

	public void deleteAllNodes () {
		foreach (GameObject node in allNodes) {
			Destroy(node); //KILL ZEM ALL!
		}
	}

	#region XML to node converter

	public void readDocument (string dir) {
		XmlDocument doc = new XmlDocument();
		doc.Load(dir);

		xIncrement = 0;
		yIncrement.Add(0);
		foreach (XmlNode node in doc.DocumentElement.ChildNodes) {
			XMLConvolution(node, startPoint);
			yIncrement[0]++;
		}
		
	}
	
	private int xIncrement = 0;
	private List<int> yIncrement = new List<int>(); //There is a running profile of how many places on the y axis on what level (x position) as to organize them
	public void XMLConvolution (XmlNode rootNode, GameObject lastOutPort) { //LastOutPort is the parent node's out port GameObject - so these next nodes can be linked to it
		xIncrement++;
		if (yIncrement.Count - 1 < xIncrement) {
			yIncrement.Add(0);
		} 
		if (rootNode.Attributes != null && rootNode.Attributes.Count > 0) { //
			if (rootNode.HasChildNodes) {
				GameObject newOutPort = newAttributeNode(rootNode, lastOutPort, xIncrement, yIncrement[xIncrement]); //Attribute node WITH MORE ATTACHED!
				yIncrement[xIncrement] += 2;
				//yIncrement[xIncrement] = 0;
				foreach (XmlNode node in rootNode) {
					XMLConvolution(node, newOutPort);
					//yIncrement[xIncrement]++; //Padding space
				}
			} else {
				newAttributeNode(rootNode, lastOutPort, xIncrement, yIncrement[xIncrement]); //Attribute Node
				yIncrement[xIncrement] += 2;
			}
		} else {
			if (rootNode.HasChildNodes) {
				if (rootNode.FirstChild.Name == "#text") {
					newDataNode(rootNode, lastOutPort, xIncrement, yIncrement[xIncrement]); //Data Node
					yIncrement[xIncrement]++;
				} else {
					GameObject newOutPort = newBranchNode(rootNode, lastOutPort, xIncrement, yIncrement[xIncrement]); //Branch node
					yIncrement[xIncrement]++;
					//yIncrement[xIncrement] = 0;
					foreach (XmlNode node in rootNode) {
						XMLConvolution(node, newOutPort);
						//yIncrement++; //Padding space
					}
				}
			}
		}
		xIncrement--;
	}

	#endregion

	#region Node Creators
	public GameObject newAttributeNode (XmlNode node, GameObject connectToNode, int xCoord, int yCoord) { //Returns the out port of that node it makes
		//Debug.Log("Attribute Node: " + node.Name + " X Coord: " + xCoord + " Y Coord : " + yCoord);
		GameObject newNode = (GameObject)Instantiate(attributePrefab);
		SetAttributeNode nodeSettings = newNode.GetComponent<SetAttributeNode>();
		nodeSettings.setUpNode(lineCam, connectCallObject);
		if (connectToNode != null) { nodeSettings.setInConnection(connectToNode); }
		nodeSettings.extractAttributes(node);
		newNode.transform.SetParent(worldCanvas.transform, false);

		newNode.transform.position = new Vector3(550f * xCoord, 150f * yCoord, 0f);
		newNode.transform.position += gameObject.transform.position;

		connectToNode.GetComponent<BranchPort>().branches.Add(nodeSettings.inPort);
		allNodes.Add(newNode);
		return nodeSettings.outPort;

	}

	public GameObject newBranchNode (XmlNode node, GameObject connectToNode, int xCoord, int yCoord) { //Returns the out port of that node it makes
		//Debug.Log("Branch Node: " + node.Name + " X Coord: " + xCoord + " Y Coord : " + yCoord);
		GameObject newNode = (GameObject)Instantiate(branchPrefab);
		SetBranchNode nodeSettings = newNode.GetComponent<SetBranchNode>();
		nodeSettings.setUpNode(lineCam, connectCallObject);
		if (connectToNode != null) { nodeSettings.setInConnection(connectToNode); }
		nodeSettings.setEnlosingType(node.Name);
		newNode.transform.SetParent(worldCanvas.transform, false);

		newNode.transform.position = new Vector3(550f * xCoord, 150f * yCoord, 0f);
		newNode.transform.position += gameObject.transform.position;

		connectToNode.GetComponent<BranchPort>().branches.Add(nodeSettings.inPort);
		allNodes.Add(newNode);
		return nodeSettings.outPort;
		
	}

	public void newDataNode (XmlNode node, GameObject connectToNode, int xCoord, int yCoord) {
		//Debug.Log("Data Node: " + node.Name + " X Coord: " + xCoord + " Y Coord : " + yCoord);
		GameObject newNode = (GameObject)Instantiate(dataPrefab);
		SetDataNode nodeSettings = newNode.GetComponent<SetDataNode>();
		nodeSettings.setUpNode(connectCallObject);
		if (connectToNode != null) { nodeSettings.setInConnection(connectToNode); }
		nodeSettings.setEnlosingType(node.Name);
		nodeSettings.setData(node.InnerXml);
		newNode.transform.SetParent(worldCanvas.transform, false);

		newNode.transform.position = new Vector3(550f * xCoord, 150f * yCoord, 0f);
		newNode.transform.position += gameObject.transform.position;

		connectToNode.GetComponent<BranchPort>().branches.Add(nodeSettings.inPort);
		allNodes.Add(newNode);
	}
	#endregion

	#region Node to XML converter
	public string createXMLFromNodeConfigurations (GameObject startingPort) {
		string XML = "";
		XML += "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
		XML += "<Definitions xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\n";
		treeTabCount = 1;
		XML += getIndent(1) + followTree(startPoint, true);
		XML += "</Definitions>\n";
		return XML;
	}
	
	private int treeTabCount = 0;
	public string followTree (GameObject startingPort, bool isRoot) {
		//if (!isRoot) { treeTabCount++; } //Problem on first-run indentation
		treeTabCount++;
		string result = "";
		List<GameObject> input = startingPort.GetComponent<BranchPort>().branches;
		foreach (GameObject port in input) {
			if (port) {
				ConnectAblePort CAP = port.GetComponent<ConnectAblePort>();
				if (CAP.referForAttributes) {
					if (CAP.getData() != null) { result += getIndent(treeTabCount) + CAP.getData() + "\n"; }
					if (CAP.referForContinuation.GetComponent<BranchPort>().branches.Count > 0) { //If has any connected branches on it's 'OUT' port follow the tree through there
						result += followTree(CAP.referForContinuation, false);
						result += getIndent(treeTabCount) + "</" + CAP.enclosingType + ">\n";
					}
				} else {
					if (CAP.referForContinuation) { //If it has a continuing part follow that instead but log the current node, else it has hit a data wall 
						if (isRoot) { treeTabCount -= 2; } //Problem on first-run indentation
						result += getIndent(treeTabCount) + "<" + CAP.enclosingType + ">\n";
						result += followTree(CAP.referForContinuation, false);
						result += getIndent(treeTabCount) + "</" + CAP.enclosingType + ">\n";
					} else {
						result += getIndent(treeTabCount) + "<" + CAP.enclosingType + ">";
						if (CAP.getData() != null) { result += CAP.getData(); };
						result = result + "</" + CAP.enclosingType + ">\n";
					}
				}
			}
		}
		treeTabCount--;
		return result;
	}

	public string getIndent (int tabNumber) {
		string result = "";
		if (tabNumber != 0) {
			for (int t = 0; t < tabNumber; t++) {
				result += "\t";
			} 
		}
		return result;
	}

	#endregion

}

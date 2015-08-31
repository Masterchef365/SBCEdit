using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class SetAttributeNode : MonoBehaviour {
	
	public GameObject inPort;
	public GameObject outPort;
	public GameObject titleText;
	public GameObject attributePanel;

	void Start () {
		
	}

	public void setInConnection (GameObject port) {
		inPort.GetComponent<ConnectAblePort>().currentConnection = port;
	}

	public void setOutConnections (List<GameObject> ports) {
		outPort.GetComponent<BranchPort>().branches = ports;
	}

	public void addOutConnection (GameObject port) {
		outPort.GetComponent<BranchPort>().branches.Add(port);
	}

	public void extractAttributes (XmlNode node) { //Extracttributes!! <- Pardon the code-crazed me making jokes, thank you.
		setEnlosingType(node.Name);
		AttributePanelManager manager = attributePanel.GetComponent<AttributePanelManager>();
		foreach (XmlAttribute attribute in node.Attributes) {
			manager.addAttribute(attribute.Name, attribute.Value);
		}
	}
	
	public void setEnlosingType (string type) {
		titleText.GetComponent<Text>().text = "<" + type + ">";
		inPort.GetComponent<ConnectAblePort>().enclosingType = type;
	}
	
	public void setUpNode (GameObject lineCam, GameObject connectCallObject) {
		outPort.GetComponent<BranchPort>().lineCam = lineCam;
		outPort.GetComponent<BranchPort>().connectCallObject = connectCallObject;
		inPort.GetComponent<ConnectAblePort>().connectCallObject = connectCallObject;
	}

}

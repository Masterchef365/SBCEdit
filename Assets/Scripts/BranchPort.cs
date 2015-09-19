using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BranchPort : MonoBehaviour {
	
	public List<GameObject> branches; //Repository of all the connected ports
	public GameObject lineCam; //In charge of drawing splines/grid
	public LineTypeDictionary.lineTypes connectionType = LineTypeDictionary.lineTypes.Branch;
	public GameObject connectCallObject; //Place where the script lives that connects ports remotely
	public GameObject branchContinuationObject; //If it is a double sided branch this would be the next destination.
	public LineTypeDictionary.valueClass colorDictionary;
	public string currentData;
	public bool showLines = true;

	void Start() {
		colorDictionary = new LineTypeDictionary.valueClass(); 
		GetComponent<Image> ().color = colorDictionary.colors[(int)connectionType]; //Set the node color
		lineCam.GetComponent<SplineDraw>().ports.Add(gameObject);
	}

	public void startConnection () {
		SplineDraw lineDrawer = lineCam.GetComponent<SplineDraw>();
		PortConnector connect = connectCallObject.GetComponent<PortConnector>();
		connect.outputPort = gameObject;
		lineDrawer.drawMouseTo = gameObject;
		lineDrawer.drawToMouse = true;
	}

	public void addConnection(GameObject port) {
		if (!branches.Contains(port)) {
			branches.Add (port);
			lineCam.GetComponent<SplineDraw>().drawToMouse = false;
		}
		sendDataToConnectionEnds(currentData);
	}

	public void sendDataToConnectionEnds (string data) {
		currentData = data;
		foreach (GameObject port in branches) {
			if (port) {
				port.GetComponent<ConnectAblePort>().setData(data);
			}
		}
	}

	public void showFurther (bool show) {
		if (show) {
			showLines = true;
			objectConvolutionShowHide(branches, true);
		} else {
			showLines = false;
			objectConvolutionShowHide(branches, false);
		}
	}
	
	void objectConvolutionShowHide (List<GameObject> starts, bool showHide) {
		foreach (GameObject conPort in starts) {
			if (conPort.GetComponent<ConnectAblePort>().referForContinuation != null) {
				GameObject port = conPort.GetComponent<ConnectAblePort>().referForContinuation;
				objectConvolutionShowHide(port.GetComponent<BranchPort>().branches, showHide);
				port.GetComponent<BranchPort>().showLines = showHide;
				conPort.GetComponent<ConnectAblePort>().nodeEnable(showHide);
			} else {
				conPort.GetComponent<ConnectAblePort>().nodeEnable(showHide);
			}
		}
	}


}

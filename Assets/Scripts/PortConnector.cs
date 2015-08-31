using UnityEngine;
using System.Collections;

public class PortConnector : MonoBehaviour {

	public GameObject outputPort;

	void Update () {
		if (Input.GetMouseButton (1)) { //Cancel if you're in the middle of drag and drop and right click happens
			outputPort = null;
		}
	}

	public void bindPorts (GameObject sender) {
		if (outputPort) {
			if ((int)outputPort.GetComponent<BranchPort>().connectionType == (int)sender.GetComponent<ConnectAblePort>().connectionType) { //Same Type
				outputPort.GetComponent<BranchPort>().addConnection(sender);
				sender.GetComponent<ConnectAblePort>().setConnection(outputPort);
				outputPort = null;
			}
		}
	}

}

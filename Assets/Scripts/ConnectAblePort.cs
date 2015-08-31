using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConnectAblePort : MonoBehaviour {
	
	public GameObject currentConnection;
	public LineTypeDictionary.lineTypes connectionType = LineTypeDictionary.lineTypes.Branch;
	public LineTypeDictionary.valueClass colorDictionary;
	public string data;
	public string enclosingType; //XML Type, like <CubeBlocks> or something
	public GameObject referForContinuation;
	public GameObject referForData;
	public GameObject hideObject; //Hidden when this node is connected to
	public GameObject referForAttributes;
	public GameObject connectCallObject;

	void Start() {
		colorDictionary = new LineTypeDictionary.valueClass(); 
		GetComponent<Image> ().color = colorDictionary.colors[(int)connectionType]; //Set the node color
	}

	public void finishConnection () {
		connectCallObject.GetComponent<PortConnector>().bindPorts(gameObject);
	}

	public void terminateConnection () {
		if (currentConnection) {
			currentConnection.GetComponent<BranchPort>().branches.Remove(gameObject);
		}
		currentConnection = null;
		if (hideObject) { hideObject.SetActive(true); }
	}

	public void setConnection (GameObject port) {
		terminateConnection();
		currentConnection = port;
		if (hideObject) { hideObject.SetActive(false); }
	}

	public void setData (string input) {
		if (referForData) {
			referForData.GetComponent<ConnectAblePort>().data = input;
		} else {
			data = input;
		}
	}

	public string getData () {
		if (referForData) {
			return referForData.GetComponent<ConnectAblePort>().getData();
		} else {
			if (referForAttributes) { //Attributes node
				if (referForContinuation.GetComponent<BranchPort>().branches.Count > 0) { //Continues on
					return "<" + enclosingType + referForAttributes.GetComponent<AttributePanelManager>().getAttributeXMLString() + ">";
				} else {
					return "<" + enclosingType + referForAttributes.GetComponent<AttributePanelManager>().getAttributeXMLString() + "/>";
				}
			} else {
				return data;
			}
		}
	}

}

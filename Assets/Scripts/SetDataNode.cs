using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetDataNode : MonoBehaviour {
	
	public GameObject inPort;
	public GameObject titleText;
	public GameObject dataInputTextBox;

	void Start () {
		
	}

	public void setInConnection (GameObject port) {
		inPort.GetComponent<ConnectAblePort>().currentConnection = port;
	}

	public void setData (string data) {
		inPort.GetComponent<ConnectAblePort>().setData(data);
		dataInputTextBox.GetComponent<InputField>().text = data;
	}

	public void setEnlosingType (string type) {
		titleText.GetComponent<Text>().text = "<" + type + ">";
		inPort.GetComponent<ConnectAblePort>().enclosingType = type;
	}

	public void setUpNode (GameObject connectCallObject) {
		inPort.GetComponent<ConnectAblePort>().connectCallObject = connectCallObject;
	}

}

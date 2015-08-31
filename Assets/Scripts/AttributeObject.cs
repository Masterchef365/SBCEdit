using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AttributeObject : MonoBehaviour {

	public GameObject attributeNameTextPanel;
	public GameObject attributeValueTextPanel;
	public GameObject attributePanelManagerObject;

	public string getName () {
		return attributeNameTextPanel.GetComponent<InputField>().text;
	}

	public string getValue () {
		return attributeValueTextPanel.GetComponent<InputField>().text;
	}

	public string setName (string name) {
		return attributeNameTextPanel.GetComponent<InputField>().text = name;
	}
	
	public string setValue (string value) {
		return attributeValueTextPanel.GetComponent<InputField>().text = value;
	}

	public void copySelf () {
		GameObject selfCopy = (GameObject)Instantiate((Object)gameObject);
		selfCopy.transform.parent = gameObject.transform.parent;
		attributePanelManagerObject.GetComponent<AttributePanelManager>().panelObjects.Add(selfCopy);
	}

	public void deleteSelf () {
		attributePanelManagerObject.GetComponent<AttributePanelManager>().remove(gameObject);
	}


}

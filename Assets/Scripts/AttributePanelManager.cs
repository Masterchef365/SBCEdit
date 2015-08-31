using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttributePanelManager : MonoBehaviour {

	public List<GameObject> panelObjects;
	public bool hasBeenSet = false;

	public void remove(GameObject subject) {
		if (panelObjects.Count > 1) {
			panelObjects.Remove(subject);
			Destroy(subject);
		}
	}

	public string getAttributeXMLString () {
		string result = "";
		foreach (GameObject attribute in panelObjects) {
			result += " " + attribute.GetComponent<AttributeObject>().getName() + "=\"" + attribute.GetComponent<AttributeObject>().getValue() + "\""; 
		}
		return result;
	}

	public void addAttribute (string name, string value) {
		if (!hasBeenSet) { //Set the one thats there
			panelObjects[0].GetComponent<AttributeObject>().setName(name);
			panelObjects[0].GetComponent<AttributeObject>().setValue(value);
			hasBeenSet = true;
		} else {
			GameObject newPanel = (GameObject)Instantiate(panelObjects[0].gameObject);
			newPanel.transform.SetParent(panelObjects[0].transform.parent);
			newPanel.GetComponent<AttributeObject>().setName(name);
			newPanel.GetComponent<AttributeObject>().setValue(value);
			panelObjects.Add (newPanel);

		}
	}

}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FileObjectSetter : MonoBehaviour {

	public GameObject nameTextObject; 
	public GameObject sizeTextObject;
	public string directory = "";
	GameObject fileBrowserObject;

	public void setFileInfo (string name, string size, string dir, GameObject fileBrowserObj) {
		nameTextObject.GetComponent<Text>().text = name;
		sizeTextObject.GetComponent<Text>().text = size;
		directory = dir;
		fileBrowserObject = fileBrowserObj;
	}

	public void click() {
		fileBrowserObject.GetComponent<FileBrowser>().openDirectory(directory);
	}
	
}

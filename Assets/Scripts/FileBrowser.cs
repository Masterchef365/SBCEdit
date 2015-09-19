using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class FileBrowser : MonoBehaviour {

	public GameObject filePrefab;
	public GameObject folderPrefab;
	public List<GameObject> windowObjects;
	public GameObject directoryText; //So I can set the directory
	public GameObject serializationNode; //Again so I can set the directory
	public string currentDir;
	public GameObject indicatorText;

	void Start() {
		string examplePath = System.IO.Directory.GetCurrentDirectory() + @"\Examples";
		if (Directory.Exists(examplePath)) {
			setCurrentDirectory(examplePath);
			openDirectory(examplePath);
		} else {
			setCurrentDirectory(@"C:\");
			openDirectory(@"C:\");
		}
	}

	public void openDirectory (string dir) {
		currentDir = dir;
		if (windowObjects != null && windowObjects.Count > 0) {
			foreach (GameObject obj in windowObjects) {
				Destroy(obj);
			} //KILL ZEM ALL!
		}

		if (Directory.Exists(dir)) { //It has things in it!
			setCurrentDirectory(dir);
			setIndicationText("");
			foreach (string newDir in Directory.GetDirectories(dir)) {
				GameObject newDirectoryObject = (GameObject)Instantiate(folderPrefab);
				newDirectoryObject.transform.SetParent(gameObject.transform);
				newDirectoryObject.GetComponent<FileObjectSetter>().setFileInfo(new DirectoryInfo(newDir).Name, "", newDir, gameObject);
				windowObjects.Add(newDirectoryObject);
			}

			foreach (string newFile in Directory.GetFiles(dir)) {
				if (Path.GetExtension(newFile) == ".sbc" || Path.GetExtension(newFile) == ".xml"|| Path.GetExtension(dir) == ".gsc") { //Valid files
					GameObject newFileObject = (GameObject)Instantiate(filePrefab);
					newFileObject.transform.SetParent(gameObject.transform);
					newFileObject.GetComponent<FileObjectSetter>().setFileInfo("" + Path.GetFileName(newFile), new System.IO.FileInfo(newFile).Length + " B", newFile, gameObject);
					windowObjects.Add(newFileObject);
				}
			}

		} else { //Maybe it is a thing?
			if (File.Exists(dir)) { //Yay, it is a thing!
				setCurrentDirectory(dir);
				setIndicationText(Path.GetFileName("Current File: " + dir));
			} else { //It is nothing. It is a phantom.
				setIndicationText("Invalid directory");
			}

		} 
	}

	public void upOneDirectory () {
		openDirectory(Directory.GetParent(currentDir).FullName);
	}

	void setCurrentDirectory (string dir) {
		directoryText.GetComponent<InputField>().text = dir;
		if (File.Exists(dir) && (Path.GetExtension(dir) == ".sbc" || Path.GetExtension(dir) == ".xml"|| Path.GetExtension(dir) == ".gsc")) { //Make sure the file is valid
			serializationNode.GetComponent<Serialization>().directory = dir;
		}

	}

	void setIndicationText (string text) {
		indicatorText.GetComponent<Text>().text = text;
	}

	public void openExampleFolder () {
		string examplePath = System.IO.Directory.GetCurrentDirectory() + @"\Examples";
		setCurrentDirectory(examplePath);
		openDirectory(examplePath);
	}

	public void openModsFolder () {
		string modsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + @"\SpaceEngineers\Mods";
		setCurrentDirectory(modsFolder);
		openDirectory(modsFolder);
	}

}

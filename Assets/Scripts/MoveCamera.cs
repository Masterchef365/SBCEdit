using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

	public Vector3 startPoint = new Vector3(0,0,0);
	public Vector3 mouseWorld;
	Camera mainCam;
	public GameObject lineCam;
	public Rect hardCamLimits;
	public float mouseZoomMultiplier = 30f;

	void Start() {
		mainCam = GetComponent<Camera>();
	}


	void Update () {
		if (Input.GetMouseButton(1)) { //Right Click and drag
			mouseWorld = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
			if (startPoint == new Vector3(0,0,0)) {
				startPoint = mouseWorld;
			} else {
				transform.position = new Vector3(startPoint.x - (mouseWorld.x - transform.position.x), startPoint.y - (mouseWorld.y - transform.position.y), startPoint.z - (mouseWorld.z - transform.position.z));
			}
		} else {
			startPoint = new Vector3(0,0,0);
		}

		//Space Limits [EDIT] NOPE!
		/*
		if (transform.position.x > hardCamLimits.max.x) {
			transform.position = new Vector3(hardCamLimits.max.x, transform.position.y, transform.position.z);
		}

		if (transform.position.x < hardCamLimits.min.x) {
			transform.position = new Vector3(hardCamLimits.min.x, transform.position.y, transform.position.z);
		}

		if (transform.position.y > hardCamLimits.max.y) {
			transform.position = new Vector3(transform.position.x, hardCamLimits.max.y, transform.position.z);
		}
		
		if (transform.position.y < hardCamLimits.min.y) {
			transform.position = new Vector3(transform.position.x, hardCamLimits.min.y, transform.position.z);
		}
		*/

		//Camera Zoom
		mainCam.orthographicSize += Input.mouseScrollDelta.y * -1f * mouseZoomMultiplier;
		if (mainCam.orthographicSize < 120) {
			mainCam.orthographicSize = 120;
		}
		lineCam.GetComponent<Camera>().orthographicSize = mainCam.orthographicSize;





	}
   	
}

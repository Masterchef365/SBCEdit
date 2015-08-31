using UnityEngine;
using System.Collections;

public class DragWithMouse : MonoBehaviour {

	public GameObject raycastCamera;
	public Vector3 startPoint = Vector3.zero; //Make the drag smoother by not teleporting to the center of the mouse cursor

	void Start() {
		raycastCamera = Camera.main.gameObject;
	}

	public void Click() {
		startPoint = raycastCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
		startPoint.x = transform.position.x - startPoint.x;
		startPoint.y = transform.position.y - startPoint.y;
	}

	public void Drag () {
		Vector3 mouseWorld = raycastCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector3(mouseWorld.x + startPoint.x, mouseWorld.y + startPoint.y, 0f);
	}

	public void DestroyNode () {
		Destroy(gameObject);
	}

}

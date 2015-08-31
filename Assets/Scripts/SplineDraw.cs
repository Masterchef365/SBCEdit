using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplineDraw : MonoBehaviour
{

	public List<GameObject> ports; //Ports contain their own XYZ coordinates and the coords of the ones they are connected to
	public int subdivisions = 10;
	public bool drawToMouse = false;
	public GameObject drawMouseTo;
	public GameObject connectCallObject;
	public LineTypeDictionary.valueClass colorDictionary;

	void Start ()
	{
		colorDictionary = new LineTypeDictionary.valueClass (); 
	}

	void OnPostRender ()
	{
		GL.Begin (GL.LINES);
		if (drawToMouse) {
			Vector3 mouseWorld = GetComponent<Camera> ().ScreenToWorldPoint (Input.mousePosition);
			mouseWorld.z = 0;

			if (Input.GetMouseButton (1)) {
				drawToMouse = false;
			}

			Vector3 handleA = new Vector3 (mouseWorld.x, drawMouseTo.transform.position.y, 0);
			Vector3 handleB = new Vector3 (mouseWorld.x, mouseWorld.y, 0);
			Vector3 handleC = new Vector3 (drawMouseTo.transform.position.x, drawMouseTo.transform.position.y, 0);
			Vector3 handleD = new Vector3 (drawMouseTo.transform.position.x, mouseWorld.y, 0);

			GL.Color (colorDictionary.colors [(int)drawMouseTo.GetComponent<BranchPort> ().connectionType]);

			for (float t = 0; t <= 1f; t = t + (1f/subdivisions)) {
				GL.Vertex (ReturnCatmullRom (t, handleA, handleB, handleC, handleD));
				GL.Vertex (ReturnCatmullRom (t + (1f / subdivisions), handleA, handleB, handleC, handleD));
			}
		}


		foreach (GameObject port in ports) {
			if (port) {
				List<GameObject> connections = port.GetComponent<BranchPort> ().branches;
				foreach (GameObject connect in connections) {
					if (connect) {
						//Debug.DrawLine(port.transform.position, connect.transform.position, colorDictionary.colors [(int)port.GetComponent<BranchPort> ().connectionType]);

						Vector3 handleA = new Vector3 (port.transform.position.x, connect.transform.position.y, 0);
						Vector3 handleB = new Vector3 (port.transform.position.x, port.transform.position.y, 0);
						Vector3 handleC = new Vector3 (connect.transform.position.x, connect.transform.position.y, 0);
						Vector3 handleD = new Vector3 (connect.transform.position.x, port.transform.position.y, 0);

						try {
							GL.Color (colorDictionary.colors [(int)port.GetComponent<BranchPort> ().connectionType]);
						} finally {

						}

						for (float t = 0; t <= 1f; t = t + (1f/subdivisions)) {
							GL.Vertex (ReturnCatmullRom (t, handleA, handleB, handleC, handleD));
							GL.Vertex (ReturnCatmullRom (t + (1f / subdivisions), handleA, handleB, handleC, handleD));
						}
					}
				}
			}
		}
		GL.End ();
	}

	//Returns a position between 4 Vector3 with Catmull-Rom Spline algorithm
	//http://www.iquilezles.org/www/articles/minispline/minispline.htm
	Vector3 ReturnCatmullRom (float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		Vector3 a = 0.5f * (2f * p1);
		Vector3 b = 0.5f * (p2 - p0);
		Vector3 c = 0.5f * (2f * p0 - 5f * p1 + 4f * p2 - p3);
		Vector3 d = 0.5f * (-p0 + 3f * p1 - 3f * p2 + p3);
		
		Vector3 pos = a + (b * t) + (c * t * t) + (d * t * t * t);

		return pos;
	}

}

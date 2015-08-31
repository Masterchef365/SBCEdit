// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using System.Collections;

public class GridOptimized : MonoBehaviour {
	
	public Color mainColor = new Color(1f,1f,1f,1f);
	public Color subColor = new Color(1f,1f,1f,1f);
	public float increment = 1000f;
	public float subIncrement = 1000f;
	private Material lineMat;
	public bool showGrid = true;
	public Vector3 screenTopRightWorld;
	public Vector3 screenBottomLeftWorld;

	void OnPostRender() {
		Camera cam = GetComponent<Camera>(); //Must be applied to camera
		screenTopRightWorld = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		screenBottomLeftWorld = cam.ScreenToWorldPoint(new Vector3(0, 0, 0));

		screenTopRightWorld.x = ((float)(int)(screenTopRightWorld.x / increment)* increment) + (3 * increment);
		screenTopRightWorld.y = ((float)(int)(screenTopRightWorld.y / increment)* increment) + (3 * increment);

		screenBottomLeftWorld.x = ((float)(int)(screenBottomLeftWorld.x / increment)* increment) - (3 * increment);
		screenBottomLeftWorld.y = ((float)(int)(screenBottomLeftWorld.y / increment)* increment) - (3 * increment);


		if (showGrid) {
			lineMat = CreateLineMaterial ();
			lineMat.SetPass (0);

			GL.Begin (GL.LINES);
			GL.Color(subColor);
			for (float x = screenBottomLeftWorld.x; x < screenTopRightWorld.x; x += subIncrement) {
				GL.Vertex3 (x, screenTopRightWorld.y, 0); 
				GL.Vertex3 (x, screenBottomLeftWorld.y - 2, 0);
			}
			
			for (float y = screenBottomLeftWorld.y; y < screenTopRightWorld.y; y += subIncrement) {
				GL.Vertex3 (screenTopRightWorld.x, y, 0); 
				GL.Vertex3 (screenBottomLeftWorld.x - 2, y, 0);
			}



			GL.Color(mainColor);
			for (float x = screenBottomLeftWorld.x; x < screenTopRightWorld.x; x += increment) {
				GL.Vertex3 (x, screenTopRightWorld.y, 0); 
				GL.Vertex3 (x, screenBottomLeftWorld.y - 2, 0);
			}

			for (float y = screenBottomLeftWorld.y; y < screenTopRightWorld.y; y += increment) {
				GL.Vertex3 (screenTopRightWorld.x, y, 0); 
				GL.Vertex3 (screenBottomLeftWorld.x - 2, y, 0);
			}
		
			GL.End ();
		}
	}
	
	
	
	Material CreateLineMaterial() 
	{
		Material lineMaterial;
		lineMaterial = new Material( "Shader \"Lines/Colored Blended\" {" +
		                            "SubShader { Pass { " +
		                            "    Blend SrcAlpha OneMinusSrcAlpha " +
		                            "    ZTest Always " +
		                            "    ZWrite Off Cull Off Fog { Mode Off } " +
		                            "    BindChannels {" +
		                            "      Bind \"vertex\", vertex Bind \"color\", color }" +
		                            "} } }" );
		lineMaterial.hideFlags = HideFlags.HideAndDontSave;
		lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		return lineMaterial;
	}
	
	public void showLineGrid (bool input) {
		showGrid = input;
	}
	
	
}

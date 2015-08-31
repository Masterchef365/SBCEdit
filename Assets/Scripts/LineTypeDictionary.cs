using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineTypeDictionary : MonoBehaviour {
	public enum lineTypes
	{
		Branch, Data
	};
	public class valueClass {
		public List<Color> colors = new List<Color> ();
		public valueClass () {
			colors.Add(new Color(0.7f, 0.7f, 0.7f));
			colors.Add(new Color(0f, 0.588f, 1f));
		}
	}


}

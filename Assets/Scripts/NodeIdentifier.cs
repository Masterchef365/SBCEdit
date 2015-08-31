using UnityEngine;
using System.Collections;

public class NodeIdentifier : MonoBehaviour {
	public enum nodeTypes {
		data, branch, attribute
	};
	public nodeTypes nodeType = nodeTypes.attribute;

}

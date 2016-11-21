using UnityEngine;
using System.Collections;

public class FixedPositionX : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(0, transform.position.y, transform.position.z);
	}
}

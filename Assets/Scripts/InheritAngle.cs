using UnityEngine;
using System.Collections;

public class InheritAngle : MonoBehaviour {

	ParticleSystem ps;

	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		ps.startRotation = -transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
	}
}

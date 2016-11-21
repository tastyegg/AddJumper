using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	int destination;

	// Use this for initialization
	void Start () {
		destination = 0;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0, Mathf.Lerp(0, destination - transform.position.y, 0.1f), 0);
	}

	public void SetDestination(int destination)
	{
		this.destination = destination;
	}
}

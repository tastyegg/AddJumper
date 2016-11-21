using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloorController : MonoBehaviour {

	public int floorHeight;
	public int xPosition;

	Text floorText;
	Rigidbody2D rb;
	LevelGen lg;

	// Use this for initialization
	void Awake () {
		floorText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
		rb = GetComponent<Rigidbody2D>();
		lg = GameObject.FindObjectOfType<LevelGen>();
	}
	
	void FloatToFloorHeight()
	{
		transform.position = new Vector3(Mathf.Lerp(transform.position.x, xPosition, 0.15f), Mathf.Lerp(transform.position.y, floorHeight - 5, 0.05f), transform.position.z);
		CheckXPosition();
	}

	void ArrangeNewPosition()
	{
		xPosition = lg.GetEmptyXPosition();
		floorHeight = lg.GetNextHeightLevel();
	}

	void CheckXPosition()
	{
		if (Mathf.Abs(transform.position.x) > lg.GetWidth() / 2)
		{
			transform.position = new Vector3(-Mathf.Sign(transform.position.x) * lg.GetWidth() / 2, transform.position.y, transform.position.z);
			int newPosition = (int)(-(lg.GetWidth() - Mathf.Abs(xPosition)) * Mathf.Sign(xPosition));
			xPosition = lg.GetNextXPosition(newPosition);
		}
	}

	public void SetHighAndFall()
	{
		transform.position = new Vector3(xPosition, Camera.main.transform.position.y + 8, 0);
		rb.isKinematic = true;
		UpdateText();
	}

	// Update is called once per frame
	void Update () {
		FloatToFloorHeight();
	}

	void OnBecameInvisible()
	{
		//Set New Variables
		ArrangeNewPosition();
		SetHighAndFall();
	}

	public void UpdateText()
	{
		floorText.text = "" + floorHeight;
	}
}

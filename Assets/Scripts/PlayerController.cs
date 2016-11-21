using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	float moveSpeed = 0.1f, jumpSpeed = 8.0f;
	Text playerCanvasText = null;
	bool jumpAvailable;
	Rigidbody2D rb;
	LevelGen lg;

	int playerXPosition = 0;
	int playerNumber = 1;
	int startPlatformHeight = 1;
	FloorController startPlatform;

	ScoreAndEvaluation sae;

	// Use this for initialization
	void Awake ()
	{
		jumpAvailable = true;
		lg = FindObjectOfType<LevelGen>();
		rb = GetComponent<Rigidbody2D>();
		playerCanvasText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
		startPlatform = GameObject.FindGameObjectWithTag("Floor").GetComponent<FloorController>();
		sae = FindObjectOfType<ScoreAndEvaluation>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//AngleCorrection();
		CheckInput();
	}

	void FixedUpdate()
	{
		transform.position = new Vector3(Mathf.Lerp(transform.position.x, playerXPosition, moveSpeed), transform.position.y, transform.position.z);
	}

	void AngleCorrection()
	{
		Vector3 angle = transform.rotation.eulerAngles;
		if (angle.z > 30.0f && angle.z < 180.0f)
		{
			angle.z = 30.0f;
			rb.angularVelocity = 0.0f;
		} else if (angle.z < 330.0f && angle.z > 180.0f)
		{
			angle.z = 330.0f;
			rb.angularVelocity = 0.0f;
		}
		transform.rotation = Quaternion.Euler(angle);
	}

	void CheckInput()
	{
		//Apply friction on velocity
		rb.velocity = new Vector2(rb.velocity.x * 0.85f, rb.velocity.y);

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (jumpAvailable)
				rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
			else
				rb.velocity = new Vector2(rb.velocity.x, -jumpSpeed);
		}
		/*
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
		}
		*/
		if (!jumpAvailable)
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				playerXPosition = Mathf.Max(playerXPosition - 1, -lg.GetWidth() / 2);
				rb.angularVelocity = rb.angularVelocity + 180.0f;
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				playerXPosition = Mathf.Min(playerXPosition + 1, lg.GetWidth() / 2);
				rb.angularVelocity = rb.angularVelocity - 180.0f;
			}
		}
	}

	public void OnBecameInvisible()
	{
		if (transform.position.y < Camera.main.transform.position.y)
		{
			sae.IncrementFalls();
			ResetToContinue();
		}
	}

	public void OnCollisionEnter2D(Collision2D c2d)
	{
		if (c2d.collider.CompareTag("Floor"))
		{
			if (c2d.contacts.Length > 0 && c2d.contacts[0].point.y < transform.position.y)
			{
				int targetPlatform = startPlatformHeight + playerNumber;
				int thisFloorHeight = c2d.collider.GetComponent<FloorController>().floorHeight;
				if (thisFloorHeight == targetPlatform)
				{
					//next level
					startPlatformHeight = targetPlatform;
					startPlatform = c2d.collider.GetComponent<FloorController>();
					lg.NextLevel(startPlatform);
					c2d.collider.GetComponent<SpriteRenderer>().color = Color.green;
					playerXPosition = 0;
					playerNumber = (int)Random.Range(1.0f, 4.9f);
					playerCanvasText.text = "" + playerNumber;

					//Corrective Bounce
					rb.velocity = new Vector2(0, 4 * (transform.position.y - startPlatform.transform.position.y));
					rb.angularVelocity = 0;

					sae.UpdateScore(startPlatformHeight);

				} else if (thisFloorHeight != startPlatformHeight)
				{
					//reset player for incorrect landing
					c2d.collider.GetComponent<SpriteRenderer>().color = Color.red;
					c2d.collider.enabled = false;
					sae.IncrementWrongs();
				}
			}
		}
	}

	public void OnCollisionStay2D(Collision2D c2d)
	{
		if (c2d.collider.CompareTag("Floor"))
		{
			if (c2d.contacts.Length > 0 && c2d.contacts[0].point.y < transform.position.y)
			{
				jumpAvailable = true;
			}
		}
	}
	
	public void OnCollisionExit2D(Collision2D c2d)
	{
		jumpAvailable = false;
	}

	void ResetToContinue()
	{
		rb.angularVelocity = 0;
		transform.rotation = Quaternion.identity;
		playerXPosition = startPlatform.xPosition;
		transform.position = new Vector3(startPlatform.xPosition, startPlatformHeight - 3, 0);
		GameObject.FindObjectOfType<LevelGen>().Reset();
	}

	bool CheckRange(float value, int min, int max)
	{
		return value > min && value < max;
	}

	public int GetStartPlatformHeight()
	{
		return startPlatformHeight;
	}
}

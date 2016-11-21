using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	float moveSpeed = 0.1f, jumpSpeed = 8.0f;
	Text playerCanvasText = null;
	bool jumpAvailable;
	Rigidbody2D rb;
	LevelGen lg;

	float repeatFloat = 0;
	float repeatInterval = 0.13f;
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

		float repeatDiff = Time.time - repeatFloat;
		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.LeftArrow) && repeatDiff > repeatInterval)
		{
			playerXPosition = Mathf.Max(playerXPosition - 1, -lg.GetWidth() / 2);
			rb.angularVelocity = rb.angularVelocity + 180.0f;
			repeatFloat += repeatInterval;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.RightArrow) && repeatDiff > repeatInterval)
		{
			playerXPosition = Mathf.Min(playerXPosition + 1, lg.GetWidth() / 2);
			rb.angularVelocity = rb.angularVelocity - 180.0f;
			repeatFloat += repeatInterval;
		}
		if (!(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
		{
			repeatFloat = Time.time;
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
					sae.SetResults(startPlatformHeight, playerNumber, targetPlatform, true);

					//next level
					startPlatformHeight = targetPlatform;
					startPlatform = c2d.collider.GetComponent<FloorController>();
					lg.NextLevel(startPlatform);
					c2d.collider.GetComponent<SpriteRenderer>().color = Color.green;
					transform.position = new Vector3(transform.position.x, c2d.collider.transform.position.y + 0.5f, transform.position.z);
					transform.rotation = Quaternion.identity;
					playerXPosition = 0;
					playerNumber = (int)Random.Range(1.0f, 6.9f);
					playerCanvasText.text = "" + playerNumber;

					rb.velocity = Vector2.up * 1.3f;
					rb.angularVelocity = 0;
				} else if (thisFloorHeight != startPlatformHeight)
				{
					sae.SetResults(startPlatformHeight, playerNumber, thisFloorHeight, false);

					rb.velocity = Vector2.up * 0.3f;
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
		lg.Reset();
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

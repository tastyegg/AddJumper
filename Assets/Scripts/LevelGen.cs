using UnityEngine;
using System.Collections;

public class LevelGen : MonoBehaviour {

	public FloorController[] fcs = null;
	int width = 12;
	int floorCount = 9;

	// Use this for initialization
	void Start () {
		LoadLevel();
	}
	
	public int GetWidth()
	{
		return width;
	}

	void LoadLevel()
	{
		FloorController fc = GameObject.FindGameObjectWithTag("Floor").GetComponent<FloorController>();
		fcs = new FloorController[floorCount];

		fcs[0] = fc;
		fcs[0].floorHeight = 1;

		int[] scrambleArray = new int[floorCount];

		scrambleArray[0] = fcs[0].xPosition;

		for (int i = 1; i < floorCount; i++)
		{
			int value = (int)(Random.value * width) - width / 2;
			while (Contains(scrambleArray, value))
			{
				value = (int)(Random.value * width) - width / 2;
			}
			scrambleArray[i] = value;
		}


		for (int i = 1; i < floorCount; i++)
		{
			fcs[i] = Instantiate(fc);
			fcs[i].floorHeight = i + 1;

			fcs[i].xPosition = scrambleArray[i];
			fcs[i].SetHighAndFall();
		}
	}

	public void Reset()
	{
	}

	public void NextLevel(FloorController playerPlatform)
	{
		int correctHeight = playerPlatform.floorHeight;
		Vector3 position = playerPlatform.transform.position;

		for (int i = 0; i < fcs.Length; i++)
		{
			fcs[i].GetComponent<Collider2D>().enabled = true;
			fcs[i].GetComponent<SpriteRenderer>().color = Color.white;
		}

		//Set landed platform to 0 x-position
		int xShift = Mathf.RoundToInt(position.x);
		for (int i = 0; i < fcs.Length; i++)
		{
			fcs[i].xPosition = fcs[i].xPosition - xShift;
		}

		//Translate the camera up (perferably use another class object for smooth translating)
		//Camera.main.transform.Translate(0, (correctHeight - 1) - Camera.main.transform.position.y, 0);
		Camera.main.GetComponent<CameraController>().SetDestination(correctHeight - 1);
	}
	

	bool Contains(int[] array, int value)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == value)
				return true;
		}
		return false;
	}

	public int GetEmptyXPosition()
	{
		int[] xPositions = new int[fcs.Length];
		for (int i = 0; i < fcs.Length; i++)
		{
			xPositions[i] = fcs[i].xPosition;
		}
		ArrayList availablePositions = new ArrayList();
		for (int i = -width / 2; i <= width / 2; i++)
		{
			if (!Contains(xPositions, i))
			{
				availablePositions.Add(i);
			}
		}
		return (int)availablePositions[(int)Random.Range(0, availablePositions.Count)];
	}


	public int GetNextXPosition(int setValue)
	{
		int[] xPositions = new int[fcs.Length];
		for (int i = 0; i < fcs.Length; i++)
		{
			xPositions[i] = fcs[i].xPosition;
		}
		for (int i = setValue; Mathf.Abs(i) <= width / 2; i -= (int)Mathf.Sign(setValue))
		{
			if (!Contains(xPositions, i))
			{
				//Next spot available
				return i;
			}
		}
		//No alternative
		return setValue;
	}

	public int GetNextHeightLevel()
	{
		int maximumHeight = 0;
		for (int i = 0; i < fcs.Length; i++)
		{
			maximumHeight = Mathf.Max(fcs[i].floorHeight, maximumHeight);
		}
		return maximumHeight + 1;
	}
}

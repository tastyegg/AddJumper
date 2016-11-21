using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreAndEvaluation : MonoBehaviour {
	public Text evaluation;
	public Text scoreboard;

	int wrongPlatforms;
	int errorFalls;

	string numberBoard = "Floor 1";
	bool correctness;

	float timeFloat;
	float textFlow = 0.12f;

	void Start () {
		wrongPlatforms = 0;
		errorFalls = 0;
		UpdateEvaluation();
	}

	void Update()
	{
		float timeDiff = Time.time - timeFloat;

		scoreboard.color = Color.black;
		if (scoreboard.text == numberBoard)
		{
			if (timeDiff < 1.0f)
			{
				if (correctness)
					scoreboard.color = Color.green;
				else
					scoreboard.color = Color.red;
			} else if (numberBoard[0] != 'F')
			{
				string[] splits = numberBoard.Split(' ');
				numberBoard = "Floor " + splits[splits.Length - 1];
				scoreboard.text = numberBoard;
			}
		} else if (timeDiff > textFlow)
		{
			scoreboard.text = numberBoard.Substring(0, scoreboard.text.Length + 1);
			timeFloat += textFlow;
		}
	}

	public void SetResults(int floorNumber, int playerNumber, int resultFloor, bool correctness)
	{
		if (correctness)
			numberBoard = floorNumber + " + " + playerNumber + " = " + resultFloor;
		else
			numberBoard = floorNumber + " + " + playerNumber + " ≠ " + resultFloor;
		this.correctness = correctness;

		timeFloat = Time.time;
		scoreboard.text = "";
	}

	public void IncrementWrongs()
	{
		wrongPlatforms++;
		UpdateEvaluation();
	}

	public void IncrementFalls()
	{
		errorFalls++;
		UpdateEvaluation();
	}

	void UpdateEvaluation()
	{
		evaluation.text = "Wrong Platforms: " + wrongPlatforms + "\nFalls: " + errorFalls;
	}
}

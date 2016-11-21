using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreAndEvaluation : MonoBehaviour {
	public Text evaluation;
	public Text scoreboard;

	int wrongPlatforms;
	int errorFalls;
	
	void Start () {
		wrongPlatforms = 0;
		errorFalls = 0;
		UpdateEvaluation();
	}

	public void UpdateScore(int score)
	{
		scoreboard.text = score.ToString();
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

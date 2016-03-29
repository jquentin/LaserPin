using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour {

	#region Singleton
	private static ScoreManager _instance;
	public static ScoreManager instance
	{
		get
		{
			if (_instance == null)
				_instance = FindObjectOfType<ScoreManager>();
			return _instance;
		}
	}
	#endregion

	public Transform scoreContainer;
	public ScoreLabel scoreLabelPrefab;

	private List<ScoreLabel> currentLabels = new List<ScoreLabel>();

	public void CreateScores(List<Team> currentTeams)
	{
		foreach (ScoreLabel sl in currentLabels)
		{
			if (sl != null)
				Destroy(sl.gameObject);
		}
		currentLabels.Clear();
		for (int i = 0 ; i < currentTeams.Count ; i++)
		{
			Team currentTeam = currentTeams[i];
			currentTeam.teamIndex = i;
			ScoreLabel label =  Instantiate(scoreLabelPrefab) as ScoreLabel;
			label.transform.parent = scoreContainer;
			label.Initialize(i, currentTeam.color);
			currentTeam.scoreLabel = label;
			currentLabels.Add(label);
		}
	}
}

﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Team
{
	[NonSerialized]
	public int teamIndex;
	[NonSerialized]
	public TextMesh scoreLabel;
	[NonSerialized]
	public List<LaserNode> nodes = new List<LaserNode>();
	private int _score = 0;
	public int score
	{
		get
		{
			return _score;
		}
		set
		{
			_score = value;
			scoreLabel.text = _score.ToString();
		}
	}

	public Color color;
}


public class GameController : MonoBehaviour {

	#region Singleton
	private static GameController _instance;
	public static GameController instance
	{
		get
		{
			if (_instance == null)
				_instance = FindObjectOfType<GameController>();
			return _instance;
		}
	}
	#endregion

	public List<Team> teams;
	public LaserNode nodePrefab;
	public int nbNodesAtStart = 5;
	public float delayBetweenSpawns = 2f;
	public int nbAttemptsAtCreating = 5;

	Vector3 _bottomLeft = Vector3.one * float.NaN;
	Vector3 bottomLeft
	{
		get
		{
			if (float.IsNaN(_bottomLeft.x))
				_bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
			return _bottomLeft;
		}
	}
	Vector3 _topRight = Vector3.one * float.NaN;
	Vector3 topRight
	{
		get
		{
			if (float.IsNaN(_topRight.x))
				_topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
			return _topRight;
		}
	}

	float left { get { return bottomLeft.x; } }
	float right { get { return topRight.x; } }
	float bottom { get { return bottomLeft.y; } }
	float top { get { return topRight.y; } }

	Vector3 randomPosition
	{
		get
		{
			return new Vector3(
				UnityEngine.Random.Range(left, right),
				UnityEngine.Random.Range(bottom, top));
		}
	}

	void InitTeams()
	{
		for (int i = 0 ; i < teams.Count ; i++)
		{
			teams[i].teamIndex = i;
			GameObject go = new GameObject("Score Player " + i);
			teams[i].scoreLabel = go.AddComponent<TextMesh>();
			teams[i].scoreLabel.color = teams[i].color;
			teams[i].scoreLabel.fontSize = 20;
			teams[i].scoreLabel.transform.localScale = Vector3.one * 0.5f;
			if (i == 0)
			{
				teams[i].scoreLabel.anchor = TextAnchor.UpperLeft;
				teams[i].scoreLabel.transform.position = new Vector3(left, top);
			}
			else if (i == 1)
			{
				teams[i].scoreLabel.anchor = TextAnchor.UpperRight;
				teams[i].scoreLabel.transform.position = new Vector3(right, top);
			}
			else if (i == 2)
			{
				teams[i].scoreLabel.anchor = TextAnchor.LowerLeft;
				teams[i].scoreLabel.transform.position = new Vector3(left, bottom);
			}
			else if (i == 3)
			{
				teams[i].scoreLabel.anchor = TextAnchor.LowerRight;
				teams[i].scoreLabel.transform.position = new Vector3(right, bottom);
			}
			teams[i].score = 0;
		}
	}

	IEnumerator Start()
	{
		InitTeams();
		for (int i = 0 ; i < nbNodesAtStart ; i++)
		{
			CreateNodes();
		}
		while(true)
		{
			CreateNodes();
			yield return new WaitForSeconds(delayBetweenSpawns);
		}
	}

	void CreateNodes()
	{
		for (int i = 0 ; i < teams.Count ; i++)
		{
			CreatedNode(teams[i]);
		}
	}

	void CreatedNode(Team team)
	{
		bool foundPlace = false;
		Vector3 pos = Vector3.zero;
		for (int j = 0 ; j < nbAttemptsAtCreating ; j++)
		{
			pos = randomPosition;
			if (Physics2D.OverlapCircle(pos, 0.7f) == null)
			{
				foundPlace = true;
				break;
			}
		}
		if (foundPlace)
		{
			LaserNode createdNode = Instantiate(nodePrefab, pos, nodePrefab.transform.rotation) as LaserNode;
			createdNode.Init(team);
			team.nodes.Add(createdNode);
		}
	}

	void CreatedNodes(Team team, int number)
	{
		for (int j = 0 ; j < number ; j++)
		{
			CreatedNode(team);
		}
	}

	public void UpdateNodes()
	{
		foreach(Team team in teams)
		{
			team.nodes.RemoveAll(node => node == null || node.dead);
			if (team.nodes.Count < nbNodesAtStart)
				CreatedNodes(team, nbNodesAtStart - team.nodes.Count);
		}
	}

}

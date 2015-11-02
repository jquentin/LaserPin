using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Team
{
	[NonSerialized]
	public int teamIndex;
	[NonSerialized]
	public TextMesh scoreLabel;
	[NonSerialized]
	public List<LaserNode> nodes = new List<LaserNode>();

	public List<LaserNode> availableNodes
	{
		get
		{
			return nodes.FindAll(node => node.isAvailable);
		}
	}

	public List<LaserNode> currentPaths
	{
		get
		{
			List<LaserNode> res = new List<LaserNode>();
			foreach(LaserNode node in nodes)
			{
				if (node.nodes.Count > 0)
					res.Add(node);
			}
			return res;
		}
	}

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

	public enum Mode { Linear, Binary, Fibonacci }
	public Mode mode;

	public List<Team> teams;
	public LaserNode nodePrefab;
	public int nbNodesAtStart = 5;
	public float delayBetweenSpawns = 2f;
	public int nbAttemptsAtCreating = 5;
	public int timeGame = 30;
	public int countdownNbSeconds = 5;
	private float timeStart;
	private bool isPlaying = false;
	
	Vector3 _gameBottomLeft = Vector3.one * float.NaN;
	Vector3 gameBottomLeft
	{
		get
		{
			if (float.IsNaN(_gameBottomLeft.x))
				_gameBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.05f, Screen.height * 0.1f));
			return _gameBottomLeft;
		}
	}
	Vector3 _gameTopRight = Vector3.one * float.NaN;
	Vector3 gameTopRight
	{
		get
		{
			if (float.IsNaN(_gameTopRight.x))
				_gameTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.95f, Screen.height * 0.9f));
			return _gameTopRight;
		}
	}
	
	float gameLeft { get { return gameBottomLeft.x; } }
	float gameRight { get { return gameTopRight.x; } }
	float gameBottom { get { return gameBottomLeft.y; } }
	float gameTop { get { return gameTopRight.y; } }

	TextMesh _countdown;
	TextMesh countdown
	{
		get
		{
			if (_countdown == null)
				_countdown = GetComponent<TextMesh>();
			return _countdown;
		}
	}
	
	Vector3 _bottomLeft = Vector3.one * float.NaN;
	Vector3 bottomLeft
	{
		get
		{
			if (float.IsNaN(_bottomLeft.x))
				_bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0f, Screen.height * 0f));
			return _bottomLeft;
		}
	}
	Vector3 _topRight = Vector3.one * float.NaN;
	Vector3 topRight
	{
		get
		{
			if (float.IsNaN(_topRight.x))
				_topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 1f, Screen.height * 1f));
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
				UnityEngine.Random.Range(gameLeft, gameRight),
				UnityEngine.Random.Range(gameBottom, gameTop));
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

	void OnTapToPlay(Gesture gesture)
	{
		EasyTouch.On_SimpleTap -= OnTapToPlay;
		isSubscribed = false;
		Play();
	}

	void Play()
	{
		print("Play");
		foreach(Team team in teams)
			team.score = 0;
		isPlaying = true;
		timeStart = Time.time;
		StartCoroutine("GameCoroutine");
	}

	IEnumerator GameCoroutine()
	{
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

	void Start()
	{
		InitTeams();
//		Play ();
	}

	bool isSubscribed = false;

	void Update()
	{
		if (isPlaying && Time.time >= timeStart + (float)timeGame - (float)countdownNbSeconds)
		{
			countdown.text = Mathf.CeilToInt((timeStart + (float)timeGame) - Time.time).ToString();
		}
		else
		{
			countdown.text = string.Empty;
		}
		if (isPlaying && Time.time >= timeStart + (float)timeGame)
			GameOver();
		if (!isPlaying && !isSubscribed)
		{
			EasyTouch.On_SimpleTap += OnTapToPlay;
			isSubscribed = true;
		}
	}

	void GameOver()
	{
		isPlaying = false;
		foreach(LaserNode node in FindObjectsOfType<LaserNode>())
			Destroy(node.gameObject);
		StopCoroutine("GameCoroutine");
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

	public void KeepMinimumNodes(Team team)
	{
		if (team.availableNodes.Count < nbNodesAtStart)
			CreatedNode(team);
	}
	
	static bool FasterLineSegmentIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
		
		Vector2 a = p2 - p1;
		Vector2 b = p3 - p4;
		Vector2 c = p1 - p3;
		
		float alphaNumerator = b.y*c.x - b.x*c.y;
		float alphaDenominator = a.y*b.x - a.x*b.y;
		float betaNumerator  = a.x*c.y - a.y*c.x;
		float betaDenominator  = a.y*b.x - a.x*b.y;
		
		bool doIntersect = true;
		
		if (alphaDenominator == 0 || betaDenominator == 0) {
			doIntersect = false;
		} else {
			
			if (alphaDenominator > 0) {
				if (alphaNumerator < 0 || alphaNumerator > alphaDenominator) {
					doIntersect = false;
					
				}
			} else if (alphaNumerator > 0 || alphaNumerator < alphaDenominator) {
				doIntersect = false;
			}
			
			if (doIntersect && betaDenominator > 0) {
				if (betaNumerator < 0 || betaNumerator > betaDenominator) {
					doIntersect = false;
				}
			} else if (betaNumerator > 0 || betaNumerator < betaDenominator) {
				doIntersect = false;
			}
		}
		
		return doIntersect;
	}

	public List<LaserNode> IsCrossing(LaserNode nodeA, LaserNode nodeB)
	{
		List<LaserNode> res = new List<LaserNode>();
		foreach(Team team in teams)
		{
			if (team == nodeA.team)
				continue;
			foreach(LaserNode path in team.currentPaths)
			{
				for (int i = 0 ; i < path.nodes.Count - 1 ; i++)
				{
					if (FasterLineSegmentIntersection(nodeA.position, nodeB.position, path.nodes[i].position, path.nodes[i+1].position))
					{
						res.Add(path);
						break;
					}
				}
			}
		}
		return res;
	}

	public int GetNbPoints(int index)
	{
		switch (mode)
		{
		case Mode.Binary:
		{
			return 1 << (index - 1);
		}
		case Mode.Fibonacci:
		{
			return Fibo(index);
		}
		case Mode.Linear:
		default:
		{
			return index;
		}
		}
	}

	int Fibo(int a)
	{
		if (a == 1)
			return 1;
		else if (a == 2)
			return 2;
		else
			return Fibo (a - 1) + Fibo (a - 2);
	}

}

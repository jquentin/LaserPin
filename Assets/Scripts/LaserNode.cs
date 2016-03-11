using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
public class LaserNode : MonoBehaviour {

	public Team team;
	[NonSerialized]
	public bool dead = false;
	public ScorePopper scorePopper;

	public delegate void OnNodeTakenHandler(LaserNode newNode, LaserNode rootNode);
	public static OnNodeTakenHandler OnNodeTaken;

	public bool isAvailable
	{
		get
		{
			if (dead)
				return false;
			foreach(LaserNode node in team.nodes)
				if (node.nodes.Contains(this))
					return false;
			return true;
		}
	}

	private ImprovedLineRenderer _lineRenderer;
	private ImprovedLineRenderer lineRenderer
	{
		get
		{
			if (_lineRenderer == null)
				_lineRenderer = GetComponent<ImprovedLineRenderer>();
			if (_lineRenderer == null)
			{
				_lineRenderer = gameObject.AddComponent<ImprovedLineRenderer>();
			}
			_lineRenderer.material = sphereOn.material;
			return _lineRenderer;
		}
	}
	
	private Collider2D _collider;
	public Collider2D collider
	{
		get
		{
			if (_collider == null)
				_collider = GetComponent<Collider2D>();
			return _collider;
		}
	}

	public MeshRenderer sphereOn;
	public MeshRenderer sphereOff;

	private Color color
	{
		set
		{
			sphereOn.material.color = value;
			sphereOn.material.SetColor("_EmissionColor", Color.Lerp(value, Color.black, 0.2f));
			sphereOn.material.EnableKeyword ("_EMISSION");
			sphereOff.material.color = value;
			sphereOff.material.SetColor("_EmissionColor", Color.black);
			sphereOff.material.EnableKeyword ("_EMISSION");
		}
	}

	public Vector2 position
	{
		get
		{
			return transform.position;
		}
	}
	public AudioSource audioSource
	{
		get
		{
			return transform.GetOrAddComponent<AudioSource>();
		}
	}

	[SerializeField]
	public List<LaserNode> nodes = new List<LaserNode>();

	public Instrument instrument
	{
		get
		{
			return GameController.instance.instruments[team.instrument];
		}
	}

	void Start()
	{
		TurnOff();
	}
	
	void Update () 
	{
		if (dead || nodes.Count == 0)
		{
			lineRenderer.SetVertexCount(0);
			return;
		}
	}

	void OnETMouseDown(Gesture gesture)
	{
		Debug.Log("OnETMouseDown " + name);
		nodes.Add(this);
		this.TurnOn();
		if (OnNodeTaken != null)
			OnNodeTaken(this, this);
		instrument.PlayNote(audioSource, nodes.Count);
	}

	void OnETMouseDrag(Gesture gesture)
	{
		Collider2D[] touched = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(gesture.position));
		if (touched != null && touched.Length > 0)
		{
			foreach(Collider2D c in touched)
			{
				LaserNode touchedNode = c.GetComponent<LaserNode>();
				if (touchedNode != null && touchedNode.isAvailable && touchedNode.team == this.team)
				{
					AddNode(touchedNode);
				}
			}
		}
		lineRenderer.SetVertexCount((nodes.Count + 1)*2);
		for(int i = 0 ; i < nodes.Count ; i++)
		{
			LaserNode node = nodes[i];
			lineRenderer.SetPosition(i*2, node.transform.position);
			lineRenderer.SetPosition(i*2+1, node.transform.position);
		}
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(gesture.position);
		mousePos.z = 0f;
		lineRenderer.SetPosition(nodes.Count*2, mousePos);
		lineRenderer.SetPosition(nodes.Count*2+1, mousePos);
	}

	void OnETMouseUp(Gesture gesture)
	{
		nodes.ForEach(node => node.TurnOff());
		if (nodes.Count >= 2)
			StartCoroutine(ValidateNodes());
		else
		{
			nodes.Clear();
		}
	}

	void AddNode(LaserNode node)
	{
		Debug.Log("AddNode " + node.name + " to " + name);
		nodes.Add(node);
		node.TurnOn();
		TakeOverCrossedTeams();
		GameController.instance.KeepMinimumNodes(team);
		instrument.PlayNote(audioSource, nodes.Count);
		if (OnNodeTaken != null)
			OnNodeTaken(node, this);
	}
	
	void TakeOverCrossedTeams()
	{
		LaserNode nodeA = nodes[nodes.Count - 2];
		LaserNode nodeB = nodes[nodes.Count - 1];
		foreach(LaserNode path in GameController.instance.IsCrossing(nodeA, nodeB))
		{
			path.GetTakenOver(team);
			StartCoroutine(instrument.PlayTakeOverSound(transform.GetOrAddComponent<AudioSource>()));
		}
	}

	public void GetTakenOver(Team team)
	{
		foreach(LaserNode node in nodes)
			node.Init(team);
		StartCoroutine(instrument.PlayGetTakenOverSound(transform.GetOrAddComponent<AudioSource>()));
	}

	void Validate(int scoreGiven)
	{
		collider.enabled = false;
		iTween.ScaleBy(sphereOff.gameObject, iTween.Hash(
			"amount", 2f * Vector3.one,
			"time", 0.2f,
			"easetype", iTween.EaseType.linear));
		iTween.FadeTo(sphereOff.gameObject, iTween.Hash(
			"alpha", 0f,
			"time", 0.2f,
			"easetype", iTween.EaseType.linear,
			"oncomplete", "Hide",
			"oncompletetarget", gameObject));
		scorePopper.Pop(scoreGiven);
	}

	void Hide()
	{
		sphereOff.enabled = false;
		sphereOn.enabled = false;
	}

	void TurnOn()
	{
//		UpdateAlphaForEnlight(1f);
		sphereOn.gameObject.SetActive(true);
		sphereOff.gameObject.SetActive(false);
	}

	void TurnOff()
	{
//		UpdateAlphaForEnlight(0f);
		sphereOn.gameObject.SetActive(false);
		sphereOff.gameObject.SetActive(true);
	}

	void UpdateAlphaForEnlight(float value)
	{
//		Color c = meshRenderer.material.GetColor("emission");
//		c.a = value;
		Color c = new Color(team.color.r * value, team.color.g * value, team.color.b * value);
		sphereOn.material.SetColor("_EmissionColor", c);
	}

	void DestroyNode()
	{
		dead = true;
//		Destroy(gameObject);
	}

	IEnumerator ValidateNodes()
	{
		Debug.Log("nodes.Count =" + nodes.Count);
		if (nodes.Count > 5)
		{
			print ("wow");
		}
		List<LaserNode> nodesCopy = new List<LaserNode>(nodes);
		nodes.Clear();
		GameController.instance.UpdateNodes();
		nodesCopy.ForEach(node => node.DestroyNode());
		for(int i = 0 ; i < nodesCopy.Count ; i++)
		{
			Debug.Log("i=" + i);
			int nodePoints = GameController.instance.GetNbPoints(i + 1);
			Debug.Log("nodePoints=" + nodePoints);
			nodesCopy[i].Validate(nodePoints);
			instrument.PlayNote(audioSource, i);
			audioSource.PlayOneShot(GameController.instance.validationSound);
			team.score += (nodePoints);
			yield return new WaitForSeconds(0.05f);
		}
//		nodes.Clear();
	}

	public void Init(Team team)
	{
		this.team = team;
		color = team.color;
		scorePopper.Init(team.color);
		gameObject.name = "Node-" + team.teamIndex + ":" + team.counterSpawn;
//		scorePopper.
	}

}

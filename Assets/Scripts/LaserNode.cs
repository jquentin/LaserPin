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

	private CustomLineRenderer _lineRenderer;
	private CustomLineRenderer lineRenderer
	{
		get
		{
			if (_lineRenderer == null)
				_lineRenderer = GetComponent<CustomLineRenderer>();
			if (_lineRenderer == null)
			{
				_lineRenderer = gameObject.AddComponent<CustomLineRenderer>();
			}
//			if (sphereOn != null)
//				_lineRenderer.material = sphereOn.material;
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

	public Sprite faceOn;
	public Sprite faceOff;
	public SpriteRenderer face;
	public List<GameObject> objectsOn;
	public List<GameObject> objectsOff;

	public List<AudioClip> soundsOn;
	public List<AudioClip> soundsOff;

	private Color color
	{
		set
		{
			Colorable colorable = GetComponentInChildren<Colorable>();
			if (colorable != null)
				colorable.SetColor(value);
			else
			{
				sphereOn.material.color = value;
				sphereOn.material.SetColor("_EmissionColor", Color.Lerp(value, Color.black, 0.2f));
				sphereOn.material.EnableKeyword ("_EMISSION");
				sphereOff.material.color = value;
				sphereOff.material.SetColor("_EmissionColor", Color.black);
				sphereOff.material.EnableKeyword ("_EMISSION");
			}
			scorePopper.Init(team.color);
			lineRenderer.color = team.color;
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

	public float radius
	{
		get
		{
			return transform.localScale.x * GetComponent<CircleCollider2D>().radius;
		}
	}

	[SerializeField]
	public List<LaserNode> nodes = new List<LaserNode>();

	public Instrument instrument
	{
		get
		{
			return team.instrument;
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
//		instrument.PlayNote(audioSource, nodes.Count);
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
		lineRenderer.SetVertexCount(nodes.Count + 1);
		for(int i = 0 ; i < nodes.Count ; i++)
		{
			LaserNode node = nodes[i];
			lineRenderer.SetPosition(i, node.transform.position);
		}
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(gesture.position);
		mousePos.z = 0f;
		lineRenderer.SetPosition(nodes.Count, mousePos);
	}

	void OnETMouseUp(Gesture gesture)
	{
		if (nodes.Count >= 2)
			StartCoroutine(ValidateNodes());
		else
		{
			foreach(LaserNode node in nodes)
				node.TurnOff();
			nodes.Clear();
		}
	}

	void AddNode(LaserNode node)
	{
		Debug.Log("AddNode " + node.name + " to " + name);
		nodes.Add(node);
		node.TurnOn();
		TakeOverCrossedTeams();
		if (GameController.instance.isPlaying)
			GameController.instance.KeepMinimumNodes(team);
//		instrument.PlayNote(audioSource, nodes.Count);
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
		iTween.ScaleTo(gameObject, iTween.Hash(
			"scale", 2f * transform.localScale,
			"time", 0.2f,
			"easetype", iTween.EaseType.linear));
		iTween.FadeTo(gameObject, iTween.Hash(
			"alpha", 0f,
			"time", 0.1f,
			"easetype", iTween.EaseType.linear,
			"oncomplete", "Hide",
			"oncompletetarget", gameObject));
		scorePopper.Pop(scoreGiven);
	}

	void Hide()
	{
//		sphereOff.gameObject.SetActive(false);
//		sphereOn.gameObject.SetActive(false);
	}

	void TurnOn()
	{
//		sphereOn.gameObject.SetActive(true);
//		sphereOff.gameObject.SetActive(false);
		face.sprite = faceOn;
		foreach(GameObject go in objectsOn)
			go.SetActive(true);
		foreach(GameObject go in objectsOff)
			go.SetActive(false);
//		GetComponent<ScaleLoop>().Wobble(false);
		audioSource.PlayOneShotControlled(soundsOn);
	}

	void TurnOff()
	{
//		sphereOn.gameObject.SetActive(false);
//		sphereOff.gameObject.SetActive(true);
		face.sprite = faceOff;
		foreach(GameObject go in objectsOn)
			go.SetActive(false);
		foreach(GameObject go in objectsOff)
			go.SetActive(true);
//		GetComponent<ScaleLoop>().Wobble(true);
		audioSource.PlayOneShotControlled(soundsOff);
	}

	void DestroyNode()
	{
		dead = true;
		GetComponentInChildren<FaceControl>().enabled = false;
	}

	IEnumerator ValidateNodes()
	{
		List<LaserNode> nodesCopy = new List<LaserNode>(nodes);
		nodes.Clear();
		GameController.instance.UpdateNodes();
		nodesCopy.ForEach(node => node.DestroyNode());
		for(int i = 0 ; i < nodesCopy.Count ; i++)
		{
			int nodePoints = GameController.instance.GetNbPoints(i + 1);
			nodesCopy[i].Validate(nodePoints);
			instrument.PlayNote(audioSource, i);
			audioSource.PlayOneShot(GameController.instance.validationSound);
			if (GameController.instance.isPlaying)
				team.score += (nodePoints);
			yield return new WaitForSeconds(0.05f);
		}
	}

	public void Init(Team team, int index = 0)
	{
		this.team = team;
		color = team.color;
		gameObject.name = "Node-" + team.teamIndex + ":" + team.counterSpawn;
		foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>(true))
			sr.sortingOrder = sr.sortingOrder + index * 200;
	}

	void OnDrawGizmosSelected () 
	{
		Gizmos.color = new Color(1f, 0f, 0f, 0.4f);
		Gizmos.DrawSphere(transform.position,radius);
	}

}

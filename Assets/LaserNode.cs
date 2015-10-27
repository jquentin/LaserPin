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

	private LineRenderer _lineRenderer;
	private LineRenderer lineRenderer
	{
		get
		{
			if (_lineRenderer == null)
				_lineRenderer = GetComponent<LineRenderer>();
			if (_lineRenderer == null)
			{
				_lineRenderer = gameObject.AddComponent<LineRenderer>();
			}
			_lineRenderer.material = GetComponent<MeshRenderer>().material;
			_lineRenderer.useWorldSpace = true;
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

	private Color color
	{
		set
		{
			GetComponent<MeshRenderer>().material.color = value;
		}
	}

	[SerializeField]
	private List<LaserNode> nodes = new List<LaserNode>();

	// Use this for initialization
	void Start () {
		
	}
	
	void Update () 
	{
		if (nodes.Count == 0)
		{
			lineRenderer.SetVertexCount(0);
			return;
		}
		lineRenderer.SetVertexCount(nodes.Count + 1);
		for(int i = 0 ; i < nodes.Count ; i++)
		{
			LaserNode node = nodes[i];
			lineRenderer.SetPosition(i, node.transform.position);
		}
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0f;
		lineRenderer.SetPosition(nodes.Count, mousePos);
	}

	void OnMouseDown()
	{
		nodes.Add(this);
	}

	void OnMouseDrag()
	{
		Collider2D[] touched = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		if (touched != null && touched.Length > 0)
		{
			foreach(Collider2D c in touched)
			{
				LaserNode touchedNode = c.GetComponent<LaserNode>();
				if (touchedNode != null && !nodes.Contains(touchedNode) && touchedNode.team == this.team)
				{
					nodes.Add(touchedNode);
				}
			}
		}
	}

	void OnMouseUp()
	{
		if (nodes.Count >= 2)
			StartCoroutine(ValidateNodes());
		else
			nodes.Clear();
	}

	void Validate()
	{
		collider.enabled = false;
		iTween.ScaleBy(gameObject, iTween.Hash(
			"amount", 2f * Vector3.one,
			"time", 0.2f,
			"easetype", iTween.EaseType.linear));
		iTween.FadeTo(gameObject, iTween.Hash(
			"alpha", 0f,
			"time", 0.2f,
			"easetype", iTween.EaseType.linear));
	}

	void DestroyNode()
	{
		dead = true;
		Destroy(gameObject);
	}

	IEnumerator ValidateNodes()
	{
		for(int i = 0 ; i < nodes.Count ; i++)
		{
			nodes[i].Validate();
			team.score += (i + 1);
			yield return new WaitForSeconds(0.05f);
		}
		nodes.ForEach(node => node.DestroyNode());
		GameController.instance.UpdateNodes();
//		nodes.Clear();
	}

	public void Init(Team team)
	{
		this.team = team;
		color = team.color;
	}

}

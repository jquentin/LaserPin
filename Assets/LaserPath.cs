using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class LaserPath : MonoBehaviour {

	public int team;

	private static LaserPath[] _instances;
	private static LaserPath[] instances
	{
		get
		{
			if (_instances == null)
			{
				LaserPath[] paths = FindObjectsOfType<LaserPath>();
				_instances = new LaserPath[paths.Length];
				foreach(LaserPath path in paths)
					_instances[path.team] = path;
			}
			return _instances;
		}
	}
	public static LaserPath GetPath(int team)
	{
		return instances[team];
	}

	private LineRenderer _lineRenderer;
	private LineRenderer lineRenderer
	{
		get
		{
			if (_lineRenderer == null)
				_lineRenderer = GetComponent<LineRenderer>();
			return _lineRenderer;
		}
	}
	private List<LaserNode> nodes;
	private int fingerId;

	public void AddNode(LaserNode node)
	{

	}

	void Start () 
	{

	}

}

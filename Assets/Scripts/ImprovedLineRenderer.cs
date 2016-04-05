using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImprovedLineRenderer : MonoBehaviour {

	List<LineRenderer> lines = new List<LineRenderer>();

	public Material material;
	private Material materialInstance;
	public bool useWorldSpace;
	public float startWidth;
	public float endWidth;

	public Color color
	{
		set
		{
			materialInstance.color = value;
		}
	}

	void Awake()
	{
		materialInstance = new Material(material);
	}

	public void SetVertexCount (int count) 
	{
		foreach(LineRenderer lr in lines)
			Destroy(lr.gameObject);
		lines.Clear();
		for (int i = 0 ; i < count - 1 ; i++)
		{
			GameObject go = new GameObject("LineRenderer" + i);
			go.transform.parent = transform;
			LineRenderer lr = go.AddComponent<LineRenderer>();
			lr.SetVertexCount(2);
			lr.material = materialInstance;
			lr.useWorldSpace = useWorldSpace;
			lr.SetWidth(startWidth, endWidth);
			lines.Add(lr);
		}
	}

	public void SetPosition (int i, Vector3 pos) 
	{
		if (i > 0)
			lines[i - 1].SetPosition(1, pos);
		if (i < lines.Count)
			lines[i].SetPosition(0, pos);
	}
}

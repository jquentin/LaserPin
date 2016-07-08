using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class CustomLineRenderer : MonoBehaviour {


	public abstract Color color
	{
		set;
	}

	public abstract void SetVertexCount (int count);

	public abstract void SetPosition (int i, Vector3 pos);

}

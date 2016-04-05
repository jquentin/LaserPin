using UnityEngine;
using System.Collections;

public class ScaleLoop : TweenLoop {

	protected override string allAxisPropertyName {
		get {
			return "scale";
		}
	}

	protected override Vector3 coordinatesValues
	{
		get
		{
			return transform.localScale;
		}
		set
		{
			transform.localScale = value;
		}
	}
	
	protected override void Tween (GameObject go, Hashtable hash)
	{
		if (hash.ContainsKey(allAxisPropertyName))
			hash[allAxisPropertyName] = ((float)hash[allAxisPropertyName]) * Vector3.one;
		iTween.ScaleTo(go, hash);
	}
}

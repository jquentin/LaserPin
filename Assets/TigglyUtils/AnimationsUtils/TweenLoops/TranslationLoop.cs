using UnityEngine;
using System.Collections;

public class TranslationLoop : TweenLoop {

	protected override string allAxisPropertyName {
		get {
			return "position";
		}
	}
	
	protected override Vector3 coordinatesValues
	{
		get
		{
			return (isLocal ? transform.localPosition : transform.position);
		}
		set
		{
			if (isLocal) 
				transform.localPosition = value; 
			else 
				transform.position = value;
		}
	}

	protected override void Tween (GameObject go, Hashtable hash)
	{
		iTween.MoveTo(go, hash);
	}

}

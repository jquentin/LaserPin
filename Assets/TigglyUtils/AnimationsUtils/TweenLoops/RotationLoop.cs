using UnityEngine;
using System.Collections;

public class RotationLoop : TweenLoop {

	protected override string allAxisPropertyName {
		get {
			return "rotation";
		}
	}

	protected override Vector3 coordinatesValues
	{
		get
		{
			return (isLocal ? transform.localRotation.eulerAngles : transform.rotation.eulerAngles);
		}
		set
		{
			if (isLocal) 
				transform.localRotation = Quaternion.Euler(value); 
			else 
				transform.rotation = Quaternion.Euler(value);
		}
	}
	
	protected override void Tween (GameObject go, Hashtable hash)
	{
		iTween.RotateTo(go, hash);
	}
}

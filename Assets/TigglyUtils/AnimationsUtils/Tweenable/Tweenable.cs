using UnityEngine;
using System.Collections;

public abstract class Tweenable : MonoBehaviour {

	protected virtual string TweenName
	{
		get
		{
			return GetType().Name;
		}
	}

	public void StopTween()
	{
		iTween.StopByName(gameObject, TweenName);
	}

}

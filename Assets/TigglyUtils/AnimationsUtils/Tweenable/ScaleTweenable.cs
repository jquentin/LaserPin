using UnityEngine;
using System.Collections;

public class ScaleTweenable : Tweenable {

	public float scale { get; private set; }
	public bool ignoreTimeScale = false;
	
	void Awake()
	{
		if (transform.localScale.x != transform.localScale.y)
			Debug.LogWarning("Non uniform scale on ScaleTweenable object");
		scale = transform.localScale.x;
	}
	
	public void TweenScaleFrom(float from, float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		transform.localScale = Vector3.one * from;
		iTween.ScaleTo(gameObject, iTween.Hash(
			"scale", Vector3.one * to,
			"time", time,
			"delay", delay,
			"easetype", easeType,
			"name", TweenName,
			"ignoreTimeScale", ignoreTimeScale));
	}
	
	public void TweenScale(float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		iTween.ScaleTo(gameObject, iTween.Hash(
			"scale", Vector3.one * to,
			"time", time,
			"delay", delay,
			"easetype", easeType,
			"name", TweenName,
			"ignoreTimeScale", ignoreTimeScale));
	}
	
	public void TweenXScaleFrom(float from, float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		transform.localScale = Vector3.one * from;
		iTween.ScaleTo(gameObject, iTween.Hash(
			"x", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenXScale(float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		iTween.ScaleTo(gameObject, iTween.Hash(
			"x", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenYScaleFrom(float from, float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		transform.localScale = Vector3.one * from;
		iTween.ScaleTo(gameObject, iTween.Hash(
			"y", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenYScale(float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		iTween.ScaleTo(gameObject, iTween.Hash(
			"y", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
}

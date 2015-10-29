using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class SizeTweenable : Tweenable {

	private static readonly string TWEEN_NAME = "SizeTweenable";

	public bool ignoreTimeScale = false;

	private Camera camera;
	public float size { get; private set; }
	
	void Awake()
	{
		camera = GetComponent<Camera>();
		size = camera.orthographicSize;
	}
	
	public void UpdateSize(float value)
	{
		camera.orthographicSize = value;
		size = value;
	}

	public void StopTween()
	{
		iTween.StopByName(gameObject, TWEEN_NAME);
	}
	
	public void TweenSizeFrom(float from, float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		StopTween();
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", from,
			"to", to,
			"time", time,
			"delay", delay,
			"onupdate", "UpdateSize",
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenSize(float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		StopTween();
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", camera.orthographicSize,
			"to", to,
			"time", time,
			"delay", delay,
			"onupdate", "UpdateSize",
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class VolumeTweenable : Tweenable {

	private static readonly string TWEEN_NAME = "VolumeTweenable";

	public bool ignoreTimeScale = false;

	private AudioSource audioSource;
	public float volume { get; private set; }
	
	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		volume = audioSource.volume;
	}
	
	public void UpdateSize(float value)
	{
		audioSource.volume = value;
		volume = value;
	}

	public void StopTween()
	{
		iTween.StopByName(gameObject, TWEEN_NAME);
	}
	
	public void TweenFrom(float from, float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
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
	
	public void Tween(float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		StopTween();
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", audioSource.volume,
			"to", to,
			"time", time,
			"delay", delay,
			"onupdate", "UpdateSize",
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
}

using UnityEngine;
using System.Collections;

public class ColorTweenable : Tweenable {
	
	private SpriteRenderer[] spriteRenderers;
	private MeshRenderer[] meshRenderers;
	public Color color { get; private set; }
	
	void Awake()
	{
		color = Color.white;
		spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
		meshRenderers = GetComponentsInChildren<MeshRenderer>();
	}
	
	public void UpdateColor(Color value)
	{
		foreach(SpriteRenderer r in spriteRenderers)
			r.color = value;
		foreach(MeshRenderer r2 in meshRenderers)
			r2.material.color = value;
		color = value;
	}
	
	public void TweenColorFrom(Color from, Color to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", from,
			"to", to,
			"time", time,
			"delay", delay,
			"onupdate", "UpdateColor",
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenColor(Color to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", color,
			"to", to,
			"time", time,
			"delay", delay,
			"onupdate", "UpdateColor",
			"name", TweenName,
			"easetype", easeType));
	}
}

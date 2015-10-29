//#define TMProFadeEnabled
// Add TMProFadeEnabled to your Player Settings Scripting Define Symbols to enable fading of Text Mesh Pro components

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if TMProFadeEnabled
using TMPro;
#endif

public class AlphaTweenable : Tweenable {

	private SpriteRenderer[] spriteRenderers;
	private List<MeshRenderer> meshRenderers;
#if TMProFadeEnabled
	private TextMeshPro[] textMeshProRenderers;
#endif
	private UIWidget[] NGUIWidgets;
	private AnimatedColor[] animatedColorComponents;

	private Collider[] _colliders = null;
	private Collider[] colliders
	{
		get
		{
			if (_colliders == null)
				_colliders = GetComponentsInChildren<Collider>();
			return _colliders;
		}
	}
	public float alpha { get; private set; }
	public bool disableColliders = false;
	public bool ignoreTimeScale = false;

	void Awake()
	{
		spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
		meshRenderers = new List<MeshRenderer> (GetComponentsInChildren<MeshRenderer>());
#if TMProFadeEnabled
		textMeshProRenderers = GetComponentsInChildren<TextMeshPro>();
		meshRenderers.RemoveAll(delegate(MeshRenderer obj) {
			return obj.GetComponent<TextMeshPro>() != null;
		});
#endif
		NGUIWidgets = GetComponentsInChildren<UIWidget>();
		animatedColorComponents = GetComponentsInChildren<AnimatedColor>();

		if (spriteRenderers.Length > 0)
			alpha = spriteRenderers[0].color.a;
		else if (meshRenderers.Count > 0)
			alpha = meshRenderers[0].material.color.a;
#if TMProFadeEnabled
		else if (textMeshProRenderers.Length > 0)
			alpha = textMeshProRenderers[0].color.a;
#endif
		if (NGUIWidgets.Length > 0)
			alpha = NGUIWidgets[0].color.a;
		if (animatedColorComponents.Length > 0)
			alpha = animatedColorComponents[0].color.a;
	}

	public void UpdateAlpha(float value)
	{
		foreach(SpriteRenderer r in spriteRenderers)
			r.color = new Color(r.color.r, r.color.g, r.color.b, value);
		foreach(MeshRenderer r2 in meshRenderers)
			r2.material.color = new Color(r2.material.color.r, r2.material.color.g, r2.material.color.b, value);
		
#if TMProFadeEnabled
		foreach(TextMeshPro r3 in textMeshProRenderers)
		{
			if (Mathf.Max(r3.color.r, r3.color.g, r3.color.b) <= 1f)
				r3.color = new Color(r3.color.r, r3.color.g, r3.color.b, value);
			else
				r3.color = new Color(r3.color.r / 255f, r3.color.g / 255f, r3.color.b / 255f, value);

		}
#endif
		foreach(UIWidget w in NGUIWidgets)
			w.color = new Color(w.color.r, w.color.g, w.color.b, value);
		foreach(AnimatedColor ac in animatedColorComponents)
			ac.color = new Color(ac.color.r, ac.color.g, ac.color.b, value);
		if (disableColliders)
		{
			foreach(Collider c in GetComponentsInChildren<Collider>())
				c.enabled = (value != 0f);
		}
		alpha = value;
	}

	public void TweenAlphaFrom(float from, float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", from,
			"to", to,
			"time", time,
			"delay", delay,
			"onupdate", "UpdateAlpha",
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenAlpha(float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", alpha,
			"to", to,
			"time", time,
			"delay", delay,
			"onupdate", "UpdateAlpha",
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenAlphaPingPong(float from, float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", from,
			"to", to,
			"time", time,
			"delay", delay,
			"loopType", iTween.LoopType.pingPong,
			"onupdate", "UpdateAlpha",
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
}

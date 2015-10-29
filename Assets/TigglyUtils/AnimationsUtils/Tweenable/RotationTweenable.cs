using UnityEngine;
using System.Collections;

public class RotationTweenable : Tweenable {
	
	public bool ignoreTimeScale = false;

	public void SetXRotation(float to)
	{
		Vector3 rot = transform.rotation.eulerAngles;
		rot.x = to;
		transform.rotation = Quaternion.Euler(rot);
	}
	
	public void SetYRotation(float to)
	{
		Vector3 rot = transform.rotation.eulerAngles;
		rot.y = to;
		transform.rotation = Quaternion.Euler(rot);
	}
	
	public void SetZRotation(float to)
	{
		Vector3 rot = transform.rotation.eulerAngles;
		rot.z = to;
		transform.rotation = Quaternion.Euler(rot);
	}

	public void TweenRotationFrom(Vector3 from, Vector3 to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo, bool isLocal = false)
	{
		transform.rotation = Quaternion.Euler(from);
		iTween.RotateTo(gameObject, iTween.Hash(
			"rotation", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenRotation(Vector3 to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo, bool isLocal = false)
	{
		iTween.RotateTo(gameObject, iTween.Hash(
			"rotation", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"islocal", isLocal,
			"easetype", easeType));
	}

	public void TweenXRotationFrom(float from, float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo, bool isLocal = false)
	{
		transform.rotation = Quaternion.Euler(from, transform.position.y, transform.position.z);
		iTween.RotateTo(gameObject, iTween.Hash(
			"x", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"islocal", isLocal,
			"easetype", easeType));
	}
	
	public void TweenXRotation(float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo, bool isLocal = false)
	{
		iTween.RotateTo(gameObject, iTween.Hash(
			"x", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"islocal", isLocal,
			"easetype", easeType));
	}

	public void TweenYRotationFrom(float from, float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo, bool isLocal = false)
	{
		transform.rotation = Quaternion.Euler(transform.position.x, from, transform.position.z);
		iTween.RotateTo(gameObject, iTween.Hash(
			"y", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"islocal", isLocal,
			"easetype", easeType));
	}
	
	public void TweenYRotation(float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo, bool isLocal = false)
	{
		iTween.RotateTo(gameObject, iTween.Hash(
			"y", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"islocal", isLocal,
			"easetype", easeType));
	}
	
	public void TweenZRotationFrom(float from, float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo, bool isLocal = false)
	{
		transform.rotation = Quaternion.Euler(transform.position.x, transform.position.y, from);
		iTween.RotateTo(gameObject, iTween.Hash(
			"z", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"islocal", isLocal,
			"easetype", easeType));
	}
	
	public void TweenZRotation(float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo, bool isLocal = false)
	{
		iTween.RotateTo(gameObject, iTween.Hash(
			"z", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"islocal", isLocal,
			"easetype", easeType));
	}
}

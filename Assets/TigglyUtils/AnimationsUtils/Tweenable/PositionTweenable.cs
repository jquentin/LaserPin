using UnityEngine;
using System.Collections;

public class PositionTweenable : Tweenable {
	
	public bool ignoreTimeScale = false;

	public void SetXPosition(float to)
	{
		Vector3 pos = transform.position;
		pos.x = to;
		transform.position = pos;
	}
	
	public void SetYPosition(float to)
	{
		Vector3 pos = transform.position;
		pos.y = to;
		transform.position = pos;
	}

	public void TweenPositionFrom(Vector3 from, Vector3 to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo, bool isLocal = false)
	{
		transform.position = from;
		iTween.MoveTo(gameObject, iTween.Hash(
			"position", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"easetype", easeType,
			"name", TweenName,
			"islocal", isLocal));
	}
	
	public void TweenPosition(Vector3 to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo, bool isLocal = false)
	{
		iTween.MoveTo(gameObject, iTween.Hash(
			"position", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"easetype", easeType,
			"name", TweenName,
			"islocal", isLocal));
	}

	public void TweenXYPositionFrom(Vector2 from, Vector2 to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		StopTween();
		transform.position = new Vector3(from.x, from.y, transform.position.z);
		iTween.MoveTo(gameObject, iTween.Hash(
			"position", new Vector3(to.x, to.y, transform.position.z),
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenXYPosition(Vector2 to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo, iTween.LoopType loopType = iTween.LoopType.none, bool isLocal = false)
	{
		StopTween();
		iTween.MoveTo(gameObject, iTween.Hash(
			"position", new Vector3(to.x, to.y, transform.position.z),
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType,
			"looptype",loopType,"islocal", isLocal));
	}

	public void TweenXPositionFrom(float from, float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo, bool isLocal = false)
	{
		transform.position = new Vector3(from, transform.position.y, transform.position.z);
		iTween.MoveTo(gameObject, iTween.Hash(
			"x", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"easetype", easeType,
			"name", TweenName,
			"islocal", isLocal));
	}
	
	public void TweenXPosition(float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo, bool isLocal = false)
	{
		iTween.MoveTo(gameObject, iTween.Hash(
			"x", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"easetype", easeType,
			"name", TweenName,
			"islocal", isLocal));
	}

	public void TweenYPositionFrom(float from, float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo, bool isLocal = false)
	{
		transform.position = new Vector3(transform.position.x, from, transform.position.z);
		iTween.MoveTo(gameObject, iTween.Hash(
			"y", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"easetype", easeType,
			"name", TweenName,
			"islocal", isLocal));
	}
	
	public void TweenYPosition(float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo, bool isLocal = false)
	{
		iTween.MoveTo(gameObject, iTween.Hash(
			"y", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"easetype", easeType,
			"name", TweenName,
			"islocal", isLocal));
	}

	#region Using transforms
	
	public void TweenPositionFrom(Transform from, Transform to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		StopTween();
		transform.position = from.position;
		iTween.MoveTo(gameObject, iTween.Hash(
			"position", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenPosition(Transform to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		StopTween();
		iTween.MoveTo(gameObject, iTween.Hash(
			"position", to,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenXYPositionFrom(Transform from, Transform to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		StopTween();
		transform.position = new Vector3(from.position.x, from.position.y, transform.position.z);
		iTween.MoveTo(gameObject, iTween.Hash(
			"position", new Vector3(to.position.x, to.position.y, transform.position.z),
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenXYPosition(Transform to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		StopTween();
		iTween.MoveTo(gameObject, iTween.Hash(
			"position", new Vector3(to.position.x, to.position.y, transform.position.z),
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenXPositionFrom(Transform from, Transform to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		StopTween();
		transform.position = new Vector3(from.position.x, transform.position.y, transform.position.z);
		iTween.MoveTo(gameObject, iTween.Hash(
			"x", to.position.x,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenXPosition(Transform to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		StopTween();
		iTween.MoveTo(gameObject, iTween.Hash(
			"x", to.position.x,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenYPositionFrom(Transform from, Transform to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		StopTween();
		transform.position = new Vector3(transform.position.x, from.position.y, transform.position.z);
		iTween.MoveTo(gameObject, iTween.Hash(
			"y", to.position.y,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenYPosition(Transform to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		StopTween();
		iTween.MoveTo(gameObject, iTween.Hash(
			"y", to.position.y,
			"time", time,
			"delay", delay,
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
	#endregion
}

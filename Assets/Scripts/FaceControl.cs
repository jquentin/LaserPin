using UnityEngine;
using System.Collections;

public class FaceControl : MonoBehaviour {

	public enum Face { Smile, Neutral, Sad, Worried}
	private Face _face;
	public Face face
	{
		get
		{
			return _face;
		}
		set
		{
			SetFace(value);
		}
	}
	public void SetFace(Face value, bool force = false)
	{
		if (smile == null || neutral == null || sad == null)
			return;
		if (value != _face || force)
		{
			smile.SetActive(value == Face.Smile);
			neutral.SetActive(value == Face.Neutral);
			sad.SetActive(value == Face.Sad);
			if(worried){worried.SetActive(value == Face.Worried);}
			_face = value;
			if (OnMoodChanged != null)
				OnMoodChanged(_face);
		}
	}

	public delegate void OnMoodChangedHandler(Face mood);
	public OnMoodChangedHandler OnMoodChanged;

	public GameObject smile;
	public GameObject neutral;
	public GameObject sad;
	public GameObject worried;
	public float smileFreq = 1f;
	public float neutralFreq = 0.2f;
	public float sadFreq = 0.05f;
	public float changeFreq = 1f;
	public GameObject[] eyeLids;
	public Transform eyeBrows;
	private float initBrowsHeight;
	public float frequencyBlink = 4f;
	public float randomFreqBlink = 1f;
	public float timeBlink = 0.1f;
	public bool blinking = true;
	public bool changeFaces = true;
	public bool centerEyes { get; private set; }
	public bool blinkOnTap = true;
	public Transform eyesTarget;

	public delegate void OnMouthChangeHandler();
	public OnMouthChangeHandler OnMouthOpen;
	public OnMouthChangeHandler OnMouthClose;
	public OnMouthChangeHandler OnMouthKiss;
	public OnMouthChangeHandler OnMouthNoKiss;


	public void StartTalk(float time)
	{
		StartCoroutine("Talk", time);
	}

	public void StopTalk()
	{
		StopCoroutine("Talk");
	}

	public IEnumerator Talk(float time)
	{
		float timeOpen = 0.2f;
		float timeClose = 0.2f;
		float totalTime = 0f;
		while(totalTime < time)
		{
			OpenMouth();
			yield return new WaitForSeconds(timeOpen);
			CloseMouth();
			yield return new WaitForSeconds(timeClose);
			totalTime += timeOpen + timeClose;
		}
	}

	public IEnumerator Kiss(float time)
	{
		float timeOpen = 0.3f;
		float timeClose = 0.3f;
		float totalTime = 0f;
		SetEyesOpen(false);
		while(totalTime < time)
		{
			KissMouth();
			yield return new WaitForSeconds(timeOpen);
			NoKissMouth();
			yield return new WaitForSeconds(timeClose);
			totalTime += timeOpen + timeClose;
		}
		SetEyesOpen(true);
	}

	public void CloseMouth()
	{
		if (OnMouthClose != null) OnMouthClose();
	}

	public void OpenMouth()
	{
		if (OnMouthOpen != null) OnMouthOpen();
	}

	public void KissMouth()
	{
		if (OnMouthKiss != null) OnMouthKiss();
	}
	public void NoKissMouth()
	{
		if (OnMouthNoKiss != null) OnMouthNoKiss();
	}

	void Start()
	{
		if (eyeBrows != null)
			initBrowsHeight = eyeBrows.localPosition.y;
		centerEyes = false;
		SetFace(Face.Smile, true);
		foreach(GameObject go in eyeLids)
			if (go != null)
				go.SetActive(false);
	}

	void OnEnable()
	{
		StartCoroutine("WaitAndBlink");
		StartCoroutine("Eyes");
	}

	void OnDisable()
	{
		StopCoroutine("WaitAndBlink");
		StopCoroutine("Eyes");
		SetEyesOpen(true);
	}

	IEnumerator OnMouseDown ()
	{
		//		TTDManager.instance.TapObjectInScene();
		if (blinkOnTap)
		{
			GetComponent<Collider>().enabled = false;
			int nbBlinks = 4;
			StartCoroutine(BlinkSeveral(nbBlinks, timeBlink));
			yield return new WaitForSeconds(nbBlinks * timeBlink);
			GetComponent<Collider>().enabled = true;
		}
	}

	public void Blink(float time)
	{
		StartCoroutine(BlinkOnce(time));
	}

	void Laugh()
	{
		StartCoroutine(BlinkOnce(0.4f));
	}

	void GetNervous()
	{
		StartCoroutine(BlinkOnce(0.75f));
	}

	IEnumerator Eyes()
	{
		centerEyes = false;
		while(true)
		{
			yield return new WaitForSeconds(Random.Range(2.5f, 5f));
			centerEyes = true;
			yield return new WaitForSeconds(Random.Range(0.8f, 1.2f));
			centerEyes = false;
		}
	}

	IEnumerator ChangeFaces()
	{
		while(changeFaces)
		{
			yield return new WaitForSeconds(changeFreq);
			float r = Random.Range(0f, smileFreq+neutralFreq+sadFreq);
			if (r <= smileFreq)
				face = Face.Smile;
			else if (r <= smileFreq + neutralFreq)
				face = Face.Neutral;
			else
				face = Face.Sad;
		}
	}
	IEnumerator BlinkOnce(float time)
	{
		SetEyesOpen(false);
		yield return new WaitForSeconds(time);
		SetEyesOpen(true);
	}

	void GetWeird()
	{
		StartCoroutine(BlinkOnce(0.5f));
	}

	IEnumerator BlinkSeveral(int times, float time)
	{
		for (int i = 0 ; i < times ; i++)
		{
			SetEyesOpen(false);
			yield return new WaitForSeconds(time);
			SetEyesOpen(true);
			yield return new WaitForSeconds(time);
		}
	}

	IEnumerator WaitAndBlink()
	{
		while(true)
		{
			float delayNextBlink = frequencyBlink + Random.Range(-randomFreqBlink/2, randomFreqBlink/2);
			yield return new WaitForSeconds(delayNextBlink);
			if (blinking)
				SetEyesOpen(false);
			yield return new WaitForSeconds(timeBlink);
			if (blinking)
				SetEyesOpen(true);
		}
	}

	public void SetBrowsUp(bool up)
	{
		print("SetBrowsUp " + up);
		if (eyeBrows != null)
		{
			iTween.Stop (eyeBrows.gameObject);
			iTween.MoveTo(eyeBrows.gameObject, iTween.Hash(
				"y", up ? initBrowsHeight + 0.02f : initBrowsHeight, 
				"time", 0.2f, 
				"islocal", true));
		}
		if (up)
		{
			StopCoroutine("Eyes");
			centerEyes = true;
		}
		else
		{
			StartCoroutine("Eyes");
		}
	}

	public void SetEyesOpen(bool open)
	{
		foreach(GameObject go in eyeLids)
			if (go != null)
				go.SetActive(!open);
		if (eyeBrows != null)
		{
			iTween.Stop (eyeBrows.gameObject);
			iTween.MoveTo(eyeBrows.gameObject, iTween.Hash(
				"y", open ? initBrowsHeight : initBrowsHeight - 0.03f, 
				"time", 0.2f, 
				"islocal", true));
		}
	}

}

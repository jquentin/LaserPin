using UnityEngine;
using System.Collections;

public class FollowRandomTouch : MonoBehaviour {

	public bool isFollowingTouch = false;
	public int fingerindex;

	void Start () 
	{
		EasyTouch.On_TouchStart += EasyTouch_On_TouchStart;
		EasyTouch.On_TouchUp += EasyTouch_On_TouchUp;
		EasyTouch.On_TouchDown += EasyTouch_On_TouchDown;
		gameObject.SetActive(false);
	}

	void OnDestroy () 
	{
		EasyTouch.On_TouchStart -= EasyTouch_On_TouchStart;
		EasyTouch.On_TouchUp -= EasyTouch_On_TouchUp;
		EasyTouch.On_TouchDown -= EasyTouch_On_TouchDown;
	}

	void EasyTouch_On_TouchStart (Gesture gesture)
	{
		if (isFollowingTouch)
			return;
		if (Random.Range(0f, 1f) > 0.5f)
		{
			isFollowingTouch = true;
			fingerindex = gesture.fingerIndex;
			gameObject.SetActive(true);
		}
	}

	void EasyTouch_On_TouchDown (Gesture gesture)
	{
		if (isFollowingTouch && gesture.fingerIndex == fingerindex)
		{
			Vector3 pos = Camera.main.ScreenToWorldPoint(gesture.position);
			pos.z = transform.position.z;
			transform.position = pos;
		}
	}

	void EasyTouch_On_TouchUp (Gesture gesture)
	{
		isFollowingTouch = false;
		gameObject.SetActive(false);
	}
}

using UnityEngine;
using System.Collections;

public class OrientedToDirection : MonoBehaviour {

	public float speed = 8f;
	public bool adaptSpeed = false;
	private Vector3 _lastPos;
	private Quaternion _origRot;
	
	Transform mTrans;
	
	void Awake ()
	{
		mTrans = transform;
		_origRot = mTrans.localRotation;
	}

	void OnEnable()
	{
		_lastPos = mTrans.position;
	}
	
	void OnDisable()
	{
		mTrans.localRotation = _origRot;
    }
	
	void LateUpdate ()
	{
		Vector3 dir = mTrans.position - _lastPos;
		float mag = dir.magnitude;

		Quaternion lookRot;
		if (mag > 0.0001f)
		{
			lookRot = Quaternion.LookRotation(dir);
		}
		else
		{
			lookRot = Quaternion.LookRotation(Vector3.right);
		}
		mTrans.rotation = Quaternion.Slerp(mTrans.rotation, lookRot, Mathf.Clamp01(speed * Time.deltaTime * (adaptSpeed ? mag : 1f)));
		_lastPos = mTrans.position;
	}
}

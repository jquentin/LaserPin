using UnityEngine;
using System.Collections;

public class Eye : MonoBehaviour {

	public Transform target 
	{ 
		get 
		{ 
			if (faceControl != null)
				return faceControl.eyesTarget;
			else
				return null;
		}
	}
	public float radiusEye = 1f;
	public Transform center;
	private Vector3 centerPosition { get { return center.position; } }
	public FaceControl faceControl;

	//when increasing this value the eyes will look more askew in x dir while not looking at a target
	//to make tiggly etc look more goofy, see: https://www.pivotaltracker.com/s/projects/1100572/stories/75192284
	public float askewAmount = 0;

	private Vector3 askewedPosition;
	private float targetYPos;

	//sets whether the eyes are more upwards or downwards when not looking at a target
	enum AskewYDir{
		Up,
		Down
	}
	AskewYDir currentAskewYDir = AskewYDir.Up;

	[ContextMenu("Initialize Eye")]
	void InitializeEye()
	{
		if (faceControl == null)
			faceControl = transform.FindInParents<FaceControl>();
		if (faceControl == null)
			Debug.LogWarning("Eye without a FaceControl");
		if (center == null)
		{
			center = new GameObject(name + "_center").transform;
			center.position = transform.position;
			center.parent = transform.parent;
		}
	}

	void OnDrawGizmosSelected () 
	{
		Gizmos.color = new Color(1f, 0f, 0f, 0.4f);
		Gizmos.DrawSphere(transform.position,radiusEye);
	}

	void Update()
	{
		if (faceControl == null)
			return;
		if (target == null || faceControl.centerEyes || !target.gameObject.activeSelf)
		{
			if(askewAmount==0){
				//default non askew look
				transform.position = Vector3.Lerp(transform.position, centerPosition, 20f * Time.deltaTime);
			}else{

				//goofy look
				targetYPos = centerPosition.y;
				if(currentAskewYDir == AskewYDir.Up){
					targetYPos+=Mathf.Abs(askewAmount*0.5F);
				}else{
					targetYPos-=Mathf.Abs(askewAmount*0.5F);
				}  
				askewedPosition = new Vector3 (centerPosition.x+askewAmount,targetYPos,centerPosition.z);
				transform.position = Vector3.Lerp(transform.position, askewedPosition, 20f * Time.deltaTime);
			}

		}
		else
		{ 

			//prep for next goofy look
			if(askewAmount!=0){
				if(currentAskewYDir == AskewYDir.Up){
					currentAskewYDir = AskewYDir.Down;
				}else{
					currentAskewYDir = AskewYDir.Up;
				}
			}

			Vector3 v = target.position - centerPosition;
			v = Vector3.ClampMagnitude(v, radiusEye);
			transform.position = Vector3.Lerp(transform.position, centerPosition + v, 20f * Time.deltaTime);
		}
	}
}

using UnityEngine;
using System.Collections;

public class TutorialHand : MonoBehaviour {

	public GameObject touchTexture;

	bool isActive { get { return gameObject.activeSelf; } }
	bool isTouching { get { return touchTexture.activeSelf; } }
	Gesture currentGesture;

	void Start()
	{
		Hide();
	}

	public void Show () 
	{
		gameObject.SetActive(true);
	}

	public void Hide () 
	{
		iTween.Stop(gameObject);
		touchTexture.SetActive(false);
		gameObject.SetActive(false);
	}

	public void ActivateTouch()
	{
		touchTexture.SetActive(true);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.forward);
		if(hit.collider != null)
		{
			currentGesture = new Gesture();
			currentGesture.startPosition = Camera.main.WorldToScreenPoint(transform.position);
			currentGesture.position = Camera.main.WorldToScreenPoint(transform.position);
			currentGesture.pickedObject = hit.collider.gameObject;
			hit.collider.gameObject.SendMessage("OnETMouseDown", currentGesture, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void UnactivateTouch()
	{
		if (isTouching)
		{
			touchTexture.SetActive(false);
		}
		if (currentGesture != null && currentGesture.pickedObject != null)
		{
			currentGesture.position = Camera.main.WorldToScreenPoint(transform.position);
			currentGesture.pickedObject.SendMessage("OnETMouseUp", currentGesture, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void MoveTo(Vector3 position)
	{
		transform.position = position;
	}

	public void MoveTo(Vector3 position, float time)
	{
		iTween.MoveTo(gameObject, iTween.Hash(
			"position", position,
			"time", time,
			"easetype", iTween.EaseType.linear));
	}

	void Update()
	{
		if (isActive && isTouching && currentGesture != null && currentGesture.pickedObject != null)
		{
			currentGesture.position = Camera.main.WorldToScreenPoint(transform.position);
			currentGesture.pickedObject.SendMessage("OnETMouseDrag", currentGesture, SendMessageOptions.DontRequireReceiver);
		}
	}
}

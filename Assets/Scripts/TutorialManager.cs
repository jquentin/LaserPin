using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour {

	#region Singleton
	private static TutorialManager _instance;
	public static TutorialManager instance
	{
		get
		{
			if (_instance == null)
				_instance = FindObjectOfType<TutorialManager>();
			return _instance;
		}
	}
	#endregion

	public UILabel tutorialText;
	public TutorialHand tutorialHand1;
	public TutorialHand tutorialHand2;
	public GameObject tutorialUI;
	public UIEventListener previousStepButton;
	public UIEventListener nextStepButton;
	public UIEventListener endTutorialButton;

	int currentStep = 0;

	delegate IEnumerator TutorialStep();
	List<TutorialStep> tutorialSteps = new List<TutorialStep>();
	List<GameObject> createdObjects = new List<GameObject>();

	void Awake()
	{
		tutorialSteps.Add(TutorialStep1_CR);
		tutorialSteps.Add(TutorialStep2_CR);
		tutorialSteps.Add(TutorialStep3_CR);
		previousStepButton.onClick += delegate { PreviousStep(); };
		nextStepButton.onClick += delegate { NextStep(); };
		endTutorialButton.onClick += delegate { EndTutorial(); };
	}

	void Start()
	{
		tutorialUI.SetActive(false);
	}

	public void ShowTutorial()
	{
		GameController.instance.teams[0].isUsed = true;
		GameController.instance.teams[1].isUsed = true;
		tutorialUI.SetActive(true);
		currentStep = 0;
		UpdateButtons();
		StartCurrentStep();
	}

	public void EndTutorial()
	{
		StopCurrentStep();
		Reset();
		tutorialUI.SetActive(false);
		TeamSelection.instance.Show();
	}

	public void NextStep()
	{
		if (currentStep < tutorialSteps.Count - 1)
		{
			StopCurrentStep();
			currentStep++;
			UpdateButtons();
			Reset();
			StartCurrentStep();
		}
	}
	public void PreviousStep()
	{
		if (currentStep > 0)
		{
			StopCurrentStep();
			currentStep--;
			UpdateButtons();
			Reset();
			StartCurrentStep();
		}
	}

	public void ReplayStep()
	{
		Reset();
		StartCurrentStep();
	}

	void UpdateButtons()
	{
		previousStepButton.gameObject.SetActive(currentStep > 0);
		nextStepButton.gameObject.SetActive(currentStep < tutorialSteps.Count - 1);
	}

	void StartCurrentStep()
	{
		print("StartCurrentStep: " + currentStep);
		StartCoroutine(tutorialSteps[currentStep].Method.Name);
	}
	void StopCurrentStep()
	{
		print("StopCurrentStep: " + currentStep);
		StopCoroutine(tutorialSteps[currentStep].Method.Name);
	}

	void Reset()
	{
		tutorialText.text = "";
		tutorialHand1.Hide();
		tutorialHand2.Hide();
		foreach(GameObject go in createdObjects)
			if (go != null)
				Destroy(go);
		createdObjects.Clear();
	}

	IEnumerator TutorialStep1_CR()
	{
		print("TutorialStep1_CR");
		tutorialText.text = "Link the balls to score points";
		Vector3 pos1 = Camera.main.ViewportToWorldPoint(new Vector3(1f / 3f, 0.5f, 10f));
		Vector3 pos2 = Camera.main.ViewportToWorldPoint(new Vector3(2f / 3f, 0.5f, 10f));
		createdObjects.Add(GameController.instance.CreateNode(GameController.instance.teams[0], pos1).gameObject);
		yield return new WaitForSeconds(0.1f);
		createdObjects.Add(GameController.instance.CreateNode(GameController.instance.teams[0], pos2).gameObject);
		yield return new WaitForSeconds(0.5f);
		tutorialHand1.Show();
		tutorialHand1.MoveTo(pos1 + Vector3.back);
		yield return new WaitForSeconds(0.5f);
		tutorialHand1.ActivateTouch();
		yield return new WaitForSeconds(0.3f);
		tutorialHand1.MoveTo(pos2 + Vector3.back, 0.6f);
		yield return new WaitForSeconds(1.5f);
		tutorialHand1.UnactivateTouch();
		yield return new WaitForSeconds(0.5f);
		tutorialHand1.Hide();
		yield return new WaitForSeconds(1.5f);
		ReplayStep();
	}

	IEnumerator TutorialStep2_CR()
	{
		print("TutorialStep2_CR");
		tutorialText.text = "Larger paths make bigger scores";
		Vector3 pos1 = Camera.main.ViewportToWorldPoint(new Vector3(1f / 6f, 0.5f, 10f));
		Vector3 pos2 = Camera.main.ViewportToWorldPoint(new Vector3(2f / 6f, 0.5f, 10f));
		Vector3 pos3 = Camera.main.ViewportToWorldPoint(new Vector3(3f / 6f, 0.5f, 10f));
		Vector3 pos4 = Camera.main.ViewportToWorldPoint(new Vector3(4f / 6f, 0.5f, 10f));
		Vector3 pos5 = Camera.main.ViewportToWorldPoint(new Vector3(5f / 6f, 0.5f, 10f));
		createdObjects.Add(GameController.instance.CreateNode(GameController.instance.teams[0], pos1).gameObject);
		yield return new WaitForSeconds(0.1f);
		createdObjects.Add(GameController.instance.CreateNode(GameController.instance.teams[0], pos2).gameObject);
		yield return new WaitForSeconds(0.1f);
		createdObjects.Add(GameController.instance.CreateNode(GameController.instance.teams[0], pos3).gameObject);
		yield return new WaitForSeconds(0.1f);
		createdObjects.Add(GameController.instance.CreateNode(GameController.instance.teams[0], pos4).gameObject);
		yield return new WaitForSeconds(0.1f);
		createdObjects.Add(GameController.instance.CreateNode(GameController.instance.teams[0], pos5).gameObject);
		yield return new WaitForSeconds(0.5f);
		tutorialHand1.Show();
		tutorialHand1.MoveTo(pos1 + Vector3.back);
		yield return new WaitForSeconds(0.5f);
		tutorialHand1.ActivateTouch();
		yield return new WaitForSeconds(0.1f);
		tutorialHand1.MoveTo(pos5 + Vector3.back, 1.3f);
		yield return new WaitForSeconds(2f);
		tutorialHand1.UnactivateTouch();
		yield return new WaitForSeconds(0.5f);
		tutorialHand1.Hide();
		yield return new WaitForSeconds(1.5f);
		ReplayStep();
	}

	IEnumerator TutorialStep3_CR()
	{
		print("TutorialStep3_CR");
		tutorialText.text = "Cross opponents' paths to steal their score";
		Vector3 pos1 = Camera.main.ViewportToWorldPoint(new Vector3(1f / 5f, 0.5f, 10f));
		Vector3 pos2 = Camera.main.ViewportToWorldPoint(new Vector3(2f / 5f, 0.5f, 10f));
		Vector3 pos3 = Camera.main.ViewportToWorldPoint(new Vector3(3f / 5f, 0.5f, 10f));
		Vector3 pos4 = Camera.main.ViewportToWorldPoint(new Vector3(4f / 5f, 0.5f, 10f));
		Vector3 posB1 = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f / 3f, 10f));
		Vector3 posB2 = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 2f / 3f, 10f));
		createdObjects.Add(GameController.instance.CreateNode(GameController.instance.teams[0], pos1).gameObject);
		yield return new WaitForSeconds(0.1f);
		createdObjects.Add(GameController.instance.CreateNode(GameController.instance.teams[0], pos2).gameObject);
		yield return new WaitForSeconds(0.1f);
		createdObjects.Add(GameController.instance.CreateNode(GameController.instance.teams[0], pos3).gameObject);
		yield return new WaitForSeconds(0.1f);
		createdObjects.Add(GameController.instance.CreateNode(GameController.instance.teams[0], pos4).gameObject);
		yield return new WaitForSeconds(0.1f);
		createdObjects.Add(GameController.instance.CreateNode(GameController.instance.teams[1], posB1).gameObject);
		yield return new WaitForSeconds(0.1f);
		createdObjects.Add(GameController.instance.CreateNode(GameController.instance.teams[1], posB2).gameObject);
		yield return new WaitForSeconds(0.5f);
		tutorialHand1.Show();
		tutorialHand1.MoveTo(pos1 + Vector3.back);
		yield return new WaitForSeconds(0.5f);
		tutorialHand1.ActivateTouch();
		yield return new WaitForSeconds(0.1f);
		tutorialHand1.MoveTo(pos4 + Vector3.back, 1.3f);
		yield return new WaitForSeconds(2f);
		tutorialHand2.Show();
		tutorialHand2.MoveTo(posB2 + Vector3.back);
		yield return new WaitForSeconds(0.5f);
		tutorialHand2.ActivateTouch();
		yield return new WaitForSeconds(0.1f);
		tutorialHand2.MoveTo(posB1 + Vector3.back, 1.3f);
		yield return new WaitForSeconds(2f);
		tutorialHand1.UnactivateTouch();
		yield return new WaitForSeconds(0.5f);
		tutorialHand2.UnactivateTouch();
		yield return new WaitForSeconds(0.5f);
		tutorialHand1.Hide();
		tutorialHand2.Hide();
		yield return new WaitForSeconds(1.5f);
		ReplayStep();
	}

}

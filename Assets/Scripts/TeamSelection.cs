using UnityEngine;
using System.Collections;

public class TeamSelection : MonoBehaviour {

	#region Singleton
	private static TeamSelection _instance;
	public static TeamSelection instance
	{
		get
		{
			if (_instance == null)
				_instance = FindObjectOfType<TeamSelection>();
			return _instance;
		}
	}
	#endregion

	public TeamButton[] playerButtons;
	
	public UIEventListener playButton;

	void Start()
	{
		for (int i = 0 ; i < playerButtons.Length ; i++)
		{
			playerButtons[i].SetTeam(GameController.instance.teams[i]);
		}
		playButton.onClick += delegate {
			gameObject.SetActive(false);
			GameController.instance.Play();
		};
	}

}

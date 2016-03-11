using UnityEngine;
using System.Collections;

public class TeamSelection : ShowablePanel {

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

	int nbSelectedButtons
	{
		get
		{
			int res = 0;
			foreach(TeamButton tb in playerButtons)
				if (tb.isSelected)
					res++;
			return res;
		}
	}

	void Awake()
	{
		_instance = this;
	}

	void Start()
	{
		for (int i = 0 ; i < playerButtons.Length ; i++)
		{
			playerButtons[i].SetTeam(GameController.instance.teams[i]);
		}
		playButton.onClick += delegate {
			Hide();
			GameController.instance.Play();
		};
	}

	void Update()
	{
		bool isPlayable = (nbSelectedButtons > 1);
		playButton.GetComponentInChildren<Collider>(true).enabled = isPlayable;
		playButton.GetComponentInChildren<UILabel>().color = isPlayable ? Color.white : Color.gray;
	}

}

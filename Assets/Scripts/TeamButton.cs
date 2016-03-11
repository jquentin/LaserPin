using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(UITexture))]
public class TeamButton : MonoBehaviour {

	private Collider _collider;
	public Collider collider
	{
		get
		{
			if (_collider == null)
				_collider = GetComponent<Collider>();
			return _collider;
		}
	}
	
	private UITexture _texture;
	public UITexture texture
	{
		get
		{
			if (_texture == null)
				_texture = GetComponent<UITexture>();
			return _texture;
		}
	}

	public UITexture checkedTexture;

	private Color color
	{
		set
		{
			texture.color = value;
			checkedTexture.color = value;
		}
	}

	private Team team;

	public bool isSelected
	{
		get
		{
			return team.isUsed;
		}
	}

	public void SetTeam(Team team)
	{
		this.team = team;
		color = team.color;
		UpdateCheckState();
	}

	void OnClick()
	{
		team.isUsed = !team.isUsed;
		UpdateCheckState();
	}

	void UpdateCheckState()
	{
		checkedTexture.enabled = team.isUsed;
	}
}

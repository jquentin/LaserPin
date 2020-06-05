using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BonusComponent : MonoBehaviour
{
}

[Serializable]
public class Bonus
{

	public int price;

	public Texture icon;

	public BonusComponent component;

}

public class BonusManager : MonoBehaviour 
{

	#region Singleton
	private static BonusManager _instance;
	public static BonusManager instance
	{
		get
		{
			if (_instance == null)
				_instance = FindObjectOfType<BonusManager>();
			return _instance;
		}
	}
	#endregion

	public List<Bonus> bonuses;

}

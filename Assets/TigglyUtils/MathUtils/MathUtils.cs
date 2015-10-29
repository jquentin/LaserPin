using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class MathUtils 
{
	
	public static float RandomXOnScreen(this Camera cam)
	{
		float z = -cam.transform.position.z;
		float leftScreen = cam.ScreenToWorldPoint(new Vector3(0f, 0f, z)).x;
		float rightScreen = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0f, z)).x;
		float x = Random.Range(leftScreen, rightScreen);
		return x;
	}
	
	public static float RandomXOnScreen()
	{
		return Camera.main.RandomXOnScreen();
	}

	public static float randomSign
	{
		get
		{
			return Random.Range(0f, 1f) > 0.5f ? -1f : 1f;
		}
	}
	
	public static List<int> RandomDifferentValues(int nbValues, int min, int max)
	{
		if (nbValues > max - min)
		{
			Debug.LogError("Calling RandomList with an interval smaller than the number of different elements expected");
			return null;
		}
		List<int> nbs = new List<int>();
		for (int i = 0 ; i < nbValues ; i++)
		{
			bool found = false;
			int nb = -1;
			while (!found)
			{
				nb = Random.Range(min, max);
				if (!nbs.Contains(nb))
				{
					found = true;
				}
			}
			nbs.Add(nb);
		}
		return nbs;
	}
	
	public static List<Type> RandomDifferentValuesInList<Type>(int nbValues, List<Type> elements, List<Type> excluding = null)
	{
		if (excluding == null)
			excluding = new List<Type>();
		if (nbValues > elements.Count - excluding.Count)
		{
			Debug.LogError("Calling RandomList with a list smaller than the number of different elements expected");
			return null;
		}
		List<Type> res = new List<Type>();
		for (int i = 0 ; i < nbValues ; i++)
		{
			bool found = false;
			Type elmt = default(Type);
			while (!found)
			{
				elmt = elements[Random.Range(0, elements.Count)];
				if (!res.Contains(elmt) && !excluding.Contains(elmt))
				{
					found = true;
				}
			}
			res.Add(elmt);
		}
		return res;
	}
	
	
	public static Vector2 VectorFromPolar(float angle, float magnitude)
	{
		return new Vector2(magnitude * Mathf.Cos(angle), magnitude * Mathf.Sin(angle));
	}

}

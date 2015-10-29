using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class RandomUtils {
	
	
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
}

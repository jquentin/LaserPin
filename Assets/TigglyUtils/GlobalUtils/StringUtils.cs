using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class StringUtils {
	
	public static int nbOccurences (this string s, char c)
	{
		int res = 0;
		foreach(char ch in s)
			if (ch == c)
				res++;
		return res;
	}
	
	public static List<int> Occurences (this string s, char c)
	{
		List<int> res = new List<int>();
		for(int i = 0 ; i < s.Length ; i++)
		{
			char ch = s[i];
			if (ch == c)
				res.Add(i);
		}
		return res;
	}
	
	public static List<int> IndexesOf (this string s, char c)
	{
		return s.Occurences(c);
	}

}

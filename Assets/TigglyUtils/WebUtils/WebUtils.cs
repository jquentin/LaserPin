using UnityEngine;
using System.Collections;

public static class WebUtils{

	public static IEnumerator CheckInternetReachability(System.Action<bool> callback, float timeout = float.NaN)
	{
		/*
		 *  At first i used in built Application.internetReachability method to check internet rechability, But it was not working properly 
		 *  so i have added this temporary code to check internet connectivity. 
		 **/
		float startTime = Time.realtimeSinceStartup;
		WWW www = new WWW("https://www.google.com");
		while ((float.IsNaN(timeout) || Time.realtimeSinceStartup <= startTime + timeout) && !www.isDone)
			yield return null;
		callback(www.isDone && string.IsNullOrEmpty(www.error));
	}
}

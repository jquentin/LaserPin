using UnityEngine;
using System.Collections;


public static class DeviceUtils 
{
	
	private static float _aspectRatio = float.NaN;
	public static float aspectRatio
	{
		get
		{
			if (float.IsNaN(_aspectRatio))
				_aspectRatio = (float)Screen.width / (float)Screen.height;
			return _aspectRatio;
		}
	}
}

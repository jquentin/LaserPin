using UnityEngine;
using System.Collections;


public static class AudioUtils 
{

	public static float GetLengthIfNotNull(AudioClip clip, float defaultValue = 1f)
	{
		if (clip != null)
			return clip.length;
		else
			return defaultValue;
	}

}

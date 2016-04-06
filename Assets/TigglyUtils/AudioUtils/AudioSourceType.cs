using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AudioType { Sound, Music, VoiceOver, ExtendedStoryTelling, ToBePlayedAnyway}

[RequireComponent(typeof(AudioSource))]
public class AudioSourceType : MonoBehaviour {

	public AudioType audioType = AudioType.Sound;

	void Awake () 
	{
		UpdateSource();
//		UserSettings.OnMusicStateChanged += UpdateSource;
//		UserSettings.OnSoundsStateChanged += UpdateSource;
//		UserSettings.OnExtendedStoryTellingStateChanged += UpdateSource;
//		UserSettings.OnVoiceOverStateChanged += UpdateSource;
	}

	void OnDestroy()
	{
//		UserSettings.OnMusicStateChanged -= UpdateSource;
//		UserSettings.OnSoundsStateChanged -= UpdateSource;
//		UserSettings.OnExtendedStoryTellingStateChanged -= UpdateSource;
//		UserSettings.OnVoiceOverStateChanged -= UpdateSource;
	}

	void UpdateSource()
	{
		bool playable = audioType.IsPlayable();
		Debug.Log(name + " " + playable);
		GetComponent<AudioSource>().enabled = playable;
	}

}

public static class AudioSourceTypeUtils
{

	
	public static AudioType GetAudioType(this AudioSource source)
	{
		AudioSourceType sourceType = source.GetComponent<AudioSourceType>();
		AudioType type;
		if (sourceType == null)
			type = AudioType.Sound;
		else
			type = sourceType.audioType;
		return type;
	}
	
	public static bool IsPlayable(this AudioType type)
	{
		return true;
//		return (type == AudioType.ToBePlayedAnyway
//		        ||type == AudioType.Sound && UserSettings.sfxEnabled
//		        || type == AudioType.Music && UserSettings.musicEnabled
//		        || type == AudioType.ExtendedStoryTelling && UserSettings.extendedStoryTellingVoiceOverEnabled
//		        || type == AudioType.VoiceOver && UserSettings.voiceOverEnabled);
	}
	
	public static void PlayOneShotControlled(this AudioSource source, List<AudioClip> clips)
	{
		if (source == null)
		{
			Debug.LogWarning("PlayOneShotControlled null audio source");
			return;
		}
		source.PlayOneShotControlled(clips, source.GetAudioType());
	}
	
	public static void PlayOneShotControlled(this AudioSource source, List<AudioClip> clips, AudioType type)
	{
		if (source == null)
		{
			Debug.LogWarning("PlayOneShotControlled null audio source");
			return;
		}
		if (!type.IsPlayable())
			return;
		//#if !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2 && !UNITY_4_3 && !UNITY_4_5 && !UNITY_4_6
		//source.spatialBlend = 0f;
		//#endif
		if (clips.Count > 0)
			source.PlayOneShot(clips[Random.Range(0, clips.Count)]);
		else
			Debug.LogWarning("Empty sound list to play");
	}
	
	public static void PlayOneShotControlled(this AudioSource source, AudioClip clip)
	{
		if (source == null)
		{
			Debug.LogWarning("PlayOneShotControlled null audio source");
			return;
		}
		source.PlayOneShotControlled(clip, source.GetAudioType());
	}
	
	
	public static void PlayOneShotControlled(this AudioSource source, AudioClip clip, AudioType type)
	{
		if (source == null)
		{
			Debug.LogWarning("PlayOneShotControlled null audio source");
			return;
		}
		if (!type.IsPlayable())
			return;
//		#if !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2 && !UNITY_4_3 && !UNITY_4_5 && !UNITY_4_6
//		//source.spatialBlend = 0f;
//		#endif
		source.PlayOneShot(clip);
	}

	
	// -------------------------------------------------------------------------------------------------------------------------
	
	

	public static void PlayControlled(this AudioSource source)
	{
		if (source == null)
		{
			Debug.LogWarning("PlayControlled null audio source");
			return;
		}
		source.PlayControlled(source.GetAudioType());
	}

	public static void PlayControlled(this AudioSource source, AudioType type)
	{
		if (source == null)
		{
			Debug.LogWarning("PlayControlled null audio source");
			return;
		}
		if (!type.IsPlayable())
			return;
//		#if !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2 && !UNITY_4_3 && !UNITY_4_5 && !UNITY_4_6
//		source.spatialBlend = 0f;
//		#endif
		source.Play();
	}

}

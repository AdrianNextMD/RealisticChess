using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NaughtyAttributes;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]

public class SoundsManager : MonoBehaviour
{
	private AudioSource audioSource;
	private AudioLowPassFilter lowpass;
	[SerializeField] private AudioClip[] Songs;

	[ShowNonSerializedField] private int currentSongArray = 0;
	[SerializeField] private string currentSongName;
	private float currentSongTime;

	public void Awake()
	{
		if (TryGetComponent(out AudioSource audioSource))
			this.audioSource = audioSource;
		if (TryGetComponent(out AudioLowPassFilter lowpass))
			this.lowpass = lowpass;
	}

	public void ResetToTrack(int array, float time = 0f)
	{
		currentSongArray = array;
		audioSource.clip = Songs[array];
		//audioSource.volume = Music_Volume;
		audioSource.time = time;
		audioSource.Play();

		currentSongName = audioSource.clip.name;
		currentSongTime = audioSource.clip.length;
		lowpass.cutoffFrequency = 22000f;
		//ShowSongName();
	}

	[Button]
	public void NextRandomTrack()
	{
		int randomSong = Random.Range(0, Songs.Length);
		currentSongArray = randomSong;
		audioSource.clip = Songs[currentSongArray];
		audioSource.time = 0;
		//audioSource.volume = Music_Volume;
		audioSource.Play();

		currentSongName = audioSource.clip.name;
		currentSongTime = audioSource.clip.length;
		lowpass.cutoffFrequency = 22000f;
		//ShowSongName();
	}

	[Button]
	public void NextTrack()
	{
		if (currentSongArray == Songs.Length - 1)
		{
			ResetToTrack(0);
		}
		if (currentSongArray < Songs.Length-1)
        {
			currentSongArray++;
			audioSource.clip = Songs[currentSongArray];
			//audioSource.volume = Music_Volume;
			audioSource.time = 0;
			audioSource.Play();
			
			currentSongName = audioSource.clip.name;
			currentSongTime = audioSource.clip.length;
			lowpass.cutoffFrequency = 22000f;
			//ShowSongName();
		}
	}

	[Button]
	public void PreviousTrack()
	{
		if (currentSongArray == 0)
		{
			ResetToTrack(Songs.Length - 1);
		}
		if (currentSongArray > 0)
		{
			currentSongArray--;
			audioSource.clip = Songs[currentSongArray];
			audioSource.time = 0;
			//audioSource.volume = Music_Volume;
			audioSource.Play();
			
			currentSongName = audioSource.clip.name;
			currentSongTime = audioSource.clip.length;
			lowpass.cutoffFrequency = 22000f;
			//ShowSongName();
		}
	}

	public void PlayTrackName(string name)
	{
		for (int i = 0; i < Songs.Length; i++)
		{
			if (Songs[i].name == name)
			{
				currentSongArray = i;
				audioSource.clip = Songs[i];
				audioSource.Play();
				currentSongName = audioSource.clip.name;
				currentSongTime = audioSource.clip.length;
				lowpass.cutoffFrequency = 22000f;
				//ShowSongName();
			}
		}
	}

	private void FixedUpdate()
	{
        //If audio not playing start new track
        if (audioSource)
        {
            if (!audioSource.isPlaying)
            {
                NextRandomTrack();
            }
        }
    }

	//TODO: Show in UI Current playing music
	// public void ShowSongName()
	// {
	// 	if (audioSettings)
	// 	{
	// 		audioSettings.RefreshMusicUI();
	// 		audioSettings.UpdateSongName(currentSongName);
	// 	}
	// 	else
	// 	{
	// 		if (FindObjectOfType<AudioSettingsSystem>())
	// 		{
	// 			SetAudioSettingsRef();
	// 			audioSettings.RefreshMusicUI();
	// 			audioSettings.UpdateSongName(currentSongName);
	// 		}
	// 	}
	// }
}

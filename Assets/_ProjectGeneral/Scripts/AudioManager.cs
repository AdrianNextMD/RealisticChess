using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioManager : Singleton<AudioManager>
{
    public Sound[] Sounds;

	private void Awake()
	{
		foreach (var s in Sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();	
			s.source.clip = s.audioClip;

			s.source.volume = s.volume;
			s.source.loop = s.loop;
			s.source.outputAudioMixerGroup = s.audioMixerGroup;
		}
	}

	public void Play(string name)
	{
		Sound s = Array.Find(Sounds, sound => sound.name == name);
		if (s == null)
			return;
		s.source.Play();
	}
	
	public void PlayOneShot(string name)
	{
		Sound s = Array.Find(Sounds, sound => sound.name == name);
		if (s == null)
			return;
		s.source.PlayOneShot(s.audioClip);
	}
}

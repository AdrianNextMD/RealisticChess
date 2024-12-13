using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsSystem : MonoBehaviour
{
    [Header("Mixer")] [SerializeField] 
    private AudioMixerGroup globalMixerGroup;
    
    [Header("Sounds Menu Manager")]
    [SerializeField] private Slider global_volume_slider;
    [SerializeField] private Slider gameplay_volume_slider;
    [SerializeField] private Slider sfx_volume_slider;
    [SerializeField] private Slider music_volume_slider;

    public float fadeDuration = 5f;

    [BoxGroup("Default Values")]
    [SerializeField] private float global_volume = .6f;
    [BoxGroup("Default Values")]
    [SerializeField] private float gameplay_volume = .6f;
    [BoxGroup("Default Values")]
    [SerializeField] private float sfx_volume = .6f;
    [BoxGroup("Default Values")]
    [SerializeField] private float music_volume = .6f;

    private void Awake()
    {
        LoadVolumeData();
    }

    private void LoadVolumeData()
    {
        StartCoroutine(StartFade(globalMixerGroup.audioMixer, "Music_Volume", fadeDuration,
            ES3.Load("Music_Volume", music_volume)));

        music_volume_slider.value = ES3.Load<float>("Music_Volume", music_volume);

        global_volume_slider.value = ES3.Load<float>("Global_Volume", global_volume);
        globalMixerGroup.audioMixer.SetFloat("Global_Volume", Mathf.Log10(global_volume_slider.value) * 20);

        sfx_volume_slider.value = ES3.Load<float>("SFX_Volume", sfx_volume);
        gameplay_volume_slider.value = ES3.Load<float>("Effects_Volume", gameplay_volume);
        globalMixerGroup.audioMixer.SetFloat("SFX_Volume", sfx_volume);
    }
    
    public void SFXVolume(float value)
    {
        globalMixerGroup.audioMixer.SetFloat("SFX_Volume", Mathf.Log10(value) * 20);
        value = Mathf.Clamp(value, 0.0001f, 1f);
        ES3.Save("SFX_Volume", value);
    }

    public void GlobalVolume(float value)
    {
        globalMixerGroup.audioMixer.SetFloat("Global_Volume", Mathf.Log10(value) * 20);
        value = Mathf.Clamp(value, 0.0001f, 1f);
        ES3.Save("Global_Volume", value);
    }
    
    public void EffectsVolume(float value)
    {
        globalMixerGroup.audioMixer.SetFloat("Effects_Volume", Mathf.Log10(value) * 20);
        value = Mathf.Clamp(value, 0.0001f, 1f);
        ES3.Save("Effects_Volume", value);
    }

    public void MusicVolume(float value)
    {
        globalMixerGroup.audioMixer.SetFloat("Music_Volume", Mathf.Log10(value) * 20);
        value = Mathf.Clamp(value, 0.0001f, 1f);
        ES3.Save("Music_Volume", value);
    }
    
    public IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat(exposedParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }

    public IEnumerator AudioSourceFade(AudioSource source, float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol = 0;
        source.volume = currentVol;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetVolume, currentTime / duration);
            source.volume = newVol;
            yield return null;
        }
        yield break;
    }
}


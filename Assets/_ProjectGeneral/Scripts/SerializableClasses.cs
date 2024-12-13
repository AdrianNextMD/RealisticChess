using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using NaughtyAttributes;

[System.Serializable]
public class Sound
{
	public string name;
	public AudioClip audioClip;
	public AudioMixerGroup audioMixerGroup;
	[Range(0f,1f)]
	public float volume;
	[Range(.1f, 3f)]
	public float pitch;
	public bool loop;

	[HideInInspector]
	public AudioSource source;
}

[System.Serializable]
public class ShopData
{
	public Sprite image;
	public BuyType buyType;
	public int price;
	
	public bool differentMaterials;
	
	public Material whiteMaterial;
	public Material blackMaterial;

	[Space(10)]

	public GameObject[] prefabs;

	public ShopData(GameObject[] prefabs, int price, Sprite image, BuyType buyType)
	{
		this.prefabs = prefabs;
		this.price = price;
		this.image = image;
		this.buyType = buyType;
	}
}

	[System.Serializable]
	public class ItemInfo
	{
		public int index;

		public ItemInfo(int index)
		{
			this.index = index;
		}
	}


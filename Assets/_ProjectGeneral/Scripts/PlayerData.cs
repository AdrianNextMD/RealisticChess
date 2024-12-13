using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerData : MonoBehaviour
{
	public int currentItemBuyed = -1;
	public string itemsBuyed = null;
	public List<ItemInfo> itemInfo = new List<ItemInfo>();

	public void SplitString()
	{
		if (string.IsNullOrEmpty(itemsBuyed)) return;
		string[] splitArray = itemsBuyed.Split(char.Parse("|")); 
		for (int i = 0; i < splitArray.Length; i++)
		{
			int index;
			if (int.TryParse(splitArray[i], out index))
			{
				itemInfo.Add(new ItemInfo(index));
			}
		}
		itemInfo = itemInfo.OrderBy(x => x.index).ToList();
	}

	public void SavePlayer()
	{
		ES3.Save("PlayerData", this);
		ES3CloudManager.Instance.SyncUserData();
	}

	public void LoadSave()
	{
		currentItemBuyed = -1;
		itemsBuyed = null;
		itemInfo.Clear();

		ES3.Load("PlayerData", new PlayerData());
		SplitString();
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerData : MonoBehaviour
{
	[SerializeField] public PlayerData player;

	private void Awake()
	{
		StaticActions.CurrentManager = this;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	public void BuyItem(int index)
	{
		player.itemsBuyed += index + "|";
		ItemInfo newShip = new ItemInfo(index);
		player.itemInfo.Add(newShip);
		int counter = 0;

		while (player.itemInfo[counter].index < newShip.index)
		{
			++counter;
		}

		for (int i = player.itemInfo.Count - 2; i >= counter; i--)
		{
			player.itemInfo[i + 1] = player.itemInfo[i];
		}

		player.itemInfo[counter] = newShip;
		player.currentItemBuyed = counter;
		player.SavePlayer();
	}

	public ItemsShopButtonStates CheckCurrentItemState(int index)
	{
		ItemInfo foundedItem = player.itemInfo.Find(element => element.index == index);

		if (foundedItem == null)
			return ItemsShopButtonStates.Buy;

		if (player.itemInfo[player.currentItemBuyed] == foundedItem)
			return ItemsShopButtonStates.Selected;

		return ItemsShopButtonStates.Select;
	}

	public void ChangeSelecteItem(int indexItem)
	{
		player.currentItemBuyed = player.itemInfo.FindIndex(element => element.index == indexItem);
	}

	public int CurrentSpawnItemIndex()
	{
		return (player.currentItemBuyed >= 0) ? player.itemInfo[player.currentItemBuyed].index : 0;
		
	}
}

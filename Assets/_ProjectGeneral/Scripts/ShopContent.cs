using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopContent : Singleton<ShopContent>
{
	[SerializeField] private ShopItems _shopItems;
	[SerializeField] private Transform spawnChess;
	[SerializeField] private BuyButtonItemsShop _buyButtonItemsShop;

	public List<ShopResources> items = new List<ShopResources>();
	public GameObject InstItem { get; private set; }

	int indexItem;
	int stateIndexItem;
	int priceItem;

	GameObject g;
	ItemsShopButtonStates _itemsShopButtonStates;

	private void OnEnable()
	{
		SelectItem(StaticActions.CurrentManager.CurrentSpawnItemIndex());
	}

	private void Start()
	{
		GameObject buttonTemplate = transform.GetChild(0).gameObject;

		for (int i = 0; i < _shopItems.shopData.Count; i++)
		{
			g = Instantiate(buttonTemplate, transform);
			ShopResources itemResources = g.GetComponent<ShopResources>();
			itemResources.Initialize(_shopItems.shopData[i]);
			items.Add(g.GetComponent<ShopResources>());

			stateIndexItem = i;

			itemResources.indexItem = i;
		}
		Destroy(buttonTemplate);
		
		SelectItem(StaticActions.CurrentManager.CurrentSpawnItemIndex());
		UpdateItemState();

	}

	public void SelectItem(int itemIndex)
	{
		indexItem = itemIndex;
		for (int i = 0; i < items.Count; i++)
		{
			if (i == itemIndex)
			{
				SetUpItem(i);
			}
			else
			{
				items[i].Unselected();
			}
		}
	}

	void UpdateItemState()
	{
		for (int j = 0; j < _shopItems.shopData.Count; j++)
		{
			if (StaticActions.CurrentManager.CheckCurrentItemState(j) == ItemsShopButtonStates.Selected || StaticActions.CurrentManager.CheckCurrentItemState(j) == ItemsShopButtonStates.Select)
			{
				items[j].priceItem.SetActive(false);
			}
		}
	}

	public void BuyItem()
	{
		if (_itemsShopButtonStates == ItemsShopButtonStates.Selected)
		{
			return;
		}

		if (_itemsShopButtonStates == ItemsShopButtonStates.Select)
		{
			StaticActions.CurrentManager.ChangeSelecteItem(indexItem);
			//player.currentShipNameBuyed = shipData.shipList[indexShip].shipName;

			_itemsShopButtonStates = ItemsShopButtonStates.Selected;
			UpdateBuyBtn();
			StaticActions.CurrentManager.player.SavePlayer();
			return;
		}
		if (_shopItems.shopData[indexItem].buyType == BuyType.InGameMoney)
		{
			int money = ChessUIManager.Instance.Money();
			if (money < priceItem)
			{
				UiUtils.Instance.SimplePopup(true, "You don't have enought money!");
				//O sa deschidem popUp-ul cu inap-uri (cand o sal avem)
				return;
			}
			ChessUIManager.Instance.Money(0, priceItem);
			BuyItemCore();
			UpdateBuyBtn();
		}
		else
		{
			//INAPManager.Instance.BuyPieceChess(indexItem);
		}
	}

	private void SetUpItem(int i)
	{
		SpawnManager.Instance.SpawnChessAndBoard(false, i);
		_itemsShopButtonStates = StaticActions.CurrentManager.CheckCurrentItemState(i);

		priceItem = _shopItems.shopData[i].price;
		items[i].SelectedState();
		UpdateBuyBtn();
	}

	private void UpdateBuyBtn()
	{
		switch (_itemsShopButtonStates)
		{
			case ItemsShopButtonStates.Buy:

				_buyButtonItemsShop.SetStateStatus(ItemsShopButtonStates.Buy, priceItem, _shopItems.shopData[indexItem].buyType);
				break;

			case ItemsShopButtonStates.Select:

				_buyButtonItemsShop.SetStateStatus(ItemsShopButtonStates.Select);
				break;

			case ItemsShopButtonStates.Selected:

				_buyButtonItemsShop.SetStateStatus(ItemsShopButtonStates.Selected);
				break;

			default:

				Debug.LogError("WRONG BUTTON SHIP STATE!!!");
				return;
		}
	}


	public void BuyItemCore(int index = 0)
	{
		_itemsShopButtonStates = ItemsShopButtonStates.Selected;

		if (_shopItems.shopData[indexItem].buyType == BuyType.InGameMoney)
		{
			StaticActions.CurrentManager.BuyItem(indexItem);
		}

		if (_shopItems.shopData[indexItem].buyType == BuyType.IAP)
		{
			StaticActions.CurrentManager.BuyItem(index);
			UpdateBuyBtn();
		}

		UpdateItemState();
	}
}

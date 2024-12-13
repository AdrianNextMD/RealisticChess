using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopResources : MonoBehaviour
{
	public enum ChangeSelected
	{
		None,
		Selected
	}
	
	public ChangeSelected changeSelected;
	public GameObject priceItem;
	public GameObject highLight;
	public int indexItem;

	[SerializeField] private Text itemPrice;
	[SerializeField] private GameObject[] buyType;
	[SerializeField] private Image itemImage;

	[HideInInspector] public ShopContent shopContent;
	public SelectedItem selectedItem;

	public void Initialize(ShopData newItem)
	{
		shopContent = GetComponentInParent<ShopContent>();

		itemImage.sprite = newItem.image;
		itemPrice.text = newItem.price.ToString("#,##0");
		if (newItem.buyType == BuyType.InGameMoney)
		{
			itemPrice.text = (newItem.price != 0) ? "$" + newItem.price.ToString("#,##0") : "Free";
		}
		else
		{
			itemPrice.text = "$" + (newItem.price / 100f).ToString();
		}
	}

	public void SelectItem()
	{
		shopContent.SelectItem(indexItem);
	}

	public void SelectedBtn()
	{
		if (changeSelected == ChangeSelected.None)
		{
			selectedItem.ButtonSelected(this);
		}
	}

	public void SelectedState()
	{
		changeSelected = ChangeSelected.Selected;
		highLight.SetActive(true);
	}

	public void Unselected()
	{
		changeSelected = ChangeSelected.None;
		highLight.SetActive(false);
	}
}

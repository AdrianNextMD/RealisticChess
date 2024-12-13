using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyButtonItemsShop : MonoBehaviour
{
    public Text stateText;
    public Text priceText;

    public GameObject[] priceType;

    public void SetStateStatus(ItemsShopButtonStates currentState, int price = 0, BuyType type = BuyType.InGameMoney)
    {
        SetDefaultState();

        switch (currentState)
        {
            case ItemsShopButtonStates.Selected:
            case ItemsShopButtonStates.Select:

                stateText.gameObject.SetActive(true);
                stateText.text = currentState.ToString();
                break;

            case ItemsShopButtonStates.Buy:

                priceText.gameObject.SetActive(true);
                SetPrice(type, price);
                SetBuyStatus(type);
                break;

            default:

                Debug.LogError("Wrong button state");
                return;
        }
    }

    private void SetPrice(BuyType type, int price)
    {
        if (type != BuyType.IAP)
        {
            priceText.text = (price != 0) ? price.ToString() : "Free";
        }
        else
        {
            priceText.text = (price / 100f).ToString();
        }
    }

    private void SetBuyStatus(BuyType type)
    {
        priceType[(int)type].SetActive(true);
    }

    private void SetDefaultState()
    {
        stateText.gameObject.SetActive(false);
        priceText.gameObject.SetActive(false);

        for (int i = 0; i < priceType.Length; i++)
        {
            priceType[i].SetActive(false);
        }
    }
}

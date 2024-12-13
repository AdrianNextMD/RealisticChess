using System;
using System.Collections.Generic;
using GameAnalyticsSDK;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Purchasing;

[Serializable]
public class InApsProducts
{
    public string id;
    public ProductType productType;
}

public class INAPManager : MonoBehaviour, IStoreListener
{
    public static INAPManager Instance;
    public ShopContent shopContent;
  
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;
  
    [SerializeField] private List<InApsProducts> inAppProducts = new List<InApsProducts>();
  
    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
  
        for (int i = 0; i < inAppProducts.Count; i++)
        {
            builder.AddProduct(inAppProducts[i].id, inAppProducts[i].productType);
        }
  
        UnityPurchasing.Initialize(this, builder);
    }
  
    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }
  
    //Step 4 modify purchasing
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, inAppProducts[0].id, StringComparison.Ordinal))
        {
            Debug.Log("Remove ads Succesful");
            ES3.Save("NoAdds", 1);
  
            //GAManager.Instance.SendPushedInAppButton("USD", 299, "Remove ADS", removeAds, "IAP");
        }
        else if (String.Equals(args.purchasedProduct.definition.id, inAppProducts[2].id, StringComparison.Ordinal))
        {
            BuyMoney(10000);
            //GAManager.Instance.SendPushedInAppButton("USD", 249, "Buy coin", coin50000, "IAP")
        }
        else if (String.Equals(args.purchasedProduct.definition.id, inAppProducts[3].id, StringComparison.Ordinal))
        {
            BuyMoney(20000);
            //GAManager.Instance.SendPushedInAppButton("USD", 499, "Buy coin", coin120000, "IAP");
        }
        else if (String.Equals(args.purchasedProduct.definition.id, inAppProducts[4].id, StringComparison.Ordinal))
        {
            //GAManager.Instance.SendPushedInAppButton("USD", 999, "Buy coin", coin280000, "IAP");
            BuyMoney(40000);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, inAppProducts[5].id, StringComparison.Ordinal))
        {
            //GAManager.Instance.SendPushedInAppButton("USD", 1999, "Buy coin", coin600000, "IAP");
            BuyMoney(60000);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, inAppProducts[6].id, StringComparison.Ordinal))
        {
            //GAManager.Instance.SendPushedInAppButton("USD", 4999, "Buy coin", coin1500000, "IAP");
            BuyMoney(100000);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, inAppProducts[1].id, StringComparison.Ordinal))
        {
            //GAManager.Instance.SendPushedInAppButton("USD", 499, "Buy ship", ShipTaifun, "IAP");
            BuyItem(3);
            UiUtils.Instance.ShowLoading(false);
        }
        else
        {
            var errorMess = "Purchase Failed!";
            Debug.Log(errorMess);
            UiUtils.Instance.SimplePopup(true, errorMess);
  
            UiUtils.Instance.ShowLoading(false);
        }
  
        UiUtils.Instance.ShowLoading(false);
        UiUtils.Instance.SimplePopup(true, "Successfully Purchased!");
        return PurchaseProcessingResult.Complete;
    }
  
    public void BuyRemoveAds()
    {
        BuyProductID(inAppProducts[0].id);
    }
  
    public void BuyCoin10000()
    {
        BuyProductID(inAppProducts[2].id);
    }
  
    public void BuyCoin20000()
    {
        BuyProductID(inAppProducts[3].id);
    }
  
    public void BuyCoin40000()
    {
        BuyProductID(inAppProducts[4].id);
    }
  
    public void BuyCoin60000()
    {
        BuyProductID(inAppProducts[5].id);
    }
  
    public void BuyCoin100000()
    {
        BuyProductID(inAppProducts[6].id);
    }
  
    public void BuyPieceChess(int index)
    {
        switch (index)
		{
            case 3:
                BuyProductID(inAppProducts[1].id);
                break;
			default:
                break;
		}
    }
  
    private void BuyMoney(int moneyCount)
    {
        ChessUIManager.Instance.Money(moneyCount);
        ES3CloudManager.Instance.SyncUserData();
        Debug.Log("Buy " + moneyCount + " coin Successful");
    }
  
    private void BuyItem(int index)
    {
        Debug.Log("BUYED NEW Item!!");
        shopContent.BuyItemCore(index);
    }
  
    //**************************** Dont worry about these methods ***********************************
    private void Awake()
    {
        TestSingleton();
    }
  
    void Start()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
  
        //_player = StaticActions.CurrentManager.player;
    }
  
    private void TestSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
  
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
  
    private void BuyProductID(string productId)
    {
        UiUtils.Instance.ShowLoading(true, "Loading...");
  
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log(
                    "BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
  
                UiUtils.Instance.ShowLoading(false);
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
            UiUtils.Instance.ShowLoading(false);
        }
    }
  
    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }
  
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");
            UiUtils.Instance.ShowLoading(true, "Restore Purchases started ...");
  
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) =>
            {
                var errorMess = "RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.";
                UiUtils.Instance.SimplePopup(true, errorMess);
                Debug.Log(errorMess);
  
                UiUtils.Instance.ShowLoading(true);
            });
        }
        else
        {
            var errorMess = "RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform;
            UiUtils.Instance.SimplePopup(true, errorMess);
  
            Debug.Log(errorMess);
            UiUtils.Instance.ShowLoading(false);
        }
  
        Debug.Log("<color=blue>Restore button is pressed</color>");
    }
  
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }
  
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        var errorMess = "OnInitializeFailed InitializationFailureReason:" + error;
        Debug.Log(errorMess);
        UiUtils.Instance.SimplePopup(true, errorMess);
        UiUtils.Instance.ShowLoading(false);
    }
  
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        UiUtils.Instance.ShowLoading(false);
        
        var errorMess = string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",
            product.definition.storeSpecificId, failureReason);
        Debug.Log(errorMess);
        UiUtils.Instance.SimplePopup(true, errorMess);
  
        Debug.Log(errorMess);
   }
}
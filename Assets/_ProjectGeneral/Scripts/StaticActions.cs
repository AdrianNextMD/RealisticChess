using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StaticActions
{
    public static PlayerManagerData CurrentManager;

    public static UnityAction shopPositionCamera;
    public static UnityAction setInitialPositionCamera;

    public static ShopItems shopItems;
    private static string shopItemsInfoPath = "MenuInformation/ShopData";


    [RuntimeInitializeOnLoadMethod]
    private static void DefineData()
	{
        shopItems = Resources.Load<ShopItems>(shopItemsInfoPath);
    }
}

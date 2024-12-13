using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "Menu/ShopData")]
public class ShopItems : ScriptableObject
{
    public List<ShopData> shopData;
}


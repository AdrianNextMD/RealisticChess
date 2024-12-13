using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PiecesCreator : MonoBehaviour
{
    [SerializeField] private Material blackMaterial;
    [SerializeField] private Material whiteMaterial;
    [SerializeField] private ShopItems shopItems;
    public Dictionary<string, GameObject> nameToPieceDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        int index = StaticActions.CurrentManager.player.itemInfo[StaticActions.CurrentManager.player.currentItemBuyed].index;
        foreach (var piece in shopItems.shopData[index].prefabs)
        {
            nameToPieceDict.Add(piece.GetComponent<Piece>().GetType().ToString(), piece); 
        }
    }

    public GameObject CreatePiece(Type type)
    { 
        GameObject prefab = nameToPieceDict[type.ToString()];
        if (prefab)
        {
            GameObject newPiece = Instantiate(prefab, gameObject.transform);
            return newPiece;
        }
        return null;
    }

    public Material GetTeamMaterial(TeamColor team)
    {
        return team == TeamColor.White ? whiteMaterial : blackMaterial;
    }
    
}

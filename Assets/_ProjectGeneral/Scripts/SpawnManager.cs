using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
	[SerializeField] private GameObject _board;
	[SerializeField] private Transform _boardAnchor;
	[SerializeField, Expandable] private ShopItems _shopItems;
	[SerializeField, Expandable] private BoardLayout startingBoardLayout;

	private SinglePlayerBoard board;
	
	Dictionary<string, GameObject> piecesNameDict = new Dictionary<string, GameObject>();
	int indexPiece;

	public void DestroyPieces()
	{
		if(board)
			Destroy(board.gameObject);

		if (transform.childCount > 0)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Destroy(transform.GetChild(i).gameObject);
			}
		}
	}

	public void AddPieceToDict(int index)
	{
		SpawnBoard();
		
		indexPiece = index;
		piecesNameDict.Clear();
		foreach (var piece in _shopItems.shopData[index].prefabs)
		{
			piecesNameDict.Add(piece.GetComponent<Piece>().GetType().ToString(), piece);
		}
		
		GetDataFromLayout(startingBoardLayout);
	}

	public void SpawnBoard()
	{
		board = Instantiate(_board, _boardAnchor).GetComponent<SinglePlayerBoard>();
	}

	private void GetDataFromLayout(BoardLayout layout)
	{
		if (StaticActions.CurrentManager.player.currentItemBuyed != -1)
		{
			indexPiece = StaticActions.CurrentManager.player.currentItemBuyed;
		}
		bool b = StaticActions.shopItems.shopData[indexPiece].differentMaterials;
		
		for (int i = 0; i < layout.GetPiecesCount(); i++)
		{
			Vector2Int squareCoords = layout.GetSquareCoordsAtIndex(i);
			TeamColor team = layout.GetSquareTeamColorAtIndex(i);
			string typeName = layout.GetSquarePieceNameAtIndex(i);

			Type type = Type.GetType(typeName);
			CreatePiece(squareCoords, team, type, indexPiece, b);
		}
	}

	private void CreatePiece(Vector2Int squareCoords, TeamColor team, Type type, int index, bool b)
	{
		Piece newPiece = CreatePiece(type).GetComponent<Piece>();
		newPiece.SetData(squareCoords, team, board, Vector2.zero);

		newPiece.FixRotation();

		Material teamMaterial = newPiece.SetMaterialChess(team, index, b);
		newPiece.SetMaterial(teamMaterial);

		board.SetPieceOnBoard(squareCoords, newPiece);
	}

	public GameObject CreatePiece(Type type)
	{
		GameObject prefab = piecesNameDict[type.ToString()];
		if (prefab)
		{
			GameObject newPiece = Instantiate(prefab, gameObject.transform);
			return newPiece;
		}
		return null;
	}

	public void SpawnChessAndBoard(bool state, int index = 0)
	{
		DestroyPieces();
		if (state) 
		{
			if(StaticActions.CurrentManager.player.currentItemBuyed >= 0)
            {
				AddPieceToDict(StaticActions.CurrentManager.player
				.itemInfo[StaticActions.CurrentManager.player.currentItemBuyed].index);
			}
		}
		else
		{
			AddPieceToDict(index);
		}
	}
}

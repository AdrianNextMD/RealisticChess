using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(SquareSelectorCreator))]
public abstract class Board : Singleton<Board>
{
	public const int BOARD_SIZE = 8;

	[SerializeField] private Transform bottomLeftSquareTransform;
	[SerializeField] private float squareSize;

	public Piece[,] grid;
	private Piece selectedPiece;
	private ChessGameController chessController;
	private SquareSelectorCreator squareSelector;
	
	public Tile[,] _board = new Tile[8, 8];

	protected virtual void Awake()
	{
		squareSelector = GetComponent<SquareSelectorCreator>();
		CreateGrid();
	}
	
	public void SetDependencies(ChessGameController chessController)
	{
		this.chessController = chessController;
	}
	
	private void CreateGrid()
	{
		grid = new Piece[BOARD_SIZE, BOARD_SIZE];
		StartCoroutine(SetupBoard());
	}
	
	private IEnumerator SetupBoard()
	{
		GameObject g = FindObjectOfType<SpawnManager>().gameObject;
		yield return new WaitForSeconds(2f);

		if (g.transform.childCount < 1)
		{
			for (int y = 0; y < 8; y++)
			{
				for (int x = 0; x < 8; x++)
				{
					_board[x, y] = new Tile(x, y);
				}
			}
		}
	}
	
	public Vector3 CalculatePositionFromCoords(Vector2Int coords)
	{
		return bottomLeftSquareTransform.position + new Vector3(coords.x * squareSize, 0f, coords.y * squareSize);
	}
	
	public Vector3 CalculatePositionFromCoordsAI(Vector2Int coords)
	{
		return bottomLeftSquareTransform.position + new Vector3(coords.x * squareSize, 0f, coords.y * squareSize);
	}

	internal void OnSetSelectedPiece(Vector2Int coords)
	{
		Piece piece = GetPieceOnSquare(coords);
		selectedPiece = piece;
	}

	internal void OnSelectedPieceMoved(Vector2Int intCoords)
	{
		Move move = selectedPiece.moves.Find(x => x.secondPosition.Position == intCoords);
		
		Tile firstTile = move.firstPosition;
		Tile secondTile = move.secondPosition;

		secondTile.CurrentPiece = move.pieceMoved;
		firstTile.CurrentPiece = null;
		secondTile.CurrentPiece.position = secondTile.Position;
		secondTile.CurrentPiece.HasMoved = true;
		
		TryToTakeOppositePiece(intCoords);
		UpdateBoardOnPieceMove(intCoords, selectedPiece.occupiedSquare, selectedPiece, null);
		selectedPiece.MovePiece(intCoords);
		DeselectPiece();
		EndTurn();
	}
	
	private void OnSelectedPieceMovedAI(Vector2Int intCoords, Piece piece, Vector2Int oldCoords)
	{
		selectedPiece = piece;
		TryToTakeOppositePiece(intCoords);
		UpdateBoardOnPieceMove(intCoords,  oldCoords, piece, null);
		selectedPiece.MovePieceAI(intCoords);
		//EndTurn();
	}
	
	internal void SwapPieces(Move move)
	{
		Tile firstTile = move.firstPosition;
		Tile secondTile = move.secondPosition;
		
		OnSelectedPieceMovedAI(new Vector2Int(Mathf.RoundToInt(move.secondPosition.Position.x), Mathf.RoundToInt(move.secondPosition.Position.y)), move.pieceMoved, 
			new Vector2Int(Mathf.RoundToInt(move.firstPosition.Position.x), Mathf.RoundToInt(move.firstPosition.Position.y)));
		
		secondTile.CurrentPiece = move.pieceMoved;
		firstTile.CurrentPiece = null;
		secondTile.CurrentPiece.position = secondTile.Position;
		secondTile.CurrentPiece.HasMoved = true;
	}

	private Vector2Int CalculateCoordsFromPosition(Vector3 inputPosition)
	{
		int x = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).x / squareSize) + BOARD_SIZE / 2;
		int y = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).z / squareSize) + BOARD_SIZE / 2;
		return new Vector2Int(x, y);
	}

	public void OnSquareSelected(Vector3 inputPosition)
	{
		if (!chessController) return;
		if (!chessController.CanPerformMove()) return;
		
		Vector2Int coords = CalculateCoordsFromPosition(inputPosition);
		Piece piece = GetPieceOnSquare(coords);
		if (selectedPiece)
		{
			if (piece != null && selectedPiece == piece)
				DeselectPiece();
			else if (piece != null && selectedPiece != piece && chessController.IsTeamTurnActive(piece.team) &&
			         piece.team == TeamColor.White)
			{
				SelectPiece(coords);
			}
			else if (selectedPiece.CanMoveTo(coords))
			{
				SelectedPieceMoved(coords);
			}
		}
		else
		{
			if (piece != null && chessController.IsTeamTurnActive(piece.team))
				SelectPiece(coords);
		}
	}

	public abstract void SelectedPieceMoved(Vector2 coords);
	public abstract void SetSelectedPiece(Vector2 coords);

	private void SelectPiece(Vector2Int coords)
	{
		Piece piece = GetPieceOnSquare(coords);
		piece.GetMoves();
		//chessController.RemoveMovesEnablingAttakOnPieceOfType<King>(piece);
		SetSelectedPiece(coords);    
		List<Vector2Int> selection = selectedPiece.avaliableMoves;
		ShowSelectionSquares(selection);
	}
   
	private void ShowSelectionSquares(List<Vector2Int> selection)
	{
		Dictionary<Vector3, bool> squaresData = new Dictionary<Vector3, bool>();
		for (int i = 0; i < selection.Count; i++)
		{
			Vector3 position = CalculatePositionFromCoords(selection[i]);
			bool isSquareFree = GetPieceOnSquare(selection[i]) == null;
			squaresData.Add(position, isSquareFree);
		}
		squareSelector.ShowSelection(squaresData);
	}

	public void DeselectPiece()
	{
		selectedPiece = null;
		squareSelector.ClearSelection();
	}

	private void EndTurn()
	{
		chessController.EndTurn();
	}
	
	public Tile GetTileFromBoard(Vector2 tile)
	{
		return _board[(int)tile.x, (int)tile.y];
	}

	public void UpdateBoardOnPieceMove(Vector2Int newCoords, Vector2Int oldCoords, Piece newPiece, Piece oldPiece)
	{
		grid[oldCoords.x, oldCoords.y] = oldPiece;
		grid[newCoords.x, newCoords.y] = newPiece;
	}

	public Piece GetPieceOnSquare(Vector2Int coords)
	{
		if (CheckIfCoordinatesAreOnBoard(coords))
			return grid[coords.x, coords.y];
		return null;
	}

	public bool CheckIfCoordinatesAreOnBoard(Vector2Int coords)
	{
		if (coords.x < 0 || coords.y < 0 || coords.x >= BOARD_SIZE || coords.y >= BOARD_SIZE)
			return false;
		return true;
	}

	public bool HasPiece(Piece piece)
	{
		for (int i = 0; i < BOARD_SIZE; i++)
		{
			for (int j = 0; j < BOARD_SIZE; j++)
			{
				if (grid[i, j] == piece)
					return true;
			}
		}
		return false;
	}

	public void SetPieceOnBoard(Vector2Int coords, Piece piece)
	{
		if (CheckIfCoordinatesAreOnBoard(coords))
			grid[coords.x, coords.y] = piece;
	}

	private void TryToTakeOppositePiece(Vector2Int coords)
	{
		Piece piece = GetPieceOnSquare(coords);
		if (piece && !selectedPiece.IsFromSameTeam(piece))
		{
			TakePiece(piece);
		}
	}

	private void TakePiece(Piece piece)
	{
		if (piece)
		{
			grid[piece.occupiedSquare.x, piece.occupiedSquare.y] = null;
			chessController.OnPieceRemoved(piece);
			Destroy(piece.gameObject);
		}
	}

	public void PromotePiece(Piece piece, Vector2 position)
	{
		int index = StaticActions.CurrentManager.player.currentItemBuyed;
		if(index == -1)
			return;
		bool b = StaticActions.shopItems.shopData[index].differentMaterials;
		
		TakePiece(piece);
		chessController.CreatePieceAndInitialize(piece.occupiedSquare, piece.team, typeof(Queen), position, index, b);
	}

	public void OnGameRestarted()
	{
		selectedPiece = null;
		CreateGrid();
	}
}

using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[RequireComponent(typeof(PiecesCreator))]
public abstract class ChessGameController : MonoBehaviour
{
	protected const byte SET_GAME_STATE_EVENT_CODE = 1;

	[SerializeField] private BoardLayout startingBoardLayout;

	AlphaBeta ab = new AlphaBeta();
	private ChessUIManager UIManager;
	private CameraSetup cameraSetup;
	private Board board;
	private PiecesCreator pieceCreator;
	protected ChessPlayer whitePlayer;
	protected ChessPlayer blackPlayer;
	public ChessPlayer activePlayer;

	public Vector2 pos;
	
	public bool isPlayerTurn = false;

	private bool isWhiteTurn = true;

	protected GameState state;

	private void Awake()
	{
		pieceCreator = GetComponent<PiecesCreator>();
	}

	internal void SetDependencies(CameraSetup cameraSetup, ChessUIManager UIManager, Board board)
	{
		this.cameraSetup = cameraSetup;
		this.UIManager = UIManager;
		this.board = board;
	}

	public void InitializeGame()
	{
		CreatePlayers();
	}

	private void CreatePlayers()
	{
		whitePlayer = new ChessPlayer(TeamColor.White, board);
		blackPlayer = new ChessPlayer(TeamColor.Black, board);
	}

	[Button("CheckPosTile")]
	public void CheckPosTile()
	{
		for (int x = 0; x < 8; x++)
			for (int y = 0; y < 8; y++)
			{
				if(x == pos.x && y == pos.y)
					Debug.LogError(board._board[x, y].Position + " " + board._board[x, y].CurrentPiece._type);
			}
		
	}
	
	[Button("CheckBoardTiles")]
	public void CheckBoardTile()
	{
		ab.GetLocalBoard();		
	}

	public void StartNewGame()
	{
		UIManager.OnGameStarted();
		SetGameState(GameState.Init);
		CreatePiecesFromLayout(startingBoardLayout);
		activePlayer = whitePlayer;
		SetupCamera(activePlayer.team);
		GenerateAllPossiblePlayerMoves(activePlayer);
		TryToStartThisGame();
		if(activePlayer == whitePlayer)
			isPlayerTurn = true;
	}

	protected abstract void SetGameState(GameState state);
	public abstract void TryToStartThisGame();
	public abstract bool CanPerformMove();

	internal bool IsGameInProgress()
	{
		return state == GameState.Play;
	}

	private void CreatePiecesFromLayout(BoardLayout layout)
	{
		int index = StaticActions.CurrentManager.player.currentItemBuyed;
		bool b = StaticActions.shopItems.shopData[index].differentMaterials;

		for (int i = 0; i < layout.GetPiecesCount(); i++)
		{
			Vector2Int squareCoords = layout.GetSquareCoordsAtIndex(i);
			Vector2 position = layout.GetPositionPiece(i);
			TeamColor team = layout.GetSquareTeamColorAtIndex(i);
			string typeName = layout.GetSquarePieceNameAtIndex(i);

			Type type = Type.GetType(typeName);
			CreatePieceAndInitialize(squareCoords, team, type, position ,index, b);
		}
	}

	public void CreatePieceAndInitialize(Vector2Int squareCoords, TeamColor team, Type type, Vector2 position, int index, bool b)
	{
		Piece newPiece = pieceCreator.CreatePiece(type).GetComponent<Piece>();
		newPiece.SetData(squareCoords, team, board, position);
		newPiece.SetTypePiece(newPiece.GetType().Name.ToUpper());
		
		newPiece.name = newPiece.position.x.ToString()+ " " + newPiece.position.y.ToString();

		newPiece.FixRotation();

		Material teamMaterial = newPiece.SetMaterialChess(team, index, b);
		newPiece.SetMaterial(teamMaterial);

		board.SetPieceOnBoard(squareCoords, newPiece);

		ChessPlayer currentPlayer = team == TeamColor.White ? whitePlayer : blackPlayer;
		currentPlayer.AddPiece(newPiece);
	}

	internal void SetupCamera(TeamColor team)
	{
		cameraSetup.SetupCamera(team);
	}

	private void GenerateAllPossiblePlayerMoves(ChessPlayer player)
	{
		player.GenerateAllPossibleMoves();
	}

	public bool IsTeamTurnActive(TeamColor team)
	{
		return activePlayer.team == team;
	}

	public void EndTurn()
	{
		GenerateAllPossiblePlayerMoves(activePlayer);
		GenerateAllPossiblePlayerMoves(GetOpponentToPlayer(activePlayer));

		if (GameIsFinished())
		{
			EndGame();
		}
		else
		{
			ChangeActiveTeam();
		}

		isPlayerTurn = isPlayerTurn ? false : true;
		
		if (!isPlayerTurn)
		{
			StartCoroutine(Wait());
		}
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(2f);
		//board.ClearBoardAndSetup();
		Move move = ab.GetMove();
		_DoAIMove(move);

		EndTurn();
	}


	private bool GameIsFinished()
	{
		Piece[] kingAttackingPieces = activePlayer.GetPieceAtackingOppositePiceOfType<King>();

		//ChessPlayer oppositePlayer = GetOpponentToPlayer(activePlayer);
		Piece[] isKing = activePlayer.GetPiecesOfType<King>();
		
		if (kingAttackingPieces.Length > 0)
		{
			ChessPlayer oppositePlayer = GetOpponentToPlayer(activePlayer);
			Piece attackedKing = oppositePlayer.GetPiecesOfType<King>().FirstOrDefault();
			oppositePlayer.RemoveMovesEnablingAttakOnPieceOfType<King>(activePlayer, attackedKing);

			int avaliableKingMoves = attackedKing.avaliableMoves.Count;

			if (avaliableKingMoves == 0)
			{
				bool canCoverKing = oppositePlayer.CanHidePieceFromAttack<King>(activePlayer);
				//if (!canCoverKing) ;
					//return true;
			}
		}
		return false;
	}

	private void EndGame()
	{
		SetGameState(GameState.Finished);
		UIManager.OnGameFinished(activePlayer.team.ToString());
	}

	internal void RestartGame()
	{
		DestroyPieces();
		board.OnGameRestarted();
		whitePlayer.OnGameRestarted();
		blackPlayer.OnGameRestarted();
		StartNewGame();
	}

	internal void KillGame()
	{
		DestroyPieces();
		board.DeselectPiece();
		if(board) Destroy(board.gameObject);
		whitePlayer.OnGameRestarted();
		blackPlayer.OnGameRestarted();
	}

	private void DestroyPieces()
	{
		whitePlayer.activePieces.ForEach(p => Destroy(p.gameObject));
		blackPlayer.activePieces.ForEach(p => Destroy(p.gameObject));
	}

	private void ChangeActiveTeam()
	{
		if(activePlayer == whitePlayer) activePlayer = blackPlayer;
		else activePlayer = whitePlayer;

		if(PhotonNetwork.InRoom)
		{
			if (UIManager.networkManager.chessGameController.IsLocalPlayersTurn())
			{
				UiUtils.Instance.OnlinePopup(true, 4f, "Your turn!");
			}
			else if (gameObject.name == "MultiplayerGameMaster")
			{
				UiUtils.Instance.OnlinePopup(true, 4f, "Opponent turn!");
			}
		}
		else
        {
	        SetupCamera(activePlayer.team);
		}
	}

	private ChessPlayer GetOpponentToPlayer(ChessPlayer player)
	{
		return player == whitePlayer ? blackPlayer : whitePlayer;
	}

	public void OnPieceRemoved(Piece piece)
	{
		ChessPlayer pieceOwner = (piece.team == TeamColor.White) ? whitePlayer : blackPlayer;
		pieceOwner.RemovePiece(piece);

		if(piece.GetComponent<King>())
		{
			Debug.Log("King Kill | Team: " + pieceOwner.team);
			EndGame();
		}
	}
	
	void _DoAIMove(Move move)
	{
		board.SwapPieces(move);
	}

	

//	public void RemoveMovesEnablingAttakOnPieceOfType<T>(Piece piece) where T : Piece
//	{
//		activePlayer.RemoveMovesEnablingAttakOnPieceOfType<T>(GetOpponentToPlayer(activePlayer), piece);
//	}
}


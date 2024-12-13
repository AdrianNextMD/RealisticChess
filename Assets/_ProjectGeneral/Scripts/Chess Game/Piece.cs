using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MUnityUtils.ExtensionMethods;
using NaughtyAttributes;
#if UNITY_EDITOR
using UnityEditor;
#endif
[RequireComponent(typeof(MaterialSetter))]
[RequireComponent(typeof(IObjectTweener))]
public abstract class Piece : MonoBehaviour
{
	public enum PieceType { KING, QUEEN, BISHOP, ROOK, KNIGHT, PAWN, UNKNOWN = -1};
	public PieceType _type = PieceType.UNKNOWN;
	
	[SerializeField] private MaterialSetter materialSetter;
	
	public Material[] materials;
	public Board board { protected get; set; }
	public Vector2Int occupiedSquare { get; set; }
	public TeamColor team { get; set; }
	public bool hasMoved { get; private set; }

	private MoveFactory factory;

	public List<Move> moves = new List<Move>();
	public Tile tile;
	public List<Vector2Int> avaliableMoves;
	public Vector2 position;

	private IObjectTweener tweener;
	
	private bool _hasMoved = false;
	public bool HasMoved
	{
		get { return _hasMoved; }
		set { _hasMoved = value; }
	}

	public TeamColor Player
	{
		get { return  team; }
	}

	public abstract List<Vector2Int> SelectAvaliableSquares();

	private void Awake()
	{
		avaliableMoves = new List<Vector2Int>();
		tweener = GetComponent<IObjectTweener>();
		materialSetter = GetComponent<MaterialSetter>();
		hasMoved = false;
	}

	public void FixRotation()
    {
		if(team == TeamColor.White)
		{
			transform.rotation = Quaternion.Euler(0, 180, 0);
		}
    }

	public void GetMoves()
	{
		moves.Clear();
		moves = factory.GetMoves(this, position);
	}

	public void SetTypePiece(string name)
	{
		PieceType type = (PieceType) Enum.Parse(typeof(PieceType), name);
		_type = type;
	}

	public void SetMaterial(Material selectedMaterial)
	{
		materialSetter.SetSingleMaterial(selectedMaterial);
	}

	public bool IsFromSameTeam(Piece piece)
	{
		return team == piece.team;
	}

	public bool CanMoveTo(Vector2Int coords)
	{
		return avaliableMoves.Contains(coords);
	}
	
	public virtual void MovePiece(Vector2Int coords)
	{ 
		Vector3 targetPosition = board.CalculatePositionFromCoords(coords);
		occupiedSquare = coords;
		hasMoved = true;
		tweener.MoveTo(transform, targetPosition);
	}
	
	public virtual void MovePieceAI(Vector2Int coords)
	{ 
		Vector3 targetPosition = board.CalculatePositionFromCoordsAI(coords);
		occupiedSquare = coords;
		hasMoved = true;
		tweener.MoveTo(transform, targetPosition);
	}

	protected void TryToAddMove(Vector2Int coords)
	{
		avaliableMoves.Add(coords);
	}

	public void SetData(Vector2Int coords, TeamColor team, Board board, Vector2 position)
	{
		this.position = position;
		this.team = team;
		occupiedSquare = coords;
		this.board = board;
		transform.position = board.CalculatePositionFromCoords(coords);
		
		factory	= new MoveFactory(board);	}

	public bool IsAttackingPieceOfType<T>() where T : Piece
	{
		foreach (var square in avaliableMoves)
		{
			if (board.GetPieceOnSquare(square) is T)
				return true;
		}
		return false;
	}

	public Material SetMaterialChess(TeamColor team, int index, bool b)
	{
		if (b)
		{
			return team == TeamColor.White ? materials[0] : materials[1];
		}
		else
		{
			return team == TeamColor.White
				? StaticActions.shopItems.shopData[index].whiteMaterial
				: StaticActions.shopItems.shopData[index].blackMaterial;
		}
	}
	
}
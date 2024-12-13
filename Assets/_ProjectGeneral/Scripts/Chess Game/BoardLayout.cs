using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;

public enum TeamColor
{
    Black = 0, White = 1, UNKNOW = -1
}

public enum PieceType
{
    Pawn, Bishop, Knight, Rook, Queen, King
}

[CreateAssetMenu(menuName = "Scriptable Objects/Board/Layout")]
public class BoardLayout : ScriptableObject
{
    [Serializable]
    private class BoardSquareSetup
    {
        public Vector2Int position;
        
        [Space(5f)]
        public Vector2 piecePosAI;
        [Space(5f)]
        
        public PieceType PieceType;
        public TeamColor teamColor;
    }

    [SerializeField] private BoardSquareSetup[] boardSquares;

    public int GetPiecesCount()
    {
        return boardSquares.Length;
    }
    
    public Vector2Int GetSquareCoordsAtIndex(int index)
    {
        return new Vector2Int(boardSquares[index].position.x - 1, boardSquares[index].position.y - 1);
    }
    public string GetSquarePieceNameAtIndex(int index)
    {
        return boardSquares[index].PieceType.ToString();
    }

    public Vector2 GetPositionPiece(int i)
    {
        return boardSquares[i].piecePosAI;
    }
    
    public TeamColor GetSquareTeamColorAtIndex(int index)
    {
        return boardSquares[index].teamColor;
    }

}

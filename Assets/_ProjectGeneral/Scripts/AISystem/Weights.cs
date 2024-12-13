using UnityEngine;
using System.Collections;

public class Weights : MonoBehaviour
{
    int[,] PawnBoardWeight = new int[,]
    {
        {0,  0,  0,  0,  0,  0,  0,  0},
       {50, 50, 50, 50, 50, 50, 50, 50},
        {10, 10, 20, 30, 30, 20, 10, 10},
        {5,  5, 10, 25, 25, 10,  5,  5},
        {0,  0,  0, 20, 20,  0,  0,  0},
        {5, -5,-10,  0,  0,-10, -5,  5},
        {5, 10, 10,-20,-20, 10, 10,  5},
        {0,  0,  0,  0,  0,  0,  0,  0}
    };

    int[,] PawnMirrorBoardWeight = new int[,]
    {
        {0,  0,  0,  0,  0,  0,  0,  0},
        {5, 10, 10,-20,-20, 10, 10,  5},
        {5, -5,-10,  0,  0,-10, -5,  5},
        {0,  0,  0, 20, 20,  0,  0,  0},
        {5,  5, 10, 25, 25, 10,  5,  5},
        {10, 10, 20, 30, 30, 20, 10, 10},
        {50, 50, 50, 50, 50, 50, 50, 50},
        {0,  0,  0,  0,  0,  0,  0,  0}
    };

    int[,] KnightBoardWeight = new int[,]
    {
        {-50,-40,-30,-30,-30,-30,-40,-50},
        {-40,-20,  0,  0,  0,  0,-20,-40},
        {-30,  0, 10, 15, 15, 10,  0,-30},
        {-30,  5, 15, 20, 20, 15,  5,-30},
        {-30,  0, 15, 20, 20, 15,  0,-30},
        {-30,  5, 10, 15, 15, 10,  5,-30},
        {-40,-20,  0,  5,  5,  0,-20,-40},
        {-50,-40,-30,-30,-30,-30,-40,-50}
    };

    int[,] KnightMirrorBoardWeight = new int[,]
    {
        {-50,-40,-30,-30,-30,-30,-40,-50},
        {-40,-20,  0,  5,  5,  0,-20,-40},
        {-30,  5, 10, 15, 15, 10,  5,-30},
        {-30,  0, 15, 20, 20, 15,  0,-30},
        {-30,  5, 15, 20, 20, 15,  5,-30},
        {-30,  0, 10, 15, 15, 10,  0,-30},
        {-40,-20,  0,  0,  0,  0,-20,-40},
        {-50,-40,-30,-30,-30,-30,-40,-50}
    };

    int[,] BishopBoardWeight = new int[,]
    {
        {-20,-10,-10,-10,-10,-10,-10,-20},
        {-10,  0,  0,  0,  0,  0,  0,-10},
        {-10,  0,  5, 10, 10,  5,  0,-10},
        {-10,  5,  5, 10, 10,  5,  5,-10},
        {-10,  0, 10, 10, 10, 10,  0,-10},
        {-10, 10, 10, 10, 10, 10, 10,-10},
        {-10,  5,  0,  0,  0,  0,  5,-10},
        {-20,-10,-10,-10,-10,-10,-10,-20}
    };

    int[,] BishopMirrowBoardWeight = new int[,]
    {
        {-20,-10,-10,-10,-10,-10,-10,-20},
        {-10,  5,  0,  0,  0,  0,  5,-10},
        {-10, 10, 10, 10, 10, 10, 10,-10},
        {-10,  0, 10, 10, 10, 10,  0,-10},
        {-10,  5,  5, 10, 10,  5,  5,-10},
        {-10,  0,  5, 10, 10,  5,  0,-10},
        {-10,  0,  0,  0,  0,  0,  0,-10},
        {-20,-10,-10,-10,-10,-10,-10,-20}
    };

    int[,] RookBoardWeight = new int[,]
    {
        {0,  0,  0,  0,  0,  0,  0,  0},
        {5, 10, 10, 10, 10, 10, 10,  5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {0,  0,  0,  5,  5,  0,  0,  0}
    };

    int[,] RookMirrorBoardWeight = new int[,]
    {
        {0,  0,  0,  5,  5,  0,  0,  0},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {5, 10, 10, 10, 10, 10, 10,  5},
        {0,  0,  0,  0,  0,  0,  0,  0}
    };

    int[,] QueenBoardWeight = new int[,]
    {
        {-20,-10,-10, -5, -5,-10,-10,-20},
        {-10,  0,  0,  0,  0,  0,  0,-10},
        {-10,  0,  5,  5,  5,  5,  0,-10},
        {-5,  0,  5,  5,  5,  5,  0, -5},
        {0,  0,  5,  5,  5,  5,  0, -5},
        {-10,  5,  5,  5,  5,  5,  0,-10},
        {-10,  0,  5,  0,  0,  0,  0,-10},
        {-20,-10,-10, -5, -5,-10,-10,-20}
    };

    int[,] QueenMirrorBoardWeight = new int[,]
    {
        {-20,-10,-10, -5, -5,-10,-10,-20},
        {-10,  0,  5,  0,  0,  0,  0,-10},
        {-10,  5,  5,  5,  5,  5,  0,-10},
        {0,  0,  5,  5,  5,  5,  0, -5},
        {-5,  0,  5,  5,  5,  5,  0, -5},
        {-10,  0,  5,  5,  5,  5,  0,-10},
        {-10,  0,  0,  0,  0,  0,  0,-10},
        {-20,-10,-10, -5, -5,-10,-10,-20}
    };

    int[,] KingBoardWeight =
    {
        {-30,-40,-40,-50,-50,-40,-40,-30},
        {-30,-40,-40,-50,-50,-40,-40,-30},
        {-30,-40,-40,-50,-50,-40,-40,-30},
        {-30,-40,-40,-50,-50,-40,-40,-30},
        {-20,-30,-30,-40,-40,-30,-30,-20},
        {-10,-20,-20,-20,-20,-20,-20,-10},
        {20, 20,  0,  0,  0,  0, 20, 20},
        {20, 30, 10,  0,  0, 10, 30, 20}
    };

    int[,] KingMirrorBoardWeight =
    {
        {20, 30, 10,  0,  0, 10, 30, 20},
        {20, 20,  0,  0,  0,  0, 20, 20},
        {-10,-20,-20,-20,-20,-20,-20,-10},
        {-20,-30,-30,-40,-40,-30,-30,-20},
        {-30,-40,-40,-50,-50,-40,-40,-30},
        {-30,-40,-40,-50,-50,-40,-40,-30},
        {-30,-40,-40,-50,-50,-40,-40,-30},
        {-30,-40,-40,-50,-50,-40,-40,-30},
    };

    int[,] KingEndBoardWeight =
    {
        {-50,-40,-30,-20,-20,-30,-40,-50},
        {-30,-20,-10,  0,  0,-10,-20,-30},
        {-30,-10, 20, 30, 30, 20,-10,-30},
        {-30,-10, 30, 40, 40, 30,-10,-30},
        {-30,-10, 30, 40, 40, 30,-10,-30},
        {-30,-10, 20, 30, 30, 20,-10,-30},
        {-30,-30,  0,  0,  0,  0,-30,-30},
        {-50,-30,-30,-30,-30,-30,-30,-50}
    };

    int[,] KingEndMirrorBoardWeight =
    {
        {-50,-30,-30,-30,-30,-30,-30,-50},
        {-30,-30,  0,  0,  0,  0,-30,-30},
        {-30,-10, 20, 30, 30, 20,-10,-30},
        {-30,-10, 30, 40, 40, 30,-10,-30},
        {-30,-10, 30, 40, 40, 30,-10,-30},
        {-30,-10, 20, 30, 30, 20,-10,-30},
        {-30,-20,-10,  0,  0,-10,-20,-30},
        {-50,-40,-30,-20,-20,-30,-40,-50}
    };
    
    public int GetBoardWeight(Piece.PieceType type, Vector2 position, TeamColor color)
    {
        switch (type)
        {
            case Piece.PieceType.PAWN:
                if (color == TeamColor.Black)
                    return PawnBoardWeight[(int)position.x, (int)position.y];
                else
                    return PawnMirrorBoardWeight[(int)position.x, (int)position.y];
            case Piece.PieceType.ROOK:
                if (color == TeamColor.Black)
                    return RookBoardWeight[(int)position.x, (int)position.y];
                else
                    return RookMirrorBoardWeight[(int)position.x, (int)position.y];
            case Piece.PieceType.KNIGHT:
                if (color == TeamColor.Black)
                    return KnightBoardWeight[(int)position.x, (int)position.y];
                else
                    return KnightMirrorBoardWeight[(int)position.x, (int)position.y];
            case Piece.PieceType.BISHOP:
                if (color == TeamColor.Black)
                    return BishopBoardWeight[(int)position.x, (int)position.y];
                else
                    return BishopMirrowBoardWeight[(int)position.x, (int)position.y];
            case Piece.PieceType.QUEEN:
                if (color == TeamColor.Black)
                    return QueenBoardWeight[(int)position.x, (int)position.y];
                else
                    return QueenMirrorBoardWeight[(int)position.x, (int)position.y];
            case Piece.PieceType.KING:
                if (color == TeamColor.Black)
                    return KingBoardWeight[(int)position.x, (int)position.y];
                else
                    return KingMirrorBoardWeight[(int)position.x, (int)position.y];
            default:
                return -1;
        }
    }

    public int GetPieceWeight(Piece.PieceType type)
    {
        switch (type)
        {
            case Piece.PieceType.PAWN:
                return 1;
            case Piece.PieceType.ROOK:
                return 5;
            case Piece.PieceType.KNIGHT:
                return 3;
            case Piece.PieceType.BISHOP:
                return 3;
            case Piece.PieceType.QUEEN:
                return 9;
            case Piece.PieceType.KING:
                return 1000000;
            default:
                return -1;
        }
    }
}

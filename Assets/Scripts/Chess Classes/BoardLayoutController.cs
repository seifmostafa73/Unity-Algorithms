using System;
using Chess_Classes;
using Chess_Classes.Pieces;
using UnityEngine;


[CreateAssetMenu(menuName = "ChessGame/ScriptableObjects/BoardLayout")]
public class BoardLayoutController : ScriptableObject
{
    [Serializable]
    class BoardSquareSetup
    {
        public Vector2Int  position;
        public PieceType   pieceType;
        public TeamColor   team;
    }

    [SerializeField] BoardSquareSetup[] boardSquareSetup;

    public int GetPieceCount()
    {
        return boardSquareSetup.Length;
    }

    public Vector2Int GetSquareCoordinate(int index)
    {
        if(index<0 || index >= GetPieceCount())
        {
            Debug.Log("Index is not valid");
            return new Vector2Int(-1,-1);
        }
        return boardSquareSetup[index].position;
    }
    public PieceType GetPieceTypeAt(int index)
    {
        if(index<0 || index >= GetPieceCount())
        {
            Debug.Log("Index is not valid");
            return PieceType.Pawn;
        }

        return boardSquareSetup[index].pieceType;
    }

    public TeamColor GetSquareColorAt(int index)
    {
        if(index<0 || index >= GetPieceCount())
        {
            Debug.Log("Index is not valid");
            return TeamColor.White;
        }
        return boardSquareSetup[index].team;
    }

}

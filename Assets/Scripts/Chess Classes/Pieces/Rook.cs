using System.Collections.Generic;
using Chess_Classes;
using UnityEngine;

public class Rook : Piece {

    private Vector2Int[] directions = {Vector2Int.up,Vector2Int.down,Vector2Int.left,Vector2Int.right};

    override public List<Vector2Int> SelectAvailbleSquare() {
        int range = board.boardSize;
        availableMoves.Clear();
        foreach (Vector2Int direction in directions)
            for(int i =1;i<= range;i++)
            {
                Vector2Int newSquare = occupiedSquare + (direction * i);
                if(board.CheckSquareValidity(newSquare))
                {
                    var piece = board.GetPieceOnSquare(newSquare);
                    if(piece==null)
                        AddToAvailableMoves(newSquare);
                    else if(piece.IsFromeSameTeam(this))
                        break;
                    else //if(!piece.IsFromeSameTeam(this))
                    {
                        AddToAvailableMoves(newSquare);
                        break;
                    }
                }
            }
        return availableMoves;
    }
}



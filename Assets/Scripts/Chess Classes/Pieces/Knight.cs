using Chess_Classes;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece {
    private Vector2Int[] directions = {
        new Vector2Int(2, 1),
        new Vector2Int(2, -1),
        new Vector2Int(1, 2),
        new Vector2Int(1, -2),
        new Vector2Int(-2, 1),
        new Vector2Int(-2, -1),
        new Vector2Int(-1, 2),
        new Vector2Int(-1, -2),};

    override public List<Vector2Int> SelectAvailbleSquare() {
        int range = board.boardSize;
        availableMoves.Clear();
        foreach (Vector2Int direction in directions) {
            Vector2Int newSquare = occupiedSquare + (direction);
            if (board.CheckSquareValidity(newSquare))
            {
                var piece = board.GetPieceOnSquare(newSquare);
                if (piece == null)
                    AddToAvailableMoves(newSquare);
                else if(!piece.IsFromeSameTeam(this))
                {
                    AddToAvailableMoves(newSquare);
                    break;
                }
            }
        }
        return availableMoves;
    }
}

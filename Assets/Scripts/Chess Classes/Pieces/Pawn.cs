using Chess_Classes;
using Chess_Classes.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece {
    private Vector2Int[] directions = {Vector2Int.up,Vector2Int.down,Vector2Int.left,Vector2Int.right};

    override public List<Vector2Int> SelectAvailbleSquare() {
        availableMoves.Clear();

        Vector2Int direction = team == TeamColor.White ? Vector2Int.up : Vector2Int.down;
        float range = hasMoved ? 1 : 2;
        for (int i = 1; i <= range; i++) // this loop checks for no attacking moves
        {
            Vector2Int nextCoords = occupiedSquare + direction * i;
            if (!board.CheckSquareValidity(nextCoords))
                break;
            Piece piece = board.GetPieceOnSquare(nextCoords);
            if (piece == null)
                AddToAvailableMoves(nextCoords);
            else
                break;
        }


        Vector2Int[] takeDirections = new Vector2Int[] { new Vector2Int(1, direction.y), new Vector2Int(-1, direction.y) };
        for (int i = 0; i < takeDirections.Length; i++) // this loop checks for the attacking moves
        {
            Vector2Int nextCoords = occupiedSquare + takeDirections[i];
            if (!board.CheckSquareValidity(nextCoords))
                continue;
            Piece piece = board.GetPieceOnSquare(nextCoords);

            if (piece != null && !piece.IsFromeSameTeam(this))
            {
                AddToAvailableMoves(nextCoords);
            }
        }
        return availableMoves;
    }
}


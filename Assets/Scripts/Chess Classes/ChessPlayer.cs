using System.Collections.Generic;
using UnityEngine;

namespace Chess_Classes {
    public class ChessPlayer {
        public TeamColor team;
        public Board board;
        public List<Piece> activePieces;

        public ChessPlayer(TeamColor _team,Board _board)
        {
            this.team = _team;
            this.board = _board;
            this.activePieces = new List<Piece>();
        }

        public void AddPiece(Piece piece)
        {
            if(!activePieces.Contains(piece))
            {
                activePieces.Add(piece);
            }
        }
        public void RemovePiece(Piece piece)
        {
            if(activePieces.Contains(piece))
            {
                activePieces.Remove(piece);
            }
        }
        public void GenerateAllMoves()
        {
            foreach (var _piece in activePieces)
            {
                if(board.HasPiece(_piece)){
                    _piece.SelectAvailbleSquare();
                }
            }
        }
    }
}

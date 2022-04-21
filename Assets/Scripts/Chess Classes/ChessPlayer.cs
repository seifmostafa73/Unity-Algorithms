using System.Collections.Generic;

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
        //TODO: returns an array of all player's pieces attacking a piece of given type (T==King -> returns all piceses attacking the king)
        public Piece[] GetPieceAttacking<T>() where T: Piece
        {
            return  new Piece[0];
        }

        //TODO: returns an array of all player's pieces of given type
        public Piece[] GetPiecesOfType<T>() where T: Piece
        {
            return new Piece[0];
        }
        ///TODO: this function removes any available moves that won't protect the piece of a given type
        ///go throught all available moves of the selectedPiece , try all it's moves,
        /// if any move doesn't change the piece of type (T) being under attack , add this move to the list of moves to be Removed from avail.
        public void RemoveMovesEnablingAttackOnPiece<T>(ChessPlayer opponentPlayer, Piece selectedPiece) where T: Piece
        {

        }
        ///TODO: this goes through all the active player pieces and check if any are attacking a piece of given type
        private bool IsAttackingType<T>() where T: Piece
        {
            return false;
        }

    }
}

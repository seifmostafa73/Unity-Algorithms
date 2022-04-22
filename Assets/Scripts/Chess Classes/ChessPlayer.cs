using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// returns an array of all player's pieces attacking a piece of given type (T==King -> returns all piceses attacking the king)
        /// </summary>
        /// <typeparam name="T">Type of piece being attacked</typeparam>
        /// <returns> array of all pieces attacking it</returns>
        public Piece[] GetPieceAttacking<T>() where T: Piece
        {
            List<Piece> attackingPieces = new List<Piece>() ;
            foreach (Piece _piece in activePieces)
            {
                    if(_piece.IsAttackingPieceOfType<T>())
                    {
                        attackingPieces.Add(_piece);
                    }
            }
            return attackingPieces.ToArray();
        }

        /// <summary>
        /// returns an array of all player's pieces of given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Piece[] GetPiecesOfType<T>() where T: Piece
        {
            return activePieces.Where(piece => piece is T).ToArray();
        }


        /// <summary>
        /// this function removes any available moves that won't protect the piece of a given type
        /// go throught all available moves of the selectedPiece , try all it's moves,
        /// if any move doesn't change the piece of type (T) being under attack , add this move to the list of moves to be Removed from avail.
        /// </summary>
        /// <typeparam name="T">Type of Attacked Piece</typeparam>
        /// <param name="opponentPlayer">the attacking player</param>
        /// <param name="selectedPiece"> the attacked piece</param>
        public void RemoveMovesEnablingAttackOnPiece<T>(ChessPlayer opponentPlayer, Piece selectedPiece) where T: Piece
        {
            List<Vector2Int> invalidMoves = new List<Vector2Int>();
            foreach (var selectedPieceMove in selectedPiece.availableMoves)
            {
                var oldCoordinate = selectedPiece.occupiedSquare;
                var oldPiece = board.GetPieceOnSquare(selectedPieceMove);
                board.UpdateBoard(selectedPieceMove,selectedPiece,oldCoordinate,null);
                opponentPlayer.GenerateAllMoves();
                if(opponentPlayer.IsAttackingType<T>() )
                {
                    invalidMoves.Add(selectedPieceMove);
                }
                board.UpdateBoard(oldCoordinate,selectedPiece,selectedPieceMove,oldPiece);
            }

            foreach (var invalidMove in invalidMoves)
                selectedPiece.availableMoves.Remove(invalidMove);

        }

        /// <summary>
        /// this goes through all the active player pieces and check if any are attacking a piece of given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns> true if there is a valid piece</returns>
        private bool IsAttackingType<T>() where T: Piece
        {
            foreach (Piece _piece in activePieces)
            {
                if (_piece.IsAttackingPieceOfType<T>()){
                    return true;
                }
            }
            return false;
        }

        public bool CanHidePieceFromAttack<T>(ChessPlayer currentTurnPlayer0) where T: Piece
        {
            return false;
        }

    }
}

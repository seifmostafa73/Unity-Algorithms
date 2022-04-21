using System.Collections.Generic;
using UnityEngine;
namespace Chess_Classes {
    [RequireComponent(typeof(MaterialSetter))]

    public abstract class Piece : MonoBehaviour {
        [SerializeField] MaterialSetter materialSetter;
        [SerializeField] IMotionTweener tweener;
        protected Board board;
        public Vector2Int occupiedSquare;
        public TeamColor team;
        public bool hasMoved = false;
        public List<Vector2Int> availableMoves;

        void Awake() {
            availableMoves = new List<Vector2Int>();
            tweener = GetComponent<IMotionTweener>();
            materialSetter = GetComponent<MaterialSetter>();
            hasMoved = false;
        }

        public abstract List<Vector2Int> SelectAvailbleSquare();

        public bool IsFromeSameTeam(Piece ComparedPiece) {
            return (this.team == ComparedPiece.team);
        }

        public bool CanMoveTo(Vector2Int Position) {
            return availableMoves.Contains(Position);
        }

        public void MovePiece(Vector2Int Position) {
            occupiedSquare = Position;
            hasMoved = true;
            tweener.MoveTo(transform , board.CalculatePosition(Position)); //Moves the game object from one place to another
        }

        public void SetPieceData(Vector2Int _occupiedSquare, TeamColor _team, Board _board) {
            occupiedSquare = _occupiedSquare;
            team = _team;
            board = _board;
            if(team == TeamColor.Black)
            {
                this.transform.position = board.CalculatePosition(_occupiedSquare);
                this.transform.RotateAround (transform.position, transform.up, 180f);
            }
            else
                this.transform.position =  board.CalculatePosition(_occupiedSquare);
        }

        protected void AddToAvailableMoves(Vector2Int Move) {
            availableMoves.Add(Move);
        }

        public void SetMaterial(Material _material) {
            materialSetter.SetMaterial(_material);
        }

    }
}
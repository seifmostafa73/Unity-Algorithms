using System.Collections.Generic;
using UnityEngine;
namespace Chess_Classes {
    [RequireComponent(typeof(SquareSelectorManager))]
    public class Board : MonoBehaviour {
        [SerializeField]float boardSquareSize; //1.6 *1.6
        [SerializeField]Transform A1Square;
        public int boardSize = 8;
        [SerializeField]ChessGameController chessGameController;
        [SerializeField] Piece selectedPiece;
        //SquareSelectorManager squareSelectorManager;
        public Piece[,] grid;

        SquareSelectorManager _squareSelectorManager;

        void Awake() {
            _squareSelectorManager = GetComponent<SquareSelectorManager>();
            CreateGrid();
        }

        /// <summary>
        /// inializes the grid array
        /// </summary>
        private void CreateGrid() {
            this.grid = new Piece[boardSize,boardSize];
        }

        /// <summary>
        /// Updates the grid array values (swaps the old piece position with the new one's)
        /// </summary>
        public void UpdateBoard(Vector2Int newCoordinates,Piece newPiece,Vector2Int oldCoordinates,Piece oldPiece) {

            SetPieceOnSquare(oldCoordinates,oldPiece);
            SetPieceOnSquare(newCoordinates,newPiece);

            PrintBoard();
        }

        /// <summary>
        /// Selects the piece clicked on (if it meets the right conditions )
        /// and calls the SquareSelectorManager class to highlight the available moves
        /// </summary>
        private void SelectPiece(Piece piece) {
            chessGameController.RemoveMovesEnablingAttakOnPieceOfType<King>(piece);
            selectedPiece = piece;
            List<Vector2Int> availableMoves = piece.availableMoves;
            ShowAvailableMoves(availableMoves);
        }

        private void ShowAvailableMoves(List<Vector2Int> availableMoves) {
            Dictionary<Vector3,bool> availableSquares = new Dictionary<Vector3, bool>();

            foreach(var move in availableMoves)
            {
                Vector3 square = CalculatePosition(move);
                bool isSquareFree = GetPieceOnSquare(move) == null;
                availableSquares.Add(square,isSquareFree);
            }

            _squareSelectorManager.ShowSelection(availableSquares);
        }
        /// <summary>
        /// Deslects the currently selected piece
        /// and calls the SquareSelectorManager class to hide the available moves
        /// </summary>
        private void DeselectPiece() {
            selectedPiece = null;
            _squareSelectorManager.ClearSelection();
        }

        /// <summary>
        /// checks if the selected square is wihtin the board bounds
        /// </summary>
        public bool CheckSquareValidity(Vector2Int square)
        {
            if(square.x <0 || square.y <0 || square.y >=boardSize || square.x >= boardSize)
                return false;
            return true;
        }

        /// <summary>
        /// handles whether to select/Deselect/Take the clicked piece
        /// </summary>
        /// TODO: only allwo to select if Game in progress

        public void OnSquareSelected(Vector3 squarePosition)
        {
            Vector2Int squareCoordinates = CalculatedCoordinates(squarePosition);
            //Debug.Log(squareCoordinates);
            if(!CheckSquareValidity(squareCoordinates)) return;


            Piece pieceOnSquare = GetPieceOnSquare(squareCoordinates);

            if(pieceOnSquare == null)
            {
                if(selectedPiece !=null && selectedPiece.CanMoveTo(squareCoordinates))
                {
                    Debug.Log("Moving to "+ squareCoordinates);
                    OnSelectedPieceMoved(squareCoordinates,selectedPiece);
                }else
                {
                    DeselectPiece();
                }
            }else
            {
                if(pieceOnSquare == selectedPiece)
                    DeselectPiece();
                else if(selectedPiece !=null && selectedPiece.CanMoveTo(squareCoordinates))
                {
                    Debug.Log("Moving to "+ squareCoordinates);
                    OnSelectedPieceMoved(squareCoordinates,selectedPiece);
                }
                else if(selectedPiece == null && pieceOnSquare.team == chessGameController.GetActivePlayer().team)
                {
                    SelectPiece(pieceOnSquare);
                    Debug.Log("Selected : "+selectedPiece.team+" "+selectedPiece+" " + selectedPiece.occupiedSquare.x+","+ selectedPiece.occupiedSquare.y);
                }
            }
        }

        /// <summary>
        /// Given a board co-ordinate (x,y) returns the actual world co-ordinates (x,y,z)
        /// </summary>
        /// <param name="Coordinates">Board-relative x ,y</param>
        public Vector3 CalculatePosition(Vector2Int Coordinates) {
            return A1Square.position+new Vector3(Coordinates.x *boardSquareSize,0,Coordinates.y * boardSquareSize);
        }

        /// <summary>
        /// this retruns a Vecotr2Int indicating the relative pos to the Board
        /// </summary>
        /// <param name="pos">world clicked Position</param>
        public Vector2Int CalculatedCoordinates(Vector3 pos) {
            int xCoordinate = Mathf.FloorToInt(transform.InverseTransformPoint(pos).x / boardSquareSize) + boardSize / 2;// you need to take into account the local position of the give Vector with respect to the board , so InverseTransformPoint Change from global to local
            int yCoordinate = Mathf.FloorToInt(transform.InverseTransformPoint(pos).z / boardSquareSize) + boardSize / 2; // the boardsize/2 assuming the pivot of the board is at the middle
            return new Vector2Int(xCoordinate,yCoordinate);
        }
        /// <summary>
        /// loops through the grid array and checks if the given piece exists
        /// </summary>
        public bool HasPiece(Piece piece)
        {
            for(int row = 0;row < boardSize;row++)
                for(int col = 0;col<boardSize;col++)
                    if(grid[row,col] == piece)
                    return true;

            return false;
        }

        /// <summary>
        /// returns the piece on the given square co-ordinates
        /// </summary>
        public Piece GetPieceOnSquare(Vector2Int squareCoordinates)
        {
            return grid[squareCoordinates.x,squareCoordinates.y];
        }

        /// <summary>
        /// sets the value of grid array's element of corressponding <square> co-ordinate  to the given piece
        /// <param name="square">the selected co-ordinates from which we will set the grid's element value</param>
        /// <param name="piece">the value of the grid's element</param>
        /// </summary>
        public void SetPieceOnSquare(Vector2Int square, Piece piece)
        {
            if(CheckSquareValidity(square))
            {
                grid[square.x, square.y] = piece;
            }else
                Debug.LogWarning("the entered square is not valid");
        }

        /// <summary>
        /// Moves the actual pieces objects
        /// <param name="squareCoordinates">Square that the selected piece moved to</param>
        /// <param name="piece">Selected piece</param>
        /// </summary>
        public void OnSelectedPieceMoved(Vector2Int squareCoordinates, Piece piece) {
            TryTakeOpponentPiece(squareCoordinates);

            var oldCoordinates = piece.occupiedSquare;
            UpdateBoard(squareCoordinates,piece,oldCoordinates,null);
            selectedPiece.MovePiece(squareCoordinates);
            DeselectPiece();
            chessGameController.EndTurn();
        }

        /// <summary>
        /// print the grid array as a easy to read form
        /// </summary>
        void PrintBoard()
        {
            string BoardString = "";
            for(int y =7;y>=0;y--) {
                BoardString += y+" : ";
                for (int x = 0; x < 8; x++) {
                    if (grid[x, y] != null)
                        BoardString += "     "+grid[x,y].team.ToString()[0]+"-"+grid[x,y].name.Substring(0,2) + "     ";// we take only first 3 letters to avoid printing overkill
                    else
                        BoardString += "    ####    ";
                }
                BoardString+="\n";
            }
            Debug.Log(BoardString);
        }

        private void TryTakeOpponentPiece(Vector2Int squareCoordinates) {
            var opponentPiece = GetPieceOnSquare(squareCoordinates);
            if(opponentPiece!= null && opponentPiece.team != chessGameController.GetActivePlayer().team)
            {
                chessGameController.GetOpponentToPlayer().RemovePiece(opponentPiece);
                Destroy(opponentPiece.gameObject);
            }
        }



    }
}
using Chess_Classes.Pieces;
using UnityEngine;
namespace Chess_Classes{
[RequireComponent(typeof(PieceCreator))]
public class ChessGameController : MonoBehaviour
{
    enum State{Start,Play,Finish};
    [SerializeField] BoardLayoutController initialBoardLayout;
    [SerializeField] Board currentBoard;
    ChessPlayer whitePlayer;
    ChessPlayer blackPlayer;
    ChessPlayer CurrentTurnPlayer;
    [SerializeField] PieceCreator pieceCreator;
    State gameState;
    void Awake()
    {
        setDependency();
        CreatePlayers(); // Creates both the white and black player
    }


    void Start()
    {
        RestartGameBoard();
    }
    private void setDependency()
    {
        if(pieceCreator == null)
        pieceCreator = this.GetComponent<PieceCreator>();
    }
    private void RestartGameBoard()
    {
        SetGameState(State.Start);
        CurrentTurnPlayer = whitePlayer;
        CreatePieceFromBoardLayout(initialBoardLayout);
        GeneratePlayerValidMoves(CurrentTurnPlayer);
        SetGameState(State.Play);
    }
    public ChessPlayer GetActivePlayer()
    {
        return CurrentTurnPlayer;
    }
    private  void CreatePlayers()
    {
        whitePlayer = CreatePlayer(TeamColor.White,this.currentBoard);
        blackPlayer = CreatePlayer(TeamColor.Black,this.currentBoard);
    }
    private  ChessPlayer CreatePlayer(TeamColor color,Board _board)
    {
        return new ChessPlayer(color,_board);
    }
    private void SetGameState(State state)
    {
        gameState = state;
    }

    private bool IsGameInProgress()
    {
        return gameState == State.Play;
    }

    /// <summary>
    /// Checks if the opponent's king is under attack by atleast one of the player's pieces, and king can't be protected or hide
    /// </summary>
    /// <returns> state of the game </returns>
    public bool IsGameFinished()
    {
        ///when is game finished ?
        /// 1- you need the opponents King to be under attack
        ChessPlayer opponent = GetOpponentToPlayer();
        Piece[] attackingPieces = CurrentTurnPlayer.GetPieceAttacking<King>();
        Debug.Log("Attacking pieces : "+attackingPieces.Length);
        if(attackingPieces.Length > 0)
        {
            var kingUnderAttack = opponent.GetPiecesOfType<King>()[0];

            //we need first to update the available moves to check if they are valid
            opponent.RemoveMovesEnablingAttackOnPiece<King>(CurrentTurnPlayer,kingUnderAttack);
        ///2-King doesn't have any other available moves
        ///3- no piece can block the attack
            if(kingUnderAttack.availableMoves.Count == 0 && !opponent.CanHidePieceFromAttack<King>(CurrentTurnPlayer))
            {
                Debug.Log("GameFinished : Lost ->" + opponent.team);
                return true;
            }
        }
        return false;
    }

    private void InitializeGameBoard()
    {

    }
    private void GeneratePlayerValidMoves(ChessPlayer player)
    {
        player.GenerateAllMoves();
    }
    private void ChangePlayerTurn()
    {
        CurrentTurnPlayer = GetOpponentToPlayer();
    }
    public ChessPlayer GetOpponentToPlayer()
    {
        return (CurrentTurnPlayer == whitePlayer)?blackPlayer : whitePlayer;
    }
    public void CreatePieceFromBoardLayout(BoardLayoutController boardLayout)
    {
        PieceType currentPieceType;
        TeamColor currentTeamColor;
        Vector2Int currentPiecePosition;
        for (int i =0; i< boardLayout.GetPieceCount() ;i++)
        {
            currentPieceType = boardLayout.GetPieceTypeAt(i);
            currentTeamColor = boardLayout.GetSquareColorAt(i);
            currentPiecePosition = boardLayout.GetSquareCoordinate(i);

            CreatePiece(currentPiecePosition,currentTeamColor,currentPieceType);
        }
    }
    public void CreatePiece(Vector2Int Coordinates,TeamColor teamColor,PieceType pieceType)
    {
        GameObject chessPieceInstance = pieceCreator.CreatePiece(pieceType);
        if(chessPieceInstance == null) return;

        var chessPiece = chessPieceInstance.GetComponent<Piece>();

        chessPiece.SetPieceData(Coordinates,teamColor,currentBoard);
        chessPiece.SetMaterial(pieceCreator.GetTeamMaterial(teamColor));

        currentBoard.SetPieceOnSquare(Coordinates,chessPiece);

        ChessPlayer currentPlayer = (teamColor == TeamColor.White) ? whitePlayer : blackPlayer;
        currentPlayer.AddPiece(chessPiece);
    }
    public void EndTurn()
    {
        Debug.Log("TurnEnded");
        GeneratePlayerValidMoves(CurrentTurnPlayer);
        GeneratePlayerValidMoves(GetOpponentToPlayer());

        if(IsGameFinished())
            EndGame();
        else {
            ChangePlayerTurn();
        }
    }
    public void EndGame()
    {
        Debug.LogWarning("Game Ended");
        gameState = State.Finish;
    }

    internal void RemoveMovesEnablingAttakOnPieceOfType<T>(Piece piece) where T : Piece
    {
        CurrentTurnPlayer.RemoveMovesEnablingAttackOnPiece<T>(GetOpponentToPlayer(), piece);
    }

    internal void OnPieceRemoved(Piece piece)
    {
        ChessPlayer pieceOwner = (piece.team == TeamColor.White) ? whitePlayer : blackPlayer;
        pieceOwner.RemovePiece(piece);
    }
}
}

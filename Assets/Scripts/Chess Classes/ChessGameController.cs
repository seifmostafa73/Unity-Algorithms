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
        CreatePlayers(); // Creates both thw white and black player
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
    //Todo: cghange the current state to the given state
    private void SetGameState(State state)
    {

    }
    //Todo: checks if the state is = playing
    private bool IsGameInprogress()
    {
        return true;
    }
    ///TODO: checks if the opponent's player's king is being attacked , and doesn't have any available moves ,
    ///  AND all pieces availble can't protect him
    ///
    private bool IsGameFinished()
    {
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
    private ChessPlayer GetOpponentToPlayer()
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
    public bool IsGameInProgress()
    {
        return  true;
    }
    //Todo:check if game is ended (win/ lose /draw)
    public void EndTurn()
    {
        GeneratePlayerValidMoves(CurrentTurnPlayer);
        GeneratePlayerValidMoves(GetOpponentToPlayer());
        ChangePlayerTurn();
    }
    //TODO:sets the state to finished
    public void EndGame()
    {

    }
}
}

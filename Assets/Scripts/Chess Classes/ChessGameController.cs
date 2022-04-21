using System;
using Chess_Classes.Pieces;
using UnityEngine;
namespace Chess_Classes{
[RequireComponent(typeof(PieceCreator))]

public class ChessGameController : MonoBehaviour
{
    [SerializeField] BoardLayoutController initialBoardLayout;
    [SerializeField] Board currentBoard;
    ChessPlayer whitePlayer;
    ChessPlayer blackPlayer;
    ChessPlayer CurrentTurnPlayer;
    [SerializeField] PieceCreator pieceCreator;

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
        CurrentTurnPlayer = whitePlayer;
        CreatePieceFromBoardLayout(initialBoardLayout);
        GeneratePlayerValidMoves(CurrentTurnPlayer);
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
    private void setGameState()
    {

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
    public void EndTurn()
    {
        GeneratePlayerValidMoves(CurrentTurnPlayer);
        GeneratePlayerValidMoves(GetOpponentToPlayer());
        ChangePlayerTurn();
    }
}
}

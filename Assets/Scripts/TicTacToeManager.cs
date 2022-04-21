using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TicTacToeManager : MonoBehaviour
{
    //USing minimax Algorithm
    [Serializable]enum Charactersenum
    {
        None,
        X,
        O
    }

   [Serializable] struct EmptyCell
    {
        public int Col;
        public int Row;

       public EmptyCell(int X,int Y)
       {
           Col = X;Row = Y;
       }
   }

    int INF = int.MaxValue;
    public int NoOfCells = 9;
    [SerializeField]Button[] GridButtons;
    [SerializeField] Charactersenum[,] BoardArray = new Charactersenum[3,3]{
        {Charactersenum.None, Charactersenum.None, Charactersenum.None},
        {Charactersenum. None,Charactersenum.None, Charactersenum.None},
        {Charactersenum.None, Charactersenum.None, Charactersenum.None} };
    [SerializeField] TMP_Text ResultText = null;

    public bool IsXTrun = true;
    public static TicTacToeManager TTTManager;

    // Start is called before the first frame update
    void Start()
    {
        ResultText.text = "";
        ResultText.rectTransform.parent.GetComponent<RectTransform>().localScale = new Vector3(0,0,0);
        TTTManager = this;
        //if(!IsXTrun) minimax(BoardArray,10,true);
       // printBoardSorted();
    }

    public  void ResetTheGame()
    {
        ResultText.text = "";
        ResultText.rectTransform.parent.GetComponent<RectTransform>().localScale = new Vector3(0,0,0);
        NoOfCells = 9;
        BoardArray = new Charactersenum[3,3]{
            {Charactersenum.None, Charactersenum.None, Charactersenum.None},
            {Charactersenum. None,Charactersenum.None, Charactersenum.None},
            {Charactersenum.None, Charactersenum.None, Charactersenum.None} };

        foreach (Button B in GridButtons)
        {
            GameObject.Find(B.name+"/X").GetComponent<Image>().enabled = false;
            GameObject.Find(B.name+"/O").GetComponent<Image>().enabled = false;
            B.GetComponent<Button>().enabled = true;

        }
    }

    public void ButtonClick(int index)
    {

        int X = index%3;
        int Y = (int) (index / 3);

        Charactersenum IndexValue = (IsXTrun)? Charactersenum.X: Charactersenum.O ;
        BoardArray[X,Y] = IndexValue;

        var CLickedButton = EventSystem.current.currentSelectedGameObject;


        GameObject.Find(CLickedButton.name+"/X").GetComponent<Image>().enabled = IsXTrun;
        //GameObject.Find(CLickedButton.name+"/O").SetActive(!IsXTrun);

        CLickedButton.GetComponent<Button>().enabled = false;

        //IsXTrun = !IsXTrun;
       // printBoardSorted();
        NoOfCells--;

        if( CheckForWinner() ){
            ResultText.text = "Winner Is"+ IndexValue;
            ResultText.color = new Color32(0x93,0xD3,0x1c,0xff);
            ResultText.rectTransform.parent.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        }
        else if(CheckForTie() )
        {
            ResultText.text = "It's a Tie !";
            ResultText.color = new Color32(0x93,0xD3,0x1c,0xff);
            ResultText.rectTransform.parent.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        }else
        {
        GetAiBestMove(BoardArray,NoOfCells,true);
       // printBoardSorted();
        }
    }

    void GetAiBestMove(Charactersenum[,] BoardPos,int computeDepth,bool OTurn)//X is minimum O is max
    {
        //we compute all the remaining posibilties, which is the number of available empty postions, if it is 0 then the game should end

        if(OTurn)
        {
            int MaxEvaluation = -INF;
            EmptyCell BestMove = new EmptyCell(0,0);


            for(int i =0;i<3;i++)
                for(int j =0 ;j<3;j++){

                    if(BoardPos[i,j] == Charactersenum.None) {

                        BoardPos[i, j] = Charactersenum.O;
                        int MinimaxValue = minimax(BoardPos, computeDepth,-INF,INF, false);
                        BoardPos[i, j] = Charactersenum.None;

                        if (MinimaxValue >= MaxEvaluation)
                        {
                            MaxEvaluation = MinimaxValue;
                            BestMove.Col = i;
                            BestMove.Row = j;
                        }
                    }
                }

            //Debug.LogWarning(MaxEvaluation+"Best Move is "+ BestMove.Col +"  "+BestMove.Row);
            BoardPos[BestMove.Col,BestMove.Row] = Charactersenum.O;

            var ClickedButton =  GridButtons[BestMove.Row*3 + BestMove.Col];
           // GameObject.Find(ClickedButton.name+"/X").GetComponent<Image>().enabled = false;
            GameObject.Find(ClickedButton.name+"/O").GetComponent<Image>().enabled = true;

            ClickedButton.GetComponent<Button>().enabled = false;

            NoOfCells--;
            if( CheckForWinner() ){
                ResultText.text = "Winner Is the AI";
                ResultText.color = new Color32(200, 62, 18, 255);
                ResultText.rectTransform.parent.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            }
            else if(CheckForTie())
            {
                ResultText.text = "It's a Tie !";
                ResultText.color = new Color32(0x93,0xD3,0x1c,0xff);
                ResultText.rectTransform.parent.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            }
        }
        //else
        //{Debug.Log("Error Here");}
    }

    bool CheckForTie()
    {
        if (CheckForWinner()){return false;}
        for(int i =0;i<3;i++)
            for(int j=0;j<3;j++)
            {
                if (BoardArray[i,j] == Charactersenum.None)
                {
                    return false;
                }
            }
        return  true;
    }

    bool CheckForWinner()
    {
        bool Win = false;
        for(int Row=0;Row<3;Row++) {
            Win |= ( BoardArray[0,Row] != Charactersenum.None
            && (BoardArray[0, Row] == BoardArray[1, Row])
            && (BoardArray[1, Row] == BoardArray[2, Row]));
        }
        for(int Col=0;Col<3;Col++) {
            Win |= BoardArray[Col,0] != Charactersenum.None
            && (BoardArray[Col, 0] == BoardArray[Col, 1])
            && (BoardArray[Col, 1] == BoardArray[Col, 2]);
        }

        Win |= BoardArray[0,0] != Charactersenum.None &&(BoardArray[0, 0] == BoardArray[1, 1]) && (BoardArray[1, 1] == BoardArray[2, 2]);
        Win |= BoardArray[2,0] != Charactersenum.None &&(BoardArray[2, 0] == BoardArray[1, 1]) && (BoardArray[1, 1] == BoardArray[0, 2]);

        return Win;
    }

    void printBoardSorted()
    {
        Debug.Log("**********Board Aray: \n "
                +"\t"+BoardArray[0,0]+"\t"+BoardArray[1,0]+"\t"+BoardArray[2,0]+"\n"+
                "\t"+BoardArray[0,1]+"\t"+BoardArray[1,1]+"\t"+BoardArray[2,1]+"\n"+
                "\t"+BoardArray[0,2]+"\t"+BoardArray[1,2]+"\t"+BoardArray[2,2]+"\n"
        );
    }
    private int minimax(Charactersenum[,] BoardPos, int Depth,int Alpha,int Beta, bool OTurn) {

        bool GameOver = CheckForTie() || CheckForWinner();

        if(GameOver)
        {
            int Score = (OTurn && CheckForWinner())?-1:1;
            return Depth*Score;
        }
        if(OTurn) // maximaizing
        {
            int MaxEvaluation = -INF;

            for(int i =0;i<3;i++)
                for(int j =0 ;j<3;j++){
                    if(BoardPos[i,j] == Charactersenum.None){

                        BoardPos[i,j] = Charactersenum.O;
                        int MinimaxValue = minimax(BoardPos,Depth-1,Alpha,Beta,false);
                        BoardPos[i,j] = Charactersenum.None;

                        if(MinimaxValue >= MaxEvaluation)
                        {
                            MaxEvaluation = MinimaxValue;
                        }
                        if(MinimaxValue > Alpha)
                        {
                            Alpha = MinimaxValue;
                        }
                        if(Beta<=Alpha) break;
                    }
                }
            return MaxEvaluation;

        }
        else // minimizing
        {
            int MinEvaluation = INF;

            for(int i =0;i<3;i++)
                for(int j =0 ;j<3;j++){
                    if(BoardPos[i,j] == Charactersenum.None){

                        BoardPos[i,j] = Charactersenum.X;
                        int MinimaxValue = minimax(BoardPos,Depth-1,Alpha,Beta,true);
                        BoardPos[i,j] = Charactersenum.None;

                        if(MinimaxValue <= MinEvaluation)
                        {
                            MinEvaluation = MinimaxValue;
                        }
                        if(MinimaxValue < Beta)
                        {
                            Beta = MinimaxValue;
                        }
                        if(Beta<=Alpha) break;
                }
                }
            return MinEvaluation;
        }
    }
}

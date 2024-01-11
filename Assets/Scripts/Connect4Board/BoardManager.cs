using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardManager : Singleton<BoardManager>
{
    [SerializeField] public int RowsNumber = 6;
    [SerializeField] public int ColumnsNumber = 7;

    private Token[,] Matrix;


    public UnityAction BoardIsFull;
    public UnityAction<List<Token>> HasAWinner;
    public bool FlagWin = false;


    public BoardManager()
    {
        Matrix = new Token[RowsNumber, ColumnsNumber];
        BoardHelper.FillBoard(Matrix);
    }

    public void CleanBoard()
    {
        foreach (Token token in Matrix)
        {
            Destroy(token.VisualToken);
        }

        Matrix = new Token[RowsNumber, ColumnsNumber];
        BoardHelper.FillBoard(Matrix);
    }


    #region ----- Modifications Board
    public bool TryAddToken(Token token, int column)
    {
        if ((token.IdOwner != 0 && token.IdOwner != 1) || column >= ColumnsNumber || column < 0)
        {
            Debug.LogError("Error from Update Board");
            return false;
        }

        if(!GetDropPossibles().Contains(column))
        {
            Debug.Log("Player try to drop in a already full column");
            return false;
        }
   
        int rowDropped = BoardHelper.AddToken(Matrix, token, column);

        CheckIfWin(token, rowDropped, column);
        Debug.Log("GAME BOARD");
        BoardHelper.PrintBoard(Matrix);
        return true;
    }

    public List<int> GetDropPossibles()
    {
        List<int> drops = BoardHelper.ComputeDropPossibles(Matrix);
        if(drops.Count == 0) BoardIsFull?.Invoke();
        return drops;
    }

    public void DebugPrintGameBoard()
    {
        BoardHelper.PrintBoard(Matrix);
    }

    public Token[,] GetEnvironnementObservation()
    {
        int rows = Matrix.GetLength(0);
        int columns = Matrix.GetLength(1);

        Token[,] copy = new Token[rows, columns];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                copy[i, j] = (Token)Matrix[i, j]?.Clone();
            }
        }
        return copy;
    }

    #endregion

    #region Victory Conditions
    private void CheckIfWin(Token token, int row, int column)
    {
        if(IsVictoryLine(BoardHelper.CheckHorizontally(Matrix, token, row, column))) return;
        if(IsVictoryLine(BoardHelper.CheckVertically(Matrix, token, row, column))) return;
        if(IsVictoryLine(BoardHelper.CheckDiagL(Matrix, token, row, column))) return;
        if(IsVictoryLine(BoardHelper.CheckDiagR(Matrix, token, row, column))) return;
        return;
    }

    private bool IsVictoryLine(List<Token> list)
    {
        if(list.Count == 4) 
        {
            FlagWin = true;
            HasAWinner?.Invoke(list);
            return true;
        }
        return false;
    }
    #endregion

}
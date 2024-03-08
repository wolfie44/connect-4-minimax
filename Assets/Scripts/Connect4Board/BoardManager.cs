using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Handle all the data-side of the game
public class BoardManager : Singleton<BoardManager>
{
    [SerializeField] public int RowsNumber = 6;
    [SerializeField] public int ColumnsNumber = 7;

    private Token[,] Matrix;

    // Events others script will subscribe
    public UnityAction BoardIsFull;
    public UnityAction<List<Token>> HasAWinner;
    
    public bool FlagWin = false;

    #region ----- Modifications Board
    public void Init()
    {
        Matrix = new Token[RowsNumber, ColumnsNumber];
        BoardHelper.FillBoard(Matrix);
    }

    public void CleanBoard()
    {
        FlagWin = false;
        foreach (Token token in Matrix)
        {
            Destroy(token.VisualToken);
        }

        Matrix = new Token[RowsNumber, ColumnsNumber];
        BoardHelper.FillBoard(Matrix);
    }
   
    public bool TryAddToken(Token token, int column)
    {
        if ((token.IdOwner != 0 && token.IdOwner != 1) || column >= ColumnsNumber || column < 0)
        {
            Debug.Log("Error from Update Board");
            return false;
        }

        if(!GetDropPossibles().Contains(column))
        {
            Debug.Log("Player try to drop in a already full column");
            return false;
        }
   
        int rowDropped = BoardHelper.AddToken(Matrix, token, column);
        CheckVictory(token, rowDropped, column);
        // Debug.Log("GAME BOARD");
        // BoardHelper.PrintBoard(Matrix);
        return true;
    }
    #endregion

    #region ----- Utilities
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

    // Return a copy of the board for the IA belief system
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
    private void CheckVictory(Token token, int row, int column)
    { 
        if(BoardHelper.CheckVictory(Matrix))
        {
            List<Token> winningLine = BoardHelper.GetWinningSequence(Matrix, token.IdOwner);
            FlagWin = true;
            HasAWinner?.Invoke(winningLine);
        }
    }
    #endregion
}
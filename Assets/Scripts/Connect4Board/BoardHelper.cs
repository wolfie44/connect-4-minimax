using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardHelper
{

    public static void FillBoard(Token[,] matrix)
    {
        for(int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Token token = new Token(null, 9);
                matrix[i, j] = token;
            }
        }
    }

    public static List<int> ComputeDropPossibles(Token[,] matrix)
    {
        List<int> dropPossibles = new List<int>();

        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            if (matrix[0, j].IdOwner == 9)
            {
                dropPossibles.Add(j);
            }
        }
        return dropPossibles;
    }

    
    public static int AddToken(Token[,] matrix, Token token, int column)
    {
        int rowDropped = matrix.GetLength(0) - 1;
        for (int row = rowDropped; row >= 0; row--)
        {
            if (matrix[row, column].IdOwner == 9)
            {
                rowDropped = row;
                matrix[row, column] = token;
                break;
            }
        }
        return rowDropped;
    }

    public static void RemoveToken(Token[,] matrix, int row, int column)
    {
        matrix[row, column] = new Token(null,9);
    }

    public static List<Token> CheckHorizontally(Token[,] matrix,Token token, int row, int column)
    {
        List<Token> lineWinning = new List<Token>();
        int x = 1;
        bool stop = column - x < 0;

        lineWinning.Add(token);

        while (stop == false && matrix[row, column - x].IdOwner == token.IdOwner )
        {
            lineWinning.Add(matrix[row, column - x]);
            x++;
            stop |= column - x < 0;
        }

        x = 1;
        stop = column + x >= matrix.GetLength(1);
        while (stop == false && matrix[row, column + x].IdOwner == token.IdOwner )
        {
            lineWinning.Add(matrix[row, column + x]);
            x++;
            stop |= column + x >= matrix.GetLength(1);
        }

        return lineWinning;
    }

    public static List<Token> CheckVertically(Token[,] matrix, Token token, int row, int column)
    {
        List<Token> lineWinning = new List<Token>();
        int x = 1;
        bool stop = row - x < 0;
        lineWinning.Add(token);

        while (stop == false && matrix[row - x, column].IdOwner == token.IdOwner )
        {
            lineWinning.Add(matrix[row - x, column]);
            x++;
            stop |= row - x < 0;
        }

        x = 1;
        stop = row + x >= matrix.GetLength(0);
        while (stop == false && matrix[row + x, column].IdOwner == token.IdOwner )
        {
            lineWinning.Add(matrix[row + x, column]);
            x++;
            stop |= row + x >= matrix.GetLength(0);
        }

        return lineWinning;
    }

    public static List<Token> CheckDiagL(Token[,] matrix, Token token, int row, int column)
    {
        List<Token> lineWinning = new List<Token>();
        int x = 1; 
        bool stop = row - x < 0 || column - x < 0;
        lineWinning.Add(token);

        while (stop == false && matrix[row - x, column - x].IdOwner == token.IdOwner )
        {
            lineWinning.Add(matrix[row - x, column - x]);
            x++;
            stop |= row - x < 0 || column - x < 0;
        }

        x = 1;
        stop = row + x >= matrix.GetLength(0) || column + x >= matrix.GetLength(1);
        while (stop == false && matrix[row + x, column + x].IdOwner == token.IdOwner )
        {
            lineWinning.Add(matrix[row + x, column + x]);
            x++;
            stop |= row + x >= matrix.GetLength(0) || column + x >= matrix.GetLength(1);
        }

        return lineWinning;
    }

    public static List<Token> CheckDiagR(Token[,] matrix, Token token, int row, int column)
    {
        List<Token> lineWinning = new List<Token>();
        int x = 1;
        bool stop = row + x >= matrix.GetLength(0) || column - x < 0;
        lineWinning.Add(token);

        while (stop == false && matrix[row + x, column - x].IdOwner == token.IdOwner )
        {
            lineWinning.Add(matrix[row + x, column - x]);
            x++;
            stop |= row + x >= matrix.GetLength(0) || column - x < 0;
        }

        x = 1;
        stop = row - x < 0 || column + x >= matrix.GetLength(1);
        while (stop == false && matrix[row - x, column + x].IdOwner == token.IdOwner )
        {
            lineWinning.Add(matrix[row - x, column + x]);
            x++;
            stop |= row - x < 0 || column + x >= matrix.GetLength(1);
        }

        return lineWinning;
    }

    public static void PrintBoard(Token[,] matrix)
    {
        string print = "\n";
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                print += matrix[i, j].IdOwner + " ";
            }
            print += "\n" ;
        }

        Debug.Log(print);
    }
}
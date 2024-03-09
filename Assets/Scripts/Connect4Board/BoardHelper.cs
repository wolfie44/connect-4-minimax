using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;

// Utils function to work with a Token[,]
public class BoardHelper
{
    // 9 is used here to note an empty space
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

    // Return all columns not completly full 
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

    // Add player token to board, 0 or 1
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

    // Cancel a player play
    public static void RemoveToken(Token[,] matrix, int row, int column)
    {
        matrix[row, column] = new Token(null, 9);
    }

    // Return the first sequence of 4 token for a given player
    public static List<Token> GetWinningSequence(Token[,] matrix, ulong player)
    {
        List<Token> winningSequence = new List<Token>();

        int rows = matrix.GetLength(0);
        int colums = matrix.GetLength(1);       

        // Check horizontally
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < colums - 3; col++)
            {
                if (matrix[row, col].IdOwner == player && 
                    matrix[row, col + 1].IdOwner == player &&
                    matrix[row, col + 2].IdOwner == player && 
                    matrix[row, col + 3].IdOwner == player)
                {
                    winningSequence.Add(matrix[row, col]);
                    winningSequence.Add(matrix[row, col + 1]);
                    winningSequence.Add(matrix[row, col + 2]);
                    winningSequence.Add(matrix[row, col + 3]);
                    return winningSequence;
                }
            }
        }

        // Check vertically
        for (int col = 0; col < colums; col++)
        {
            for (int row = 0; row < rows - 3; row++)
            {
                if (matrix[row, col].IdOwner == player && 
                    matrix[row + 1, col].IdOwner == player &&
                    matrix[row + 2, col].IdOwner == player && 
                    matrix[row + 3, col].IdOwner == player)
                {
                    winningSequence.Add(matrix[row, col]);
                    winningSequence.Add(matrix[row + 1, col]);
                    winningSequence.Add(matrix[row + 2, col]);
                    winningSequence.Add(matrix[row + 3, col]);
                    return winningSequence;
                }
            }
        }

        // Check diagonally, bottom left to top right ++ ++
        for (int row = 0; row < rows - 3; row++)
        {
            for (int col = 0; col < colums - 3; col++)
            {
                if (matrix[row, col].IdOwner == player && 
                    matrix[row + 1, col + 1].IdOwner == player &&
                    matrix[row + 2, col + 2].IdOwner == player && 
                    matrix[row + 3, col + 3].IdOwner == player)
                {
                    winningSequence.Add(matrix[row, col]);
                    winningSequence.Add(matrix[row + 1, col + 1]);
                    winningSequence.Add(matrix[row + 2, col + 2]);
                    winningSequence.Add(matrix[row + 3, col + 3]);
                    return winningSequence;
                }
            }
        }

        // Check diagonally, top left to bottom right -- ++
        for (int row = 3; row < rows; row++)
        {
            for (int col = 0; col < colums - 3; col++)
            {
                if (matrix[row, col].IdOwner == player && 
                    matrix[row - 1, col + 1].IdOwner == player &&
                    matrix[row - 2, col + 2].IdOwner == player && 
                    matrix[row - 3, col + 3].IdOwner == player)
                {
                    winningSequence.Add(matrix[row, col]);
                    winningSequence.Add(matrix[row - 1, col + 1]);
                    winningSequence.Add(matrix[row - 2, col + 2]);
                    winningSequence.Add(matrix[row - 3, col + 3]);
                    return winningSequence;
                }
            }
        }

        return null;
    }

    // Return status of the board (victory or not)
    // Similar to GetWinningSequence but more efficient because used by the IA
    public static bool CheckVictory(Token[,] matrix)
    {
        ulong player = 0;
        int rows = matrix.GetLength(0);
        int colums = matrix.GetLength(1);

        // Check horizontally
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < colums - 3; col++)
            {
                player = matrix[row, col].IdOwner;

                if (player != 9 && 
                    matrix[row, col].IdOwner == player && 
                    matrix[row, col + 1].IdOwner == player &&
                    matrix[row, col + 2].IdOwner == player && 
                    matrix[row, col + 3].IdOwner == player)
                {
                    return true;
                }
            }
        }

        // Check vertically
        for (int col = 0; col < colums; col++)
        {
            for (int row = 0; row < rows - 3; row++)
            {
                player = matrix[row, col].IdOwner;

                if (player != 9 && 
                    matrix[row, col].IdOwner == player && 
                    matrix[row + 1, col].IdOwner == player &&
                    matrix[row + 2, col].IdOwner == player && 
                    matrix[row + 3, col].IdOwner == player)
                {
                    return true;
                }
            }
        }

        // Check diagonally, bottom left to top right ++ ++
        for (int row = 0; row < rows - 3; row++)
        {
            for (int col = 0; col < colums - 3; col++)
            {
                player = matrix[row, col].IdOwner;
                
                if (player != 9 && 
                    matrix[row, col].IdOwner == player && 
                    matrix[row + 1, col + 1].IdOwner == player &&
                    matrix[row + 2, col + 2].IdOwner == player && 
                    matrix[row + 3, col + 3].IdOwner == player)
                {
                    return true;
                }
            }
        }

        // Check diagonally, top left to bottom right -- ++
        for (int row = 3; row < rows; row++)
        {
            for (int col = 0; col < colums - 3; col++)
            {
                player = matrix[row, col].IdOwner;

                if (player != 9 && 
                    matrix[row, col].IdOwner == player && 
                    matrix[row - 1, col + 1].IdOwner == player &&
                    matrix[row - 2, col + 2].IdOwner == player && 
                    matrix[row - 3, col + 3].IdOwner == player)
                {
                    return true;
                }
            }
        }

        return false;
    }

    // DEBUG PRINT BOARD
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
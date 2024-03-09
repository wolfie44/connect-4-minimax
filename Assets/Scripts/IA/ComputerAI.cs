using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// Smart AI bot using Minimax algo
public class ComputerAI
{
    // Observe the environnement before choosing his move
    private Token[,] _Beliefs;

    // Minimax depth
    private int _depth = 4;

    private void ObserveEnvironnement()
    {
        _Beliefs = BoardManager.GetInstance().GetEnvironnementObservation();
        // Debug.Log("AI BELIEFS : ");
        // BoardHelper.PrintBoard(_Beliefs);
    }

    public void StartSearch()
    {
        int bestMove = -1;
        int bestScore = int.MinValue;

        ObserveEnvironnement();
        List<int> dropPossibles = BoardHelper.ComputeDropPossibles(_Beliefs);

        //Fill tree with all possibles moves for the IA
        foreach (int column in dropPossibles)
        {
            int row = BoardHelper.AddToken(_Beliefs, new Token(null, 1), column);
            int score = Minimax(_Beliefs, _depth, int.MinValue, int.MaxValue, false);
            BoardHelper.RemoveToken(_Beliefs, row, column);

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = column;
            }      
        }
        
        // Debug.Log($"Best val: {bestScore} for column {bestMove}");
        GameManager.GetInstance().AgentChoice(bestMove);
    }

    private int Minimax(Token[,] board, int depth, int alpha, int beta, bool maximizingPlayer)
    {
        List<int> dropPossibles = BoardHelper.ComputeDropPossibles(board);
        
        if (depth == 0 || dropPossibles.Count == 0 || CheckWin(board))
        {
            return Eval(board);
        }

        if (maximizingPlayer)
        {
            int maxEval = int.MinValue;
            foreach (int column in dropPossibles)
            {
                int row = BoardHelper.AddToken(board, new Token(null, 1), column);
                int eval = Minimax(board, depth - 1, alpha, beta, false);
                BoardHelper.RemoveToken(board, row, column);

                if (eval > maxEval)
                {
                    maxEval = eval;
                }

                alpha = Mathf.Max(alpha, maxEval);
                if (beta <= alpha)
                {
                    break;
                }
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            foreach (int column in dropPossibles)
            {
                int row = BoardHelper.AddToken(board, new Token(null, 0), column);
                int eval = Minimax(board, depth - 1, alpha, beta, true);
                BoardHelper.RemoveToken(board, row, column);

                if (eval < minEval)
                {
                    minEval = eval;
                }

                beta = Mathf.Min(beta, minEval);
                if (beta <= alpha)
                {
                    break;
                }
            }
            return minEval;
        }
    }

    bool CheckWin(Token[,] board)
    {
        return BoardHelper.CheckVictory(board);
    }

    // First draft of a straigt foward eval function
    // without heuristics
    private int Eval(Token[,] board)
    {
        int score = 0;
        int rows = board.GetLength(0);
        int columns = board.GetLength(1);
        
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int consecutivePlayer = 0;
                int consecutiveOpponent = 0;

                // Check horizontally
                for (int i = 0; i < Mathf.Min(4, columns - col); i++)
                {
                    if (board[row, col + i].IdOwner == 1)
                    {
                        consecutivePlayer++;
                    }
                    else if (board[row, col + i].IdOwner == 0)
                    {
                        consecutiveOpponent++;
                        // Reset if opponent
                        consecutivePlayer = 0; 
                    }
                    else
                    {
                        // Reset on empty space
                        consecutivePlayer = 0;
                        consecutiveOpponent = 0;
                    }

                    // Score is incremented or reduced everytime we uncounter a token
                    score += GetScore(consecutivePlayer, consecutiveOpponent);
                }

                consecutivePlayer = 0;
                consecutiveOpponent = 0;

                // Check vertically
                for (int i = 0; i < Mathf.Min(4, rows - row); i++)
                {
                    if (board[row + i, col].IdOwner == 1)
                    {
                        consecutivePlayer++;
                    }
                    else if (board[row + i, col].IdOwner == 0)
                    {
                        consecutiveOpponent++;
                        consecutivePlayer = 0;
                    }
                    else
                    {
                        consecutivePlayer = 0;
                        consecutiveOpponent = 0;
                    }

                    score += GetScore(consecutivePlayer, consecutiveOpponent);
                }

                consecutivePlayer = 0;
                consecutiveOpponent = 0;

                // Check diagonally ++ --
                for (int i = 0; i < Mathf.Min(4, Mathf.Min(rows - row, columns - col)); i++)
                {
                    if (board[row + i, col + i].IdOwner == 1)
                    {
                        consecutivePlayer++;
                    }
                    else if (board[row + i, col + i].IdOwner == 0)
                    {
                        consecutiveOpponent++;
                        consecutivePlayer = 0;
                    }
                    else
                    {
                        consecutivePlayer = 0;
                        consecutiveOpponent = 0;
                    }

                    score += GetScore(consecutivePlayer, consecutiveOpponent);
                }

                consecutivePlayer = 0;
                consecutiveOpponent = 0;

                // Check diagonally -- ++
                for (int i = 0; i < Mathf.Min(4, Mathf.Min(row + 1, columns - col)); i++)
                {
                    if (board[row - i, col + i].IdOwner == 1)
                    {
                        consecutivePlayer++;
                    }
                    else if (board[row - i, col + i].IdOwner == 0)
                    {
                        consecutiveOpponent++;
                        consecutivePlayer = 0;
                    }
                    else
                    {
                        consecutivePlayer = 0;
                        consecutiveOpponent = 0;
                    }

                    score += GetScore(consecutivePlayer, consecutiveOpponent);
                }
            }
        }
        return score;
    }

   private int GetScore(int playerConsecutive, int opponentConsecutive)
    {
        int score = 0;
        if (playerConsecutive == 4)
            score = 1000;
        else if(playerConsecutive == 3)
            score = 50;
        else if(playerConsecutive == 2)
            score = 10;
        else if(playerConsecutive == 1)
            score = 5;
        
        if (opponentConsecutive == 4)
            score -= 1000;
        else if(opponentConsecutive == 3)
            score -= 50;
        else if(opponentConsecutive == 2)
            score -= 10;
        else if(opponentConsecutive == 1)
            score -= 5;

        return score;
    }
}

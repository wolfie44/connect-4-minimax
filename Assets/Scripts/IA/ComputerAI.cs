using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ComputerAI
{
    private Token[,] _Beliefs;
    private int ColumnToPlay = 0;
    private int _IADepth = 4;

    private void ObserveEnvironnement()
    {
        _Beliefs = BoardManager.GetInstance().GetEnvironnementObservation();
        Debug.Log("AI BELIEFS : ");
        BoardHelper.PrintBoard(_Beliefs);
    }

    public void StartSearch()
    {
        ObserveEnvironnement();
        List<int> dropPossibles = BoardHelper.ComputeDropPossibles(_Beliefs);
        int val = Minimax(_Beliefs, _IADepth, int.MinValue, int.MaxValue, true);
        Debug.Log($"Best val: {val} for {ColumnToPlay}");
        GameManager.GetInstance().AgentChoice(ColumnToPlay);
    }

    private int Minimax(Token[,] board, int depth, int alpha, int beta, bool maximizingPlayer)
    {
        List<int> dropPossibles = BoardHelper.ComputeDropPossibles(board);
        
        if (depth == 0 || dropPossibles.Count == 0)
        {
            int eval = Eval(board);
            Debug.Log($"Eval: {eval}");
            return eval;
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
                    if (depth == _IADepth) ColumnToPlay = column;
                }

                alpha = Mathf.Max(alpha, maxEval);
                if (beta <= alpha)
                {
                    Debug.Log("Pruning in Max");
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
                    Debug.Log("Pruning in Min");
                    break;
                }
            }
            return minEval;
        }
    }

    int Eval(Token[,] board)
    {
        int score = 0;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                Token token = board[i, j];
                if (token.IdOwner == 0)
                {
                    score -= GetScore(BoardHelper.CheckHorizontally(board, token, i, j).Count);
                    score -= GetScore(BoardHelper.CheckVertically(board, token, i, j).Count);
                    score -= GetScore(BoardHelper.CheckDiagL(board, token, i, j).Count);
                    score -= GetScore(BoardHelper.CheckDiagR(board, token, i, j).Count);
                }
                else if (token.IdOwner == 1)
                {
                    score += GetScore(BoardHelper.CheckHorizontally(board, token, i, j).Count);
                    score += GetScore(BoardHelper.CheckVertically(board, token, i, j).Count);
                    score += GetScore(BoardHelper.CheckDiagL(board, token, i, j).Count);
                    score += GetScore(BoardHelper.CheckDiagR(board, token, i, j).Count);
                }
            }
        }
        return score;
    }

    private int GetScore(int cpt)
    {
        if (cpt >= 4) return 2000;
        else if (cpt == 3) return 40;
        else if (cpt == 2) return 5;
        return 0;
    }
}

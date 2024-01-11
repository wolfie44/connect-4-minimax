using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TestBoard : MonoBehaviour
{

    private BoardManager _boardManager;

    private Token tokenRose;
    private Token tokenYellow;


    void Start()
    {
        if(!GameManager.GetInstance().TestMode) return;
        _boardManager = BoardManager.GetInstance();
        tokenRose = new Token(null, 0);
        tokenYellow = new Token(null, 1);

        TestDropImpossible();
        TestHorrizontal();
        TestVerticalTrue();
        TestVerticalFalse();
        TestDiagR();
    }

    void TestDropImpossible()
    {
        _boardManager.TryAddToken(tokenRose, 1);
        _boardManager.TryAddToken(tokenYellow, 2);
        _boardManager.TryAddToken(tokenRose, 3);
        _boardManager.TryAddToken(tokenYellow, 3);
        _boardManager.TryAddToken(tokenRose, 3);
        _boardManager.TryAddToken(tokenRose, 3);
        _boardManager.TryAddToken(tokenYellow, 3);
        _boardManager.TryAddToken(tokenYellow, 3);
        Assert.AreEqual(6, _boardManager.GetDropPossibles().Count, "Drop impossible not set");
        _boardManager.DebugPrintGameBoard();
        _boardManager.CleanBoard();
    }

    private void TestDiagR()
    {
        _boardManager.TryAddToken(tokenRose, 0);
        _boardManager.TryAddToken(tokenYellow, 1);
        _boardManager.TryAddToken(tokenRose, 1);
        _boardManager.TryAddToken(tokenYellow, 2);
        _boardManager.TryAddToken(tokenYellow, 2);
        _boardManager.TryAddToken(tokenRose, 2);
        _boardManager.TryAddToken(tokenYellow, 3);
        _boardManager.TryAddToken(tokenYellow, 3);
        _boardManager.TryAddToken(tokenYellow, 3);
        _boardManager.TryAddToken(tokenRose, 3);
        Assert.IsTrue(_boardManager.FlagWin);
        _boardManager.DebugPrintGameBoard();
        _boardManager.CleanBoard();
    }

    void TestHorrizontal()
    {
        _boardManager.TryAddToken(tokenRose, 0);
        _boardManager.TryAddToken(tokenRose, 0);
        _boardManager.TryAddToken(tokenRose, 1);
        _boardManager.TryAddToken(tokenRose, 2);
        _boardManager.TryAddToken(tokenRose, 3);
        Assert.IsTrue(_boardManager.FlagWin);
        _boardManager.DebugPrintGameBoard();
        _boardManager.CleanBoard();
    }

    void TestVerticalTrue()
    {
        _boardManager.TryAddToken(tokenRose, 3);
        _boardManager.TryAddToken(tokenYellow, 3);
        _boardManager.TryAddToken(tokenYellow, 3);
        _boardManager.TryAddToken(tokenYellow, 3);
        _boardManager.TryAddToken(tokenYellow, 3);
        Assert.IsTrue(_boardManager.FlagWin);
        _boardManager.DebugPrintGameBoard();
        _boardManager.CleanBoard();
    }

    void TestVerticalFalse()
    {
        _boardManager.TryAddToken(tokenRose, 3);
        _boardManager.TryAddToken(tokenYellow, 3);
        _boardManager.TryAddToken(tokenYellow, 3);
        _boardManager.TryAddToken(tokenRose, 3);
        _boardManager.TryAddToken(tokenYellow, 3);
        Assert.IsFalse(_boardManager.FlagWin);
        _boardManager.DebugPrintGameBoard();
        _boardManager.CleanBoard();
    }

}

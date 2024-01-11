using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class GameInfo : ScriptableObject
{
    public int NumberOfRound;
    public int NumberOfPlayer;
    public int CurrentRound;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handle Max Number of Round Selection
public class MenuRoundSelection : MonoBehaviour
{
    public void UpdateValue(int value)
    {
        PlayerPrefs.SetInt(Constants.NumberOfRoundsKey, value);
    }
}

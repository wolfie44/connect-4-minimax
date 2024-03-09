using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Handle name of the player
public class MenuPlayerNameSelection : MonoBehaviour
{
    [SerializeField] private int playerID;

    public void UpdateName(string name)
    {
        if(playerID == 0)
        {
            PlayerPrefs.SetString(Constants.PlayerNameKey, name);
        }
        else 
        {
            PlayerPrefs.SetString(Constants.PlayerMultiLocalNameKey, name);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Local Player Entity
public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public PlayerLocalMovement PlayerLocalMovement;

    public ulong OwnerId;
    public string PlayerName;
    public int Score;

    public void Init()
    {     
        if(OwnerId == 0)
        {
            PlayerName = PlayerPrefs.GetString(Constants.PlayerNameKey, "PLAYER");
        }
        else if (OwnerId == 1)
        {
            PlayerName = PlayerPrefs.GetString(Constants.PlayerMultiLocalNameKey, "PLAYER");
        }

        Score = 0;
        PlayerLocalMovement.MoveAllowed = false;
    }

    public void SetVisible(bool visible)
    {
        GetComponent<MeshRenderer>().enabled = visible;
    }
}

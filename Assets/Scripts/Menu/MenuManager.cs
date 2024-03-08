using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Netcode;

// UI Events
public class MenuManager : MonoBehaviour
{
    [SerializeField] private string _nextSceneLocal;
    [SerializeField] private string _nextSceneOnline;

    void Start()
    {
        PlayerPrefs.SetInt(Constants.NumberOfRoundsKey, 3);
        PlayerPrefs.SetString(Constants.PlayerNameKey, "PLAYER 1");
        PlayerPrefs.SetString(Constants.PlayerMultiLocalNameKey, "PLAYER 2");
    }
    
    public void PlayLocal(bool solo)
    {
        if(solo)
        {
            PlayerPrefs.SetString(Constants.PlayerMultiLocalNameKey, "COMPUTER");
            PlayerPrefs.SetInt(Constants.PlayAgainstIAKey, 1);
        }
        else
        {
            PlayerPrefs.SetInt(Constants.PlayAgainstIAKey, 0);
        }
        
        SceneManager.LoadScene(_nextSceneLocal);
    }

    public void PlayOnline()
    {
        SceneManager.LoadScene(_nextSceneOnline);
    }

}

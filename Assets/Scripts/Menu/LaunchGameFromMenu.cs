using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LaunchGameFromMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField _PlayerName;
    [SerializeField] private TMP_InputField _PlayerName2;
    [SerializeField] private Button[] _RoundsNumberButton;

    [SerializeField] private string _NextScene;

    [SerializeField] private GameInfo _GameInfo;

    [SerializeField] private List<PlayerInfo> _PlayerInfo;

    public void Play(bool solo)
    {
        if (_PlayerName != null && !_PlayerName.text.Equals("")) 
        {
            _PlayerInfo[0].PlayerName = _PlayerName.text;            
        } 
        else 
        {
            Debug.Log("Missing player name");
            return;
        }

        if(solo)
        {
            _PlayerInfo[1].PlayerName = "COMPUTER";
        }
        else 
        {
            if (_PlayerName2 != null && !_PlayerName2.text.Equals("")) 
            {
                _PlayerInfo[1].PlayerName = _PlayerName2.text;            
            } 
            else 
            {
                Debug.Log("Missing player 2 name");
                return;
            }
        }
        
        
        if(!_RoundsNumberButton[0].interactable) _GameInfo.NumberOfRound = 1;
        else if (!_RoundsNumberButton[1].interactable) _GameInfo.NumberOfRound = 3;
        else if(!_RoundsNumberButton[2].interactable) _GameInfo.NumberOfRound = 7;
        else return;

        SceneManager.LoadScene(_NextScene);
    }

}

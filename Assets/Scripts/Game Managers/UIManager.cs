using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _Player1;
    [SerializeField]
    private TextMeshProUGUI _Player2;
    [SerializeField]
    private TextMeshProUGUI _Score1;
    [SerializeField]
    private TextMeshProUGUI _Score2;
    [SerializeField]
    private TextMeshProUGUI _Round;

    [SerializeField]
    private GameObject _PanelGame;
    [SerializeField]
    private GameObject _PanelVictory;
    [SerializeField]
    private TextMeshProUGUI _VictoryText;
    [SerializeField]
    private string _NextScene;
    [SerializeField]
    private GameObject _Game;

    [SerializeField]
    private GameInfo _GameInfo;
    [SerializeField]
    private List<PlayerInfo> _PlayerInfo;

    public void InitUI()
    {
        _Score1.color = _PlayerInfo[0].PlayerColor;
        _Score2.color = _PlayerInfo[1].PlayerColor;

        _Player1.text = _PlayerInfo[0].PlayerName;
        _Player2.text = _PlayerInfo[1].PlayerName;
         _Round.text = "ROUND " + _GameInfo.CurrentRound + " / " + _GameInfo.NumberOfRound;
    }

    public void UpdateUI()
    {
        _Score1.text = ""+ _PlayerInfo[0].PlayerScore;
        _Score2.text = ""+ _PlayerInfo[1].PlayerScore;
        _Round.text = "ROUND " + _GameInfo.CurrentRound + " / " + _GameInfo.NumberOfRound;
    }

    public void DisplayVictory(int win)
    {
        StartCoroutine(AnimationVictory());

        _VictoryText.text = _PlayerInfo[0].PlayerName + "      "+ _PlayerInfo[0].PlayerScore
        +" ----- "+ _PlayerInfo[1].PlayerScore+ "       "+ _PlayerInfo[1].PlayerName
        + "\n"+_PlayerInfo[win].PlayerName + " WIN !";
    }

    private IEnumerator AnimationVictory()
    {
        yield return new WaitForSeconds(5f);
        _Game.SetActive(false);
        _PanelGame.SetActive(false);
        _PanelVictory.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(_NextScene);
    }
}

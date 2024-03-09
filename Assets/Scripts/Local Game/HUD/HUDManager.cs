using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// UI for local play
public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _Player1;
    [SerializeField] private TextMeshProUGUI _Player2;
    [SerializeField] private TextMeshProUGUI _Score1;
    [SerializeField] private TextMeshProUGUI _Score2;
    [SerializeField] private TextMeshProUGUI _Round;

    [SerializeField] private GameObject _PanelGame;
    [SerializeField] private GameObject _PanelVictory;
    [SerializeField] private TextMeshProUGUI _VictoryText;
    [SerializeField] private string _NextScene;
    [SerializeField] private GameObject _Game;
    [SerializeField] private PlayerColors _colors;

    public void InitUI(string player1, string player2, int roundmax)
    {
        _Score1.color = _colors.Colors[0];
        _Score2.color = _colors.Colors[1];

        _Player1.text = player1;
        _Player2.text = player2;
         _Round.text = "ROUND 1" + " / " + roundmax;
    }

    public void UpdateUI(int score1, int score2, int currentround, int maxround)
    {
        _Score1.text = score1.ToString();
        _Score2.text = score2.ToString();
        _Round.text = "ROUND " + currentround + " / " + maxround;
    }

    public void DisplayVictory(string player1, string player2, int score1, int score2, string winner)
    {
        StartCoroutine(AnimationVictory());

        _VictoryText.text = player1 + "      "+ score1
        +" ----- "+ score2 + "       "+ player2
        + "\n"+ winner + " WON !";
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

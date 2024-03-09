using System.Collections;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

// UI
public class WinningScreenDisplay : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private GameManagerNetwork manager;
    [SerializeField] private GameObject _PanelGame;
    [SerializeField] private GameObject _PanelVictory;
    [SerializeField] private TextMeshProUGUI _VictoryText;
    [SerializeField] private GameObject _Game;

    public override void OnNetworkSpawn()
    {
        if (!IsClient) { return; }

        manager.LoserName.OnValueChanged += DisplayVictory;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsClient) { return; }

        manager.LoserName.OnValueChanged -= DisplayVictory;
    }


    private void DisplayVictory(FixedString32Bytes oldValue, FixedString32Bytes newValue)
    {
        if(newValue != "")
        {
            StartCoroutine(AnimationVictory());

            _VictoryText.text = manager.WinnerName.Value + "      "+ manager.WinnerScore.Value
            +" ----- "+ manager.LoserScore.Value + "       "+ manager.LoserName.Value
            + "\n"+ manager.WinnerName.Value + " WON !";
        }
    }

    private IEnumerator AnimationVictory()
    {
        yield return new WaitForSeconds(5f);
        _Game.SetActive(false);
        _PanelGame.SetActive(false);
        _PanelVictory.SetActive(true);
    }
}

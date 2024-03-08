using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

// UI
public class RoundsDisplay : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private GameManagerNetwork manager;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI roundMaxText;

    public override void OnNetworkSpawn()
    {
        if (!IsClient) { return; }

        manager.CurrentRound.OnValueChanged += HandleRoundChanged;
        HandleRoundChanged(0, manager.CurrentRound.Value);

        manager.NumberOfRoundsMax.OnValueChanged += HandleMaxRoundChanged;
        HandleMaxRoundChanged(0, manager.NumberOfRoundsMax.Value);
    }

    public override void OnNetworkDespawn()
    {
        if (!IsClient) { return; }

        manager.CurrentRound.OnValueChanged -= HandleRoundChanged;
        manager.NumberOfRoundsMax.OnValueChanged -= HandleMaxRoundChanged;
    }

    private void HandleMaxRoundChanged(int oldScore, int newScore)
    {
        roundMaxText.text = newScore.ToString();
    }

    private void HandleRoundChanged(int oldScore, int newScore)
    {
        roundText.text = "ROUND "+ newScore.ToString() + " /";
    }
}

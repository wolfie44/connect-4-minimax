using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

// UI
public class PlayerDisplay : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerEntity player;
    [SerializeField] private TextMeshProUGUI scoreTextLeft;
    [SerializeField] private TextMeshProUGUI nameTextLeft;

    [SerializeField] private TextMeshProUGUI scoreTextRight;
    [SerializeField] private TextMeshProUGUI nameTextRight;

    [SerializeField]
    private PlayerColors _colors;

    public override void OnNetworkSpawn()
    {
        if (!IsClient) { return; }

        player.Score.OnValueChanged += HandleScoreChanged;
        HandleScoreChanged(0, player.Score.Value);

        player.PlayerName.OnValueChanged += HandleNameChanged;
        HandleNameChanged("PLAYER", player.PlayerName.Value);

        if(OwnerClientId == 0)
        {
            scoreTextLeft.color = _colors.Colors[0];
        }
        else
        {
            scoreTextRight.color = _colors.Colors[1];
        }
    }

    public override void OnNetworkDespawn()
    {
        if (!IsClient) { return; }

        player.Score.OnValueChanged -= HandleScoreChanged;
        player.PlayerName.OnValueChanged -= HandleNameChanged;
    }

    private void HandleScoreChanged(int oldScore, int newScore)
    {
        if(OwnerClientId == 0)
        {
            scoreTextLeft.text = newScore.ToString();
        }
        else
        {
            scoreTextRight.text = newScore.ToString();
        }
    }

    private void HandleNameChanged(FixedString32Bytes oldName, FixedString32Bytes newName)
    {
        if(OwnerClientId == 0)
        {
            nameTextLeft.text = newName.ToString();
        }
        else
        {
            nameTextRight.text = newName.ToString();
        }
        
    }
}

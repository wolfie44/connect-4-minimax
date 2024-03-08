using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Unity.Collections;
using System;

// Handle Player Logic in Network
public class PlayerEntity : NetworkBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Material _tokenYellow;
    [SerializeField] private Material _tokenPink;
    [SerializeField] private GameObject _vfx;

    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();
    public NetworkVariable<int> Score = new NetworkVariable<int>();
    public NetworkVariable<bool> IsTurn = new NetworkVariable<bool>(false);

    public static event Action<PlayerEntity> OnPlayerSpawned;
    public static event Action<PlayerEntity> OnPlayerDespawned;


    public override void OnNetworkSpawn()
    {
        if(OwnerClientId == 0)
        {
            GetComponent<MeshRenderer>().material = _tokenYellow;
        }
        else
        {
            GetComponent<MeshRenderer>().material = _tokenPink;
        }

        if (IsServer)
        {
            UserData userData =
                HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);

            PlayerName.Value = userData.userName;

            OnPlayerSpawned?.Invoke(this);
        }

        IsTurn.OnValueChanged += HandleStatusChanged;
        HandleStatusChanged(false, IsTurn.Value);
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            OnPlayerDespawned?.Invoke(this);
        }
    }

    public void ChangePlayingStatus()
    {
        IsTurn.Value = !IsTurn.Value;
    }

    // Server tell clients last move was invalid
    // Last Dummy token will get destroyed
    [ClientRpc]
    public void DestroyUnvalidTokensClientRpc()
    {
        playerMovement.DestroyUnvalidTokens();
    }

    private void HandleStatusChanged(bool oldStatus, bool newStatus)
    {
        _vfx.SetActive(newStatus);
    }
}

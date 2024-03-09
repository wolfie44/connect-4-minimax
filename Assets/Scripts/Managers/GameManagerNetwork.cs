using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Unity.Netcode;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Collections;

// Handle Game Logic for a networked game
public class GameManagerNetwork : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _feedbackUI;
    [SerializeField] private TextMeshProUGUI _lobbyCode;

    [Header("VFX")]
    [SerializeField] private GameObject tokenExplosionPrefab;

    public NetworkVariable<int> CurrentRound = new NetworkVariable<int>(1);
    public NetworkVariable<int> NumberOfRoundsMax = new NetworkVariable<int>(3);

    // Winning Screen
    public NetworkVariable<int> WinnerScore = new NetworkVariable<int>(0);
    public NetworkVariable<int> LoserScore = new NetworkVariable<int>(0);
    public NetworkVariable<FixedString32Bytes> WinnerName = new NetworkVariable<FixedString32Bytes>();
    public NetworkVariable<FixedString32Bytes> LoserName = new NetworkVariable<FixedString32Bytes>();
    private NetworkList<Vector3> winningSequence;
    
    private PlayerEntity[] players;
    private int numberOfPlayer = 0;
    private bool endGame = false;

    #region ---- SINGLETON
    private static GameManagerNetwork instance;
    public static GameManagerNetwork Instance
    {
        get
        {
            if (instance != null) { return instance; }
            instance = FindObjectOfType<GameManagerNetwork>();
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    private void Awake()
    {
        winningSequence = new NetworkList<Vector3>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Server will handle the whole gameplay logic
            BoardManager.GetInstance().HasAWinner += Win;
            BoardManager.GetInstance().BoardIsFull += BoardFull;
            BoardManager.GetInstance().Init();

            PlayerEntity[] players = FindObjectsByType<PlayerEntity>(FindObjectsSortMode.None);
            foreach (PlayerEntity player in players)
            {
                HandlePlayerSpawned(player);
            }

            PlayerEntity.OnPlayerSpawned += HandlePlayerSpawned;
            PlayerEntity.OnPlayerDespawned += HandlePlayerDespawned;

            NumberOfRoundsMax.Value = PlayerPrefs.GetInt(Constants.NumberOfRoundsKey, 3);

            _lobbyCode.text = "LOBBY CODE :\n" +HostSingleton.Instance.GameManager.GetLobbyCode();
        }
        else 
        {
            // Join client doesnt need feedbacks
            _feedbackUI.gameObject.SetActive(false);
            _lobbyCode.gameObject.SetActive(false);
        }
    }

    public override void OnNetworkDespawn()
    {
        if(IsServer)
        {
            BoardManager.GetInstance().HasAWinner -= Win;
            BoardManager.GetInstance().BoardIsFull -= BoardFull;

            PlayerEntity.OnPlayerSpawned -= HandlePlayerSpawned;
            PlayerEntity.OnPlayerDespawned -= HandlePlayerDespawned;
        }       
    }

    private void HandlePlayerSpawned(PlayerEntity player)
    {
        numberOfPlayer++;
        CheckStartGame();
    }

    private void HandlePlayerDespawned(PlayerEntity player)
    {
        ClientSingleton.Instance.GameManager.GoToMenu();
    }

    private void CheckStartGame()
    {
        if(!IsServer) return;

        if(numberOfPlayer == 2)
        {
            CurrentRound.Value = 1;
            _feedbackUI.gameObject.SetActive(false);
            _lobbyCode.gameObject.SetActive(false);
            StartGame();
        }
    }

    #region ----- FLOW
    private void StartGame()
    {
        // Server gives to one random player the right to drop a token
        players = FindObjectsByType<PlayerEntity>(FindObjectsSortMode.None);
        players[UnityEngine.Random.Range(0,2)].ChangePlayingStatus();
    }

    private void NextRound()
    {
        BoardManager.GetInstance().CleanBoard();
        ResetBoardVisualClientRPC();
        players[UnityEngine.Random.Range(0,2)].ChangePlayingStatus();
        CurrentRound.Value++;
        endGame = false;
    }

    private void RestartRound()
    {
        BoardManager.GetInstance().CleanBoard();
        ResetBoardVisualClientRPC();
        players[UnityEngine.Random.Range(0,2)].ChangePlayingStatus();
        endGame = false;
    }

    // Server update the winner and loser data
    private void EndGame()
    {
        if(players[0].Score.Value > players[1].Score.Value)
        {
            WinnerName.Value = players[0].PlayerName.Value;
            WinnerScore.Value = players[0].Score.Value;
            LoserScore.Value = players[1].Score.Value;
            LoserName.Value = players[1].PlayerName.Value;       
        }
        else
        {
            WinnerName.Value = players[1].PlayerName.Value;
            WinnerScore.Value = players[1].Score.Value;
            LoserScore.Value = players[0].Score.Value;
            LoserName.Value = players[0].PlayerName.Value;          
        }
        EndGameClientRPC();
    }

    [ClientRpc]
    // Tell clients to delete all tokens inside the board
    private void ResetBoardVisualClientRPC()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    [ClientRpc]
    // Tell clients to disable player meshs
    private void EndGameClientRPC()
    {
        players[0].GetComponent<MeshRenderer>().enabled = false;
        players[1].GetComponent<MeshRenderer>().enabled = false;
    }

    #endregion


    #region ------ TOKENS MANAGEMENT 
    public void Hit(string name, GameObject currentToken)
    {
        if(!IsServer) return;

        int colum = Int32.Parse(name);
        Debug.Log("Token has been received in case "+colum);
        AddTokenToBoard(colum, currentToken);
    }

    void AddTokenToBoard(int column, GameObject currentToken)
    {   
        ulong ownerId = currentToken.GetComponent<TokenVisual>().OwnerId;
        Token token = new Token(currentToken, ownerId);
        bool tokenDropped = BoardManager.GetInstance().TryAddToken(token, column);
        if(!tokenDropped)
        {
            foreach(PlayerEntity player in players)
            {
                if(player.OwnerClientId == ownerId)
                {
                    player.DestroyUnvalidTokensClientRpc();
                }
            }
        }
        else
        {
            if(endGame) return;
            foreach(PlayerEntity player in players)
            {
                player.ChangePlayingStatus();
            }
        }
    }
    #endregion
    
    #region Events
    private void BoardFull()
    {
        endGame = true;
        Debug.Log("Server : Board is full");
        foreach (PlayerEntity player in players)
        {
            player.IsTurn.Value = false;
        }
        
        RestartRound();
    }

    private void Win(List<Token> winningLine)
    {
        endGame = true;
        Debug.Log("Server : Someone won");
        foreach (PlayerEntity player in players)
        {
            player.IsTurn.Value = false;
        }

        EndRound(winningLine);
    }
    #endregion


    #region -------- WIN / END
    private void EndRound(List<Token> winningLine)
    {
        winningSequence.Clear();
        foreach(Token t in winningLine)
        {
            winningSequence.Add(t.VisualToken.transform.position);
        }

        DisplayWinnerClientRPC();
        
        foreach(PlayerEntity player in players)
        {
            if(player.OwnerClientId == winningLine[0].IdOwner)
            {
                player.Score.Value++;
            }        
        }
       
        if (players[0].Score.Value > (int)NumberOfRoundsMax.Value/2 || players[1].Score.Value > (int)NumberOfRoundsMax.Value/2) 
        {
            Debug.Log("End Game");
            Invoke("EndGame", 5f);
        }
        else
        {
            Debug.Log("Continue...");
            Invoke("NextRound", 5f);
        }
    }

    [ClientRpc]
    // From Server to all clients
    private void DisplayWinnerClientRPC()
    {
        Debug.Log("End of Round");
        StartCoroutine(AnimEndGame());
    }

    private IEnumerator AnimEndGame()
    {
        yield return new WaitForSeconds(3f);
        SoundManager.GetInstance().Play(AudioSFX.ENDGAME);
        
        foreach(Vector3 token in winningSequence)
        {         
            Instantiate(tokenExplosionPrefab, token, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
        }
    }
    #endregion

}
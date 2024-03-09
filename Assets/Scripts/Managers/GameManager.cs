using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

// Handle Game Logic for a local game
public class GameManager : Singleton<GameManager>
{
    [Header("Gameplay")]
    [SerializeField] private int _MaxNumberOfPlayer = 2;
    [SerializeField] private Player[] _Players;
    [SerializeField] private GameObject tokenExplosionPrefab;
    [SerializeField] private Transform _resetSpawnPosition;

    [Header("UI")]
    [SerializeField] private PlayerColors _colors;
    [SerializeField] private HUDManager _HUDManager;


    private int _currentRound = 1;
    private int _numberOfRoundsMax;
    private ulong _currentPlayerIndex = 0;
    private bool _playerAgainstAI;
    private ComputerAI _computer;
    private bool alreadyProcessing = false;
    private bool endGame = false;


    void Start()
    {
        BoardManager.GetInstance().HasAWinner += Win;
        BoardManager.GetInstance().BoardIsFull += BoardFull;
        BoardManager.GetInstance().Init();

        _currentPlayerIndex = (ulong) UnityEngine.Random.Range(0,2);

        foreach(Player p in _Players)
        {
            p.Init();
        }

        _numberOfRoundsMax = PlayerPrefs.GetInt(Constants.NumberOfRoundsKey, 3);
        _HUDManager.InitUI(_Players[0].PlayerName, _Players[1].PlayerName, _numberOfRoundsMax);

        _playerAgainstAI = PlayerPrefs.GetInt(Constants.PlayAgainstIAKey, 0) == 0 ? false : true;
        if (_playerAgainstAI)
        {
            _computer = new ComputerAI();
        }
   
        ChangePlayer();
    }


    protected override void OnDestroy() 
    {
        base.OnDestroy();
        BoardManager.GetInstance().HasAWinner -= Win;
        BoardManager.GetInstance().BoardIsFull -= BoardFull;
    }
  

    #region -------- AI AGENT PLAY
    private void AgentPlay()
    {
        _computer.StartSearch();
    }

    public void AgentChoice(int column)
    {
        _Players[1].transform.position = SpawnPoint.GetSpawnPointAtPos(column);
        _Players[1].PlayerLocalMovement.OnDrop(true);
    }
    #endregion



    #region ------ TOKENS MANAGEMENT 
    public void Hit(string name)
    {
        if(alreadyProcessing || endGame ) return;
        alreadyProcessing = true;
        int colum = Int32.Parse(name);
        Debug.Log("Token has been received in case "+colum);
        AddTokenToBoard(colum);
    }

    void AddTokenToBoard(int column)
    {
        GameObject currentToken = _Players[_currentPlayerIndex].PlayerLocalMovement.CurrentToken;
        Token token = new Token(currentToken, _currentPlayerIndex);
        bool tokenDropped = BoardManager.GetInstance().TryAddToken(token, column);
        if(!tokenDropped)
        {
            Destroy(currentToken);
            _Players[_currentPlayerIndex].PlayerLocalMovement.MoveAllowed = true;
        }
        else
        {
            Invoke("ChangePlayer", 2f);
        }
    }
    #endregion
    
    #region Events
    public void Win(List<Token> winningLine)
    {
        endGame = true;
        EndRound(winningLine);
    }

    public void BoardFull()
    {
        endGame = true;
        Debug.Log("Board is full");
        Invoke("ClearInterface", 3f);
        Invoke("ChangePlayer", 5f);
    }
    #endregion


    #region ------ EACH TURN MECHANIC
    private void ChangePlayer()
    {
        if(endGame) return;
        alreadyProcessing = false;
        _currentPlayerIndex++;
        if (_currentPlayerIndex > 1) _currentPlayerIndex = 0;

        _Players[_currentPlayerIndex].SetVisible(true);

        if(_playerAgainstAI && _currentPlayerIndex == 1)
        {
            AgentPlay();
        }
        else
        {
            _Players[_currentPlayerIndex].PlayerLocalMovement.MoveAllowed = true; 
        }     
    }
    #endregion


    #region -------- WIN / END
    private void EndRound(List<Token> winningLine)
    {
        Debug.Log("End of Round");
        StartCoroutine(AnimEndGame(winningLine));

        _Players[(int) winningLine[0].IdOwner].Score++;

        if (_Players[0].Score > _numberOfRoundsMax / 2 || _Players[1].Score > _numberOfRoundsMax / 2)
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

    private void NextRound()
    {
        _currentRound++;
        ClearInterface();
        ChangePlayer();
    }

    private void EndGame()
    {
        if(_Players[0].Score > _Players[1].Score)
        {
            _HUDManager.DisplayVictory(_Players[0].PlayerName, _Players[1].PlayerName, _Players[0].Score, _Players[1].Score, _Players[0].PlayerName);
        }
        else
        {
            _HUDManager.DisplayVictory(_Players[0].PlayerName, _Players[1].PlayerName, _Players[0].Score, _Players[1].Score, _Players[1].PlayerName);
        }
    }

    private IEnumerator AnimEndGame(List<Token> winningLine)
    {
        yield return new WaitForSeconds(3f);
        SoundManager.GetInstance().Play(AudioSFX.ENDGAME);
        
        foreach(Token token in winningLine)
        {         
            GameObject obj = Instantiate(tokenExplosionPrefab, token.VisualToken.transform.position, Quaternion.identity);
            ParticleSystem component = obj.GetComponent<ParticleSystem>();

            if (component != null)
            {
                ParticleSystem.MainModule mainModule = component.main;
                mainModule.startColor = new ParticleSystem.MinMaxGradient(
                    _colors.Colors[winningLine[0].IdOwner], 
                    _colors.Colors[winningLine[0].IdOwner]);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void ClearInterface()
    {
        Debug.Log("Clearing Interface...");
        _HUDManager.UpdateUI(_Players[0].Score, _Players[1].Score, _currentRound, _numberOfRoundsMax);
        BoardManager.GetInstance().CleanBoard();
        endGame = false;
    }
    #endregion


    #region Local Inputs
    public void OnMove(InputAction.CallbackContext context)
    {
        _Players[_currentPlayerIndex].PlayerLocalMovement.OnMove(context.ReadValue<Vector2>());
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            SoundManager.GetInstance().Play(AudioSFX.DROP);
            _Players[_currentPlayerIndex].PlayerLocalMovement.OnDrop(false);   
        } 
    }
    #endregion

}
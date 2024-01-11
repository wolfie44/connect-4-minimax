using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    public bool TestMode = false;
    [SerializeField]
    private GameInfo _GameInfo;
    [SerializeField]
    private List<PlayerInfo> _PlayerInfo;
    [SerializeField]
    private GameObject tokenPrefabRose;
    [SerializeField]
    private GameObject tokenPrefabYellow;
    [SerializeField]
    private GameObject PrefabParticules;
    [SerializeField]
    private Transform tokenPosition;
    [SerializeField]
    private List<Transform> _IATransformPositions;
    [SerializeField]
    private UIManager _UIManager;


    private int _currentPlayerIndex;

    private GameObject _currentToken;
    private bool _playerAgainstAI = false;
    private ComputerAI _computer;
    private bool playerCanPlay;
    private bool alreadyProcessing = false;
    private bool endGame = false;

    public float fallSpeed = 20f;


    void Start()
    {
        if(TestMode) return;

        BoardManager.GetInstance().HasAWinner += Win;
        BoardManager.GetInstance().BoardIsFull += BoardFull;

        _currentPlayerIndex = 0;
        _GameInfo.CurrentRound = 1;
        _PlayerInfo[0].PlayerScore = 0;
        _PlayerInfo[1].PlayerScore = 0;
        _UIManager.InitUI();
        InstantiateToken();
       
         //IF AGAINST COMPUTER
        if (_GameInfo.NumberOfPlayer == 1)
        {
            _playerAgainstAI = true;
            _computer = new ComputerAI();
        }
    }


    protected override void OnDestroy() 
    {
        base.OnDestroy();
        BoardManager.GetInstance().HasAWinner -= Win;
        BoardManager.GetInstance().BoardIsFull -= BoardFull;
    }

    void Update()
    {
        if ((_playerAgainstAI && _currentPlayerIndex == 0 ) || !_playerAgainstAI)
        {
            if (Input.GetButtonDown("Submit") && playerCanPlay)
            {
                playerCanPlay = false;
                _currentToken.GetComponent<Rigidbody>().isKinematic = false;
                _currentToken.GetComponent<MoveToken>().enabled = false;    
                _currentToken.GetComponent<Rigidbody>().velocity = new Vector3(0f, -fallSpeed, 0f);
            }
        }
    }
  

    #region -------- AI AGENT PLAY
    private void AgentPlay()
    {
        _computer.StartSearch();
    }

    public void AgentChoice(int column)
    {
        _currentToken.transform.position = _IATransformPositions[column].position;
        _currentToken.GetComponent<Rigidbody>().isKinematic = false;
        _currentToken.GetComponent<MoveToken>().enabled = false;
        _currentToken.GetComponent<Rigidbody>().velocity = new Vector3(0f, -fallSpeed, 0f);
    }
    #endregion



    #region ------ TOKENS MANAGEMENT 
    public void Hit(string name)
    {
        if(alreadyProcessing || endGame ) return;
        alreadyProcessing = true;
        int colum = Int32.Parse(name);
        Debug.Log("Token has been received in case "+colum);
        DropToken(colum-1);
    }

    void DropToken(int column)
    {
        Token token = new Token(_currentToken, _currentPlayerIndex);
        bool tokenDropped = BoardManager.GetInstance().TryAddToken(token, column);
        if(!tokenDropped)
        {
            Destroy(_currentToken);
            Invoke("InstantiateToken",2f);
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

        InstantiateToken();       
    }


    private void InstantiateToken()
    {
        if(endGame) return;
        alreadyProcessing = false;

        if(_currentPlayerIndex == 0)
        {
             _currentToken = Instantiate(tokenPrefabRose, tokenPosition.position, Quaternion.identity);
        }
        else 
        {
            _currentToken = Instantiate(tokenPrefabYellow, tokenPosition.position, Quaternion.identity);
        }

        if (_playerAgainstAI && _currentPlayerIndex == 1)
        {
            _currentToken.GetComponent<MoveToken>().enabled = false;
            Invoke("AgentPlay",1f);
        }

        playerCanPlay = true;
    }
    #endregion



    #region -------- WIN / END
    private void EndRound(List<Token> winningLine)
    {
        Debug.Log("End of Round");
        StartCoroutine(AnimEndGame(winningLine));

        _PlayerInfo[_currentPlayerIndex].PlayerScore++;

        int winner = 0;

        if(_PlayerInfo[0].PlayerScore > _PlayerInfo[1].PlayerScore)
        {
            winner = 0;
        }
        else if(_PlayerInfo[1].PlayerScore > _PlayerInfo[0].PlayerScore)
        {
            winner = 1;
        }
        else
        {
            winner = 2; //Equality
        }

        if (winner != 2 && _PlayerInfo[winner].PlayerScore > _GameInfo.NumberOfRound/2) 
        {
            _UIManager.DisplayVictory(winner);
        }
        else
        {
            Debug.Log("Continue...");

            Invoke("ClearInterface", 7f);
            Invoke("ChangePlayer", 10f);
        }

        _GameInfo.CurrentRound++;
    }

    private IEnumerator AnimEndGame(List<Token> winningLine)
    {
        yield return new WaitForSeconds(3f);
        foreach(Token token in winningLine)
        {         
            GameObject obj = Instantiate(PrefabParticules, token.VisualToken.transform.position, Quaternion.identity);
            ParticleSystem component = obj.GetComponent<ParticleSystem>();

            if (component != null)
            {
                ParticleSystem.MainModule mainModule = component.main;
                mainModule.startColor = new ParticleSystem.MinMaxGradient(
                    _PlayerInfo[winningLine[0].IdOwner].PlayerColor, 
                    _PlayerInfo[winningLine[0].IdOwner].PlayerColor);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void ClearInterface()
    {
        Debug.Log("Clearing Interface...");
        _UIManager.UpdateUI();
        BoardManager.GetInstance().CleanBoard();
        endGame = false;
    }
    #endregion

}
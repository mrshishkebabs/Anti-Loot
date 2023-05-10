using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    PreGamePhase,
    TrapPhase,
    EscapePhase,
    EndScreenPhase,
    DebugPhase
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;   
    public GameObject player;
    public PlayerController test;
    public GameState state;
    public bool escapePhaseStarted = false;
    public bool trapPhase = false;

    [SerializeField] private Transform startPoint;


    public GameObject TrapperWinScreen;
    public GameObject EscapistWinScreen;
    public GameObject resetButton;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateGameState(GameState.PreGamePhase);
        player = PlayerManager.instance.player;
        FindObjectOfType<AudioManager>().Play("Menu");
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.PreGamePhase:
                HandlePreGamePhase();
                break;
            case GameState.TrapPhase:
                HandleTrapPhase();
                break;
            case GameState.EscapePhase:
                HandleEscapePhase();
                break;
            case GameState.EndScreenPhase:
                HandleEndPhase();
                break;
            case GameState.DebugPhase:
                break;
            default:
                break;
        }

    }
    private void HandlePreGamePhase()
    {
        //MainMenu

        //Ability Select
        
    }

    private void HandleTrapPhase()
    {
        EventBroker.CallTrapPhaseStart();
        FindObjectOfType<AudioManager>().Stop("Menu");
        FindObjectOfType<AudioManager>().Stop("InGame");
        FindObjectOfType<AudioManager>().Play("SetTraps");
        escapePhaseStarted = false;
        trapPhase = true;
    }

    private void HandleEscapePhase()
    {
        player.transform.position = startPoint.position;
        player.SetActive(true);
        EventBroker.CallEscapePhaseStart();
        escapePhaseStarted = true;
        trapPhase = false;

        FindObjectOfType<AudioManager>().Stop("SetTraps");
        FindObjectOfType<AudioManager>().Play("InGame");
    }

    private void HandleEndPhase()
    {
        
        FindObjectOfType<AudioManager>().Taunt();
        throw new NotImplementedException();
    }

    public void TrapperWin()
    {
        TrapperWinScreen.SetActive(true);
        resetButton.SetActive(true);
        FindObjectOfType<AudioManager>().Play("taunt3");
    }

    public void EscapistWin()
    {
        EscapistWinScreen.SetActive(true);
        resetButton.SetActive(true);
    }

    public void Reset()
    {
        TrapperWinScreen.SetActive(false);
        EscapistWinScreen.SetActive(false);
        resetButton.SetActive(false);
        UpdateGameState(GameState.TrapPhase);
    }
}



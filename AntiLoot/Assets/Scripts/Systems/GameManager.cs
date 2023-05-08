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
    public GameState state;
    public bool escapePhaseStarted = false;

    [SerializeField] private Transform startPoint;

 

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
    }

    private void HandleEscapePhase()
    {
        player.transform.position = startPoint.position;
        player.SetActive(true);
        EventBroker.CallEscapePhaseStart();
        escapePhaseStarted = true;
    }

    private void HandleEndPhase()
    {
        throw new NotImplementedException();
    }

}



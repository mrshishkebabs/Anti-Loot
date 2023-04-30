using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    PreGamePhase,
    TrapPhase,
    EscapePhase,
    EndScreenPhase
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState state;

    [SerializeField] GameObject player;
    [SerializeField] Transform startPoint;

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
        player.transform.position = startPoint.position;
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
            default:
                break;
        }

        EventBroker.CallGameStateChange(newState);
    }
    private void HandlePreGamePhase()
    {
        //MainMenu

        //Ability Select
    }

    private void HandleTrapPhase()
    {
        
    }

    private void HandleEscapePhase()
    {
        throw new NotImplementedException();
    }

    private void HandleEndPhase()
    {
        throw new NotImplementedException();
    }

}



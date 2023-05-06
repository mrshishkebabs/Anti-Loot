using System;
using UnityEngine;
using System.Collections;

public class EventBroker : MonoBehaviour
{
    /*Example Event
    public static event Action onResourceCollection; */

    //GameManager
    public static event Action<GameState> OnGameStateChange;
    public static event Action OnTrapPhaseStart;
    public static event Action OnEscapePhaseStart;

    /*Example Call
    public static void CallResourceCollection()
    {
        onResourceCollection?.Invoke();
    }*/

    public static void CallGameStateChange(GameState newState)
    {
        OnGameStateChange?.Invoke(newState);
    }

    public static void CallTrapPhaseStart()
    {
        OnTrapPhaseStart?.Invoke();
    }

    public static void CallEscapePhaseStart()
    {
        OnEscapePhaseStart?.Invoke();
    }
}

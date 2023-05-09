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
    public static event Action<string, int> OnCounterUpdate;
    public static event Action<string> OnTrapperAbilitySelect;

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

    public static void CallCounterUpdate(string name, int number)
    {
        OnCounterUpdate?.Invoke(name, number);
    }

    public static void CallTrapperAbilitySelect(string ability)
    {
        OnTrapperAbilitySelect?.Invoke(ability);
    }
}

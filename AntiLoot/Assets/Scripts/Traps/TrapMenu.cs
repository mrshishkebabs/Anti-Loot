using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMenu : MonoBehaviour
{

    public void StartEscapePhase()
    {
        GameManager.instance.UpdateGameState(GameState.EscapePhase);
    }
}

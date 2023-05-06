using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        GameManager.instance.UpdateGameState(GameState.TrapPhase);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

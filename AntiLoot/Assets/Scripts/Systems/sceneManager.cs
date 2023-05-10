using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    /*
     The purpose of this script is to manage the various screens in the game using information of the game's state
    from the PlayerController script
    If the player runs out of lives(gets hit 3 times), this script will trigger the defeat screen
    If the player reaches the goal, this script will trigger the win screen
    Both screens will have a button that says "Play Again?. Pushing the button will restart the game, using the Reset() funciton.
     */
    
    public static sceneManager instance;

    private GameObject player;
    public GameObject TrapperWinScreen;
    public GameObject EscapistWinScreen;
    public GameObject resetButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(player.hitsTillDead);
        if(player.GetComponent<PlayerController>().hitsTillDead == 0)
        {
            TrapperWinScreen.SetActive(true);
            resetButton.SetActive(true);
            FindObjectOfType<AudioManager>().Play("taunt3");
        }

        if (player.GetComponent<PlayerController>().reachedGoal == true)
        {
            EscapistWinScreen.SetActive(true);
            resetButton.SetActive(true);
        }

    }

    public void Reset()
    {
        TrapperWinScreen.SetActive(false);
        EscapistWinScreen.SetActive(false);
        resetButton.SetActive(false);
        GameManager.instance.UpdateGameState(GameState.TrapPhase);
    }
}

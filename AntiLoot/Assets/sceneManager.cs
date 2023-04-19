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
    
    public PlayerController player;
    public GameObject defeatScreen;
    public GameObject winScreen;
    public GameObject resetButton;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(player.hitsTillDead);
        if(player.hitsTillDead == 0)
        {
            defeatScreen.SetActive(true);
            resetButton.SetActive(true);
        }

        if (player.reachedGoal == true)
        {
            winScreen.SetActive(true);
            resetButton.SetActive(true);
        }

    }

    public void Reset()
    {
        defeatScreen.SetActive(false);
        winScreen.SetActive(false);
        resetButton.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

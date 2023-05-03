using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject lastLifeText;

    public GameObject player;

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
        player = Instantiate(playerPrefab);
        player.GetComponent<PlayerController>().lastLifeText = lastLifeText;
        player.SetActive(false);
    }
}

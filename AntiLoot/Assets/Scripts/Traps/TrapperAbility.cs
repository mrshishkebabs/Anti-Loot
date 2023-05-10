using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrapperAbility : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TrapperAbilityName;
    public bool accelerate, dizzy, desperation;

    private void OnEnable()
    {
        TrapperAbilityName = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        EventBroker.OnTrapperAbilitySelect += TrapperAbilityThisGame;
    }

    private void OnDisable()
    {
        EventBroker.OnTrapperAbilitySelect -= TrapperAbilityThisGame;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TrapperAbilityThisGame(string ability)
    {
        TrapperAbilityName.text = ability;
        switch (ability) {
            case TrapperAbilities.ACCELERATE:
                TrapperAbilityName.text = TrapperAbilities.ACCELERATE;
                accelerate = true;
                dizzy = false;
                desperation = false;
                break;
            case TrapperAbilities.DIZZY:
                TrapperAbilityName.text = TrapperAbilities.DIZZY;
                accelerate = false;
                dizzy = true;
                desperation = false;
                break;
            case TrapperAbilities.DESPERATION:
                TrapperAbilityName.text = TrapperAbilities.DESPERATION;
                accelerate = false;
                dizzy = false;
                desperation = true;
                break;
        }
    }

    public void UseTrapperAbility()
    {
        if (accelerate)
        {

        }
        else if (dizzy)
        {

        }
        else if (desperation)
        {

        }
    }
}

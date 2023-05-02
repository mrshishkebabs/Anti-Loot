using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Abilities
{
    Jam,
    Shield,
    Pulse,
    Accelerate,
    Dizzy,
    Desperation
}

public class AbilitySelectScreen : MonoBehaviour
{
    [SerializeField] private bool[] escapistAbility = new bool[3];
    [SerializeField] private bool[] trapperAbility = new bool[3];

    [SerializeField] private bool escReady,trapReady;
    [SerializeField] private GameObject startButton;
    [SerializeField] private TextMeshProUGUI EscText, TrapText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(escReady && trapReady)
        {
            startButton.SetActive(true);
        }
    }

    public void EscAbility(int number)
    {
        for(int i=0; i<3; i++)
        {
            if(i == number)
            {
                escapistAbility[i] = true;
                escReady = true;
            }
            else
            {
                escapistAbility[i] = false;
            }

            if (number == 0)
            {
                ChangeAbilityText(Abilities.Jam);
            }
            else if (number == 1)
            {
                ChangeAbilityText(Abilities.Shield);
            }
            else if (number == 2)
            {
                ChangeAbilityText(Abilities.Pulse);
            }
        }

    }
    
    public void TrapAbility(int number)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == number)
            {
                trapperAbility[i] = true;
                trapReady = true;
            }
            else
            {
                trapperAbility[i] = false;
            }
            if(number == 0)
            {
                ChangeAbilityText(Abilities.Accelerate);
            }
            else if(number == 1)
            {
                ChangeAbilityText(Abilities.Dizzy);
            }
            else if(number == 2)
            {
                ChangeAbilityText(Abilities.Desperation);
            }
        }
    }

    public void ChangeAbilityText(Abilities name)
    {
        switch (name)
        {
            case Abilities.Jam:
                EscText.text = "Choose a trap and disable it for a ~set amount of time";
                break;
            case Abilities.Shield:
                EscText.text = "Shield yourself for a ~set amount of time, becoming invulnerable to any hits";
                break;
            case Abilities.Pulse:
                EscText.text = "Release a wave ~pushing all traps away slightly";
                break;
            case Abilities.Accelerate:
                TrapText.text = "All Traps move faster for a ~set amount of time ";
                break;
            case Abilities.Dizzy:
                TrapText.text = "Flip the screen for a ~set amount of time";
                break;
            case Abilities.Desperation:
                TrapText.text = "Pick up ~1 trap and reposition it";
                break;
        }
    }
}
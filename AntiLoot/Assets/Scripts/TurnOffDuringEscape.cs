using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnOffDuringEscape : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TrapperText;
    [SerializeField] Button TrapperButton;
    [SerializeField] Image Trapperimage;

    private void OnEnable()
    {
        EventBroker.OnTrapPhaseStart += TurnOff;
    }

    private void OnDisable()
    {
        EventBroker.OnTrapPhaseStart -= TurnOff;
    }
    private void TurnOff()
    {
        TrapperText.enabled = false;
        TrapperButton.enabled = false;
        Trapperimage.enabled = false;
    }
}

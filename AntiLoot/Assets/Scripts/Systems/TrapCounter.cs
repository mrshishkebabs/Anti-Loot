using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrapCounter : MonoBehaviour
{
    [SerializeField] private string trapName;
    [SerializeField] private int counter = 0;
    [SerializeField] private int oriNum = 4;
    [SerializeField] TextMeshProUGUI counterText;

    private void OnEnable()
    {
        EventBroker.OnCounterUpdate += UpdateCount;
        EventBroker.OnTrapPhaseStart += ResetCount;
    }

    private void OnDisable()
    {
        EventBroker.OnCounterUpdate -= UpdateCount;
        EventBroker.OnTrapPhaseStart -= ResetCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateCount(string name, int number)
    {
        Debug.Log(number);
        if(trapName == name && number >= counter)
        {
            counterText.text = name + "\n" + (number - counter);
            counter++;
        }
        else if(number < counter)
        {
            Debug.Log("all gone");
        }

    }

    private void ResetCount()
    {
        counter = 0;
        counterText.text = trapName + "\n" + oriNum;
    }
}

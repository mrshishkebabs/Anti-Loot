using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTraps : MonoBehaviour
{
    [SerializeField] private List<GameObject> ResetTrapPhase = new List<GameObject>();

    private void OnEnable()
    {
        EventBroker.OnTrapPhaseStart += DeleteTraps;
    }

    private void OnDisable()
    {
        EventBroker.OnTrapPhaseStart -= DeleteTraps;
    }

    public void DeleteTraps()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        foreach(GameObject obj in ResetTrapPhase)
        {
            obj.SetActive(true);
        }
    }
}

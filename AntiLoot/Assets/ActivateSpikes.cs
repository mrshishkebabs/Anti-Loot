using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSpikes : MonoBehaviour
{
    public void ActivateTrap()
    {
        gameObject.tag = Tags.TRAP;
    }

    public void DeactivateTrap()
    {
        gameObject.tag = Tags.UNTAGGED;
    }
}

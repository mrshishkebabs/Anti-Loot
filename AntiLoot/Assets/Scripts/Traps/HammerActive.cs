using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerActive : MonoBehaviour
{
    [SerializeField] private Collider2D hammerCol;

    private void HammerDown()
    {
        hammerCol.enabled = true;
    }

    private void HammerUp()
    {
        hammerCol.enabled = false;
    }

}

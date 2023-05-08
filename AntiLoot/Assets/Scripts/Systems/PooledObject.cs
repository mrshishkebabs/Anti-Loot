using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PooledObject
{
    public List<GameObject> pooledTraps = new List<GameObject>();

    public string TrapName;
    public int amountToPool;
    public int numberAvailable;
    public GameObject objPrefab;
    public GameObject prefabHolder;
}

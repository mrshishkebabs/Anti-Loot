using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] private List<PooledObject> traps = new List<PooledObject>();
    private List<GameObject> pooledObjects = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        foreach(PooledObject pooledTrap in traps)
        {
            for(int i = 0; i < pooledTrap.amountToPool; i++)
            {
                GameObject obj = Instantiate(pooledTrap.objPrefab, pooledTrap.prefabHolder.transform);
                obj.SetActive(false);
                pooledTrap.pooledTraps.Add(obj);
            }
        }
    }

    /*[SerializeField] private int amountToPool = 10;
    [SerializeField] private GameObject objPrefab;
    [SerializeField] private GameObject prefabHolder;

    private void Awake()
    {
        if( instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i< amountToPool; i++)
        {
            GameObject obj = Instantiate(objPrefab,prefabHolder.transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);

        }
    }*/

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }

    public GameObject GetPooledObject(string specificTrap)
    {
        foreach(PooledObject pooledTrap in traps)
        {    
            if(pooledTrap.TrapName == specificTrap)
            {   
                for(int i = 0; i < pooledTrap.pooledTraps.Count; i++)
                {
                    if (!pooledTrap.pooledTraps[i].activeInHierarchy)
                    {
                        EventBroker.CallCounterUpdate(pooledTrap.TrapName, pooledTrap.pooledTraps.Count);
                        return pooledTrap.pooledTraps[i];
                    }
                }
            }
        }
        return null;
    }

    public void UpdateCounter()
    {
        foreach (PooledObject pooledTrap in traps)
        {
            pooledTrap.numberAvailable = 0;
            for(int i = 0; i < pooledTrap.pooledTraps.Count; i++)
            {
                if (!pooledTrap.pooledTraps[i].activeInHierarchy)
                {
                    pooledTrap.numberAvailable++;
                }
            }
            EventBroker.CallCounterUpdate(pooledTrap.TrapName, pooledTrap.numberAvailable);
        }
    }

    public void UpdateCounter(int difference)
    {
        foreach (PooledObject pooledTrap in traps)
        {
            pooledTrap.numberAvailable = 0;
            for (int i = 0; i < pooledTrap.pooledTraps.Count; i++)
            {
                if (!pooledTrap.pooledTraps[i].activeInHierarchy)
                {
                    pooledTrap.numberAvailable++;
                }
            }
            EventBroker.CallCounterUpdate(pooledTrap.TrapName, pooledTrap.numberAvailable - difference);
        }
    }

    public void ResetTraps()
    {
        foreach (PooledObject pooledTrap in traps)
        {
            for (int i = 0; i < pooledTrap.pooledTraps.Count; i++)
            {
                if (!pooledTrap.pooledTraps[i].activeInHierarchy)
                {
                    EventBroker.CallCounterUpdate(pooledTrap.TrapName, pooledTrap.pooledTraps.Count);
                }
            }
        }
    }
}

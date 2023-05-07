using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    private List<GameObject> pooledObjects = new List<GameObject>();
    
    [SerializeField] private int amountToPool = 10;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletHolder;

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
            GameObject obj = Instantiate(bulletPrefab,bulletHolder.transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);

        }
    }

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
}

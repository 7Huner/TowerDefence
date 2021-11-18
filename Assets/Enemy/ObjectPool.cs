using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: change pool Array to a list so you can add to pool

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject objectToPool;
 	[SerializeField] List<GameObject> pooledObjects;
    [SerializeField] int amountToPool = 5;
	[SerializeField] [Range(0.1f, 30f)] float spawnTimer = 1f;



    void Awake()
    {
        PopulatePool();
    }

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    public void AddToPool()
	{
        GameObject tmp = Instantiate(objectToPool, transform);
        tmp.SetActive(false);
        pooledObjects.Add(tmp);
        amountToPool++;
	}

    void PopulatePool() // populate empty game object with prefab enemies and disables them
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool, transform);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    // TODO: call a public method after a fixed time or amount of enemies killed 
    // that will add a new enemy to the pool

    void EnableObjectInPool()
    {
        for(int i = 0; i < amountToPool; i++)
		{
			if (pooledObjects[i].activeInHierarchy == false)
			{
                pooledObjects[i].SetActive(true);
                return;
			}
		}
    }

    IEnumerator SpawnEnemy()
    {
        while(true)
        {
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}

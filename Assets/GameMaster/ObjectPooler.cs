using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameMaster
{
  public class ObjectPooler : MonoBehaviour
  {
    public static ObjectPooler Instance;
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDicionary;

    private void Awake()
    {
      Instance = this;
    }

    private void Start()
    {
      poolDicionary = new Dictionary<string, Queue<GameObject>>();

      foreach (var pool in pools)
      {
        var objectPool = new Queue<GameObject>();

        for (var i = 0; i < pool.size; i++)
        {
          var obj = Instantiate(pool.prefab);

          obj.SetActive(false);

          objectPool.Enqueue(obj);
        }

        poolDicionary.Add(pool.tag, objectPool);
      }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
      if (!poolDicionary.ContainsKey(tag))
      {
        Debug.LogError($"{tag} doesn't exists");
        return null;
      }

      var objectToSpawn = poolDicionary[tag].Dequeue();

      objectToSpawn.SetActive(true);

      objectToSpawn.transform.position = position;
      objectToSpawn.transform.rotation = rotation;

      var pooledObj = objectToSpawn.GetComponent<IPooledObject>();
      pooledObj?.OnObjectSpawn();

      poolDicionary[tag].Enqueue(objectToSpawn);

      return objectToSpawn;
    }

    public void DeSpawnFromPoll(string tag, GameObject obj)
    {
      if (!poolDicionary.ContainsKey(tag))
      {
        Debug.LogError($"{tag} doesn't exists");
        return;
      }
      obj.SetActive(false);
      poolDicionary[tag].Enqueue(obj);
    }

    [Serializable]
    public class Pool
    {
      public GameObject prefab;
      public int size;
      public string tag;
    }
  }
}
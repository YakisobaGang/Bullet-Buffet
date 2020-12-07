using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enemys;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMaster.WaveSystem
{
  public class EnemySpawn : MonoBehaviour
  {
    [SerializeField] private List<GameObject> enemyToBeSpawner = new List<GameObject>(3);
    public Action SpawenerHasNoChild;
    private GameObject _enemyInstance;
    private Transform _transform;

    private void Awake()
    {
      _transform = GetComponent<Transform>();
    }
    private void Update()
    {
      EnemyDead();
    }

    private void EnemyDead()
    {
      if (transform.childCount <= 0)
      {
        SpawenerHasNoChild?.Invoke();
      }
    }

    public void Spawn()
    {
      _enemyInstance = Instantiate(GetRandomEnemy(), transform);
    }
    
    private GameObject GetRandomEnemy()
    {
      return enemyToBeSpawner[Random.Range(0, enemyToBeSpawner.Capacity - 1)];
    }
  }
}
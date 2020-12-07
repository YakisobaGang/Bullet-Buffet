using System;
using System.Collections;
using System.Collections.Generic;
using Enemys;
using UnityEngine;

namespace GameMaster.WaveSystem
{
  public class BattleSystem : MonoBehaviour
  {
    [SerializeField] private List<EnemySpawn> enemiesSpawners;
    public Action<int> Countdown;

    [Tooltip("Time in seconds"),SerializeField]
    private int timeBetweenWaves = 4;

    private EnemyManager _enemyManager;
    private State _state;
    private List<int> currentEnemiesOnScene = new List<int>();
    private int _emptySpawnersCount;
    private void Awake()
    {
      _state = State.Idle;
      _enemyManager = GetComponent<EnemyManager>();
    }

    private void Start()
    {
      ChangeState();
      _enemyManager.AllEnemysAreDead += delegate(bool b)
      {
        if(!b) return;
        StartCoroutine("ChangeStateAfterSeconds", timeBetweenWaves);
      };
      enemiesSpawners.ForEach(spawn =>
      {
        spawn.SpawenerHasNoChild += () => _emptySpawnersCount++;
      });
    }

    private void Update()
    {
      if (_enemyManager.AbleToSpawn() && _state is State.Active)
      {
        foreach (var enemy in enemiesSpawners)
        {
          enemy.Spawn();
          _enemyManager.AddEnemy(enemy.GetInstanceID());
          currentEnemiesOnScene.Add(enemy.GetInstanceID());
        }
        ChangeState();
      }
    }

    private void LateUpdate()
    {
      if (_emptySpawnersCount >= enemiesSpawners.Count)
      {
        for (var i = 0; i < currentEnemiesOnScene.Count; i++)
        {
          currentEnemiesOnScene.Remove(currentEnemiesOnScene[i]);
        }
      }
    }

    [ContextMenu("Change state")]
    private void ChangeState()
    {
      _state = _state is State.Idle ? State.Active : State.Idle;
    }

    [ContextMenu("Get all spawners")]
    private void GetAllSpawners()
    {
      var temp = FindObjectsOfType<EnemySpawn>();

      foreach (var enemy in temp) enemiesSpawners.Add(enemy);
    }

    [ContextMenu("Damage all enemy's")]
    private void KillAllEnemys()
    {
      for (var i = 0; i < enemiesSpawners.Count; i++)
        enemiesSpawners[i].GetComponentInChildren<EnemyBase>().TakeDamage(100);
    }

    private IEnumerator ChangeStateAfterSeconds(int seconds)
    {
      Countdown?.Invoke(seconds);
      yield return new WaitForSeconds(seconds);
      ChangeState();
    }

    private enum State
    {
      Idle,
      Active
    }
  }
}
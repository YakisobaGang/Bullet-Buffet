using System;
using System.Collections.Generic;
using Ludiq.PeekCore;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameMaster.WaveSystem
{
  public class EnemyManager : MonoBehaviour
  {
    public event Action AllEnemysAreDead;
    [SerializeField,LabelWidth(150)] private int maxEnemysOnScene = 15;
    [SerializeField] private List<GameObject> ListOfEnemys = new List<GameObject>();
    
    public bool AbleToSpawn() => ListOfEnemys.Count < maxEnemysOnScene && ListOfEnemys.Count == 0;

    private void Start()
    {
      EnemySpawn.EnemySpaned += enemy => ListOfEnemys.Add(enemy);
    }

    private void Update()
    {
      if (ListOfEnemys.TrueForAll(enemy => enemy.IsDestroyed()))
      {
        ListOfEnemys.Clear();
        AllEnemysAreDead?.Invoke();
      }
    }
  }
}
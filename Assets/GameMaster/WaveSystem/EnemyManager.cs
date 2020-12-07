using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameMaster.WaveSystem
{
  public class EnemyManager : MonoBehaviour
  {
    public Action<bool> AllEnemysAreDead;
    [SerializeField,LabelWidth(150)] private int maxEnemysOnScene = 10;
    private List<int> ListOfEnemys = new List<int>();
    
    public void RemoveEnemy(int id) => ListOfEnemys.Remove(id);
    public bool AbleToSpawn() => ListOfEnemys.Count < maxEnemysOnScene;
    public void AddEnemy(int id)
    {
      if(ListOfEnemys.Count != maxEnemysOnScene)
      {
        ListOfEnemys.Add(id);
      }
    }

    private void Update()
    {
      AllEnemysAreDead?.Invoke(ListOfEnemys.Count == 0);
    }
  }
}
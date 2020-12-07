using Pathfinding;
using UnityEngine;

namespace Enemys.Attack_Behaviour
{
  [RequireComponent(typeof(EnemyBase),typeof(Seeker),typeof(AIPath))]
  [RequireComponent(typeof(AIDestinationSetter))]
  public class Suicidal : MonoBehaviour
  {
    public EnemyBase myData;
    
    private void Awake()
    {
      myData = GetComponent<EnemyBase>();
    }
  }
}
using Pathfinding;
using UnityEngine;

namespace Enemys.Attack_Behaviour
{
  [RequireComponent(typeof(EnemyBase), typeof(Seeker), typeof(AIPath))]
  [RequireComponent(typeof(AIDestinationSetter))]
  public class Melle : MonoBehaviour
  {
    private EnemyBase _myData;
    private float _timeBetweenAttacks;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float startTimeBetweenAttacks;
    [SerializeField] private float damageAreaRadius;
    [SerializeField] private LayerMask playerLayer;

    private void Awake()
    {
      _myData = GetComponent<EnemyBase>();
      GetComponent<AIDestinationSetter>().target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
      GiveDamageToPlayer();
    }

    private void GiveDamageToPlayer()
    {
      if (_timeBetweenAttacks <= 0)
      {
        var playersToReceiveDamage = Physics2D.OverlapCircleAll(attackPoint.position, damageAreaRadius, playerLayer);
        if (playersToReceiveDamage.Length <= 0) return;
        foreach (var player in playersToReceiveDamage)
        {
          player.GetComponent<IDamageable>().TakeDamage();
        }

        _timeBetweenAttacks = startTimeBetweenAttacks;
      }
      else
      {
        _timeBetweenAttacks -= Time.deltaTime;
      }
    }

    private void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(attackPoint.position, damageAreaRadius);
    }
  }
}
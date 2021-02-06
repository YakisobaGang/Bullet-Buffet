using System;
using GameMaster;
using UnityEngine;

namespace YakisobaGang.Scripts
{

    public class Bullet : MonoBehaviour
    {
        [HideInInspector] public int damage = 1;
        [SerializeField] private float timeForDespawn = 3f;
        private readonly ObjectPooler _objectPooler  = ObjectPooler.Instance;
        private bool _forPlayer;
        private string _bulletPool;

        void Awake()
        {
            _forPlayer = gameObject.name == "bala Player" ? true : false;
           _bulletPool = _forPlayer ? "PlayerBullet" : "EnemyBullet";
        }

        private void LateUpdate() => 
            Invoke(nameof(DeSpawnBullet), timeForDespawn);
        
        
        private void DeSpawnBullet() => _objectPooler.DeSpawnFromPoll(_bulletPool, gameObject);

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Wall")
            {
                _objectPooler.DeSpawnFromPoll(_bulletPool, gameObject);
            }
            else if (col.CompareTag("Enemy"))
            {
                col.GetComponent<IDamageable>().TakeDamage(damage);
                _objectPooler.DeSpawnFromPoll(_bulletPool, gameObject);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using GameMaster;
using UnityEngine;

namespace Scripts
{

    public class Bullet : MonoBehaviour
    {
        public AudioSource audioTiro;
        private Vector3 shootDir;
        private readonly ObjectPooler _objectPooler  = ObjectPooler.Instance;

        void Awake()
        {
            audioTiro = GetComponentInChildren<AudioSource>();
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Wall")
            {
                _objectPooler.DeSpawnFromPoll("Bullet", gameObject);
            }
            else if (col.CompareTag("Enemy"))
            {
                audioTiro.Play();
                col.GetComponent<IDamageable>().TakeDamage();
                _objectPooler.DeSpawnFromPoll("Bullet", gameObject);
            }
        }
    }
}

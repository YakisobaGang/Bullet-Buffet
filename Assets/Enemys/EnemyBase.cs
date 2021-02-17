using System;
using System.Collections;
using GameMaster;
using UnityEngine;

namespace Enemys
{
    public class EnemyBase : MonoBehaviour, IDamageable
    {
        [SerializeField] private int health = 3;
        public FoodType foodType;
        public Transform MyTransform { get; private set; }
        public Rigidbody2D Rb { get; private set; }
        public AudioSource damageTickSFX;
        public event EventHandler OnEnemyDeath;
        private SpriteRenderer _renderer;

        private void Awake()
        {
            MyTransform = GetComponent<Transform>();
            _renderer = GetComponentInChildren<SpriteRenderer>();
            Rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (health <= 0)
            {
                damageTickSFX.Play();
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            OnEnemyDeath?.Invoke(this, EventArgs.Empty);
        }
        
        public void TakeDamage(int damage = 1)
        {
            print(gameObject.name);
            health -= damage;
            damageTickSFX.Play();
            StartCoroutine(nameof(ChangeColorCoroutine), 0.1f);
        }

         IEnumerator ChangeColorCoroutine(float timeInSeconds)
        {
            var waitForSeconds = new WaitForSeconds(timeInSeconds);
            
            var defaultColor = new Color(255,255,255,255);
            var hitColor = new Color(255,0,0,50);
            
            yield return waitForSeconds;
            _renderer.color = hitColor;
            yield return waitForSeconds;
            _renderer.color = defaultColor;
            yield return waitForSeconds;
            _renderer.color = hitColor;
            yield return waitForSeconds;
            _renderer.color = defaultColor;
        }
    }
}
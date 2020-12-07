using System;
using System.Collections;
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

        private void OnDestroy()
        {
            OnEnemyDeath?.Invoke(this, EventArgs.Empty);
        }

        public void TakeDamage()
        {
            if (health <= 0)
            {
                damageTickSFX.Play();
                ScoreAndCashSystem.Score += 100;
                ScoreAndCashSystem.Cash += 50;
                Destroy(gameObject);
            }
            health -= 1;
            StartCoroutine("ChangeColorCoroutine", 0.1f);
            damageTickSFX.Play();
        } 
        
        public void TakeDamage(int damage)
        {
            if (health <= 0)
            {
                damageTickSFX.Play();
                ScoreAndCashSystem.Score += 100;
                ScoreAndCashSystem.Cash += 50;
                Destroy(gameObject);
            }
            health -= damage;
            damageTickSFX.Play();
            StartCoroutine("ChangeColorCoroutine", 0.1f);
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
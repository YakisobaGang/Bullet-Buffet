using UnityEngine;

namespace Explosive.Script
{
  public class Mine : MonoBehaviour
  {
    [SerializeField] private int damage = 8;
    [SerializeField] private float damageArea = 4f;
    [SerializeField] private LayerMask enemysLayer;
    [SerializeField] private GameObject explosionFX;
    [SerializeField] private bool activateWithPlayer = false;
    private Animator _anim;

    private void Awake()
    {
      _anim = GetComponent<Animator>();
    }

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.magenta;
      Gizmos.DrawWireSphere(transform.position, damageArea);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.CompareTag(activateWithPlayer ? "Player" : "Enemy" )) _anim.Play("detonationAnim");
    }

    public void Explosion()
    {
      var position = transform.position;
      var enemys = Physics2D.OverlapCircleAll(position, damageArea, enemysLayer);
      
      if (enemys.Length <= 0) return;
      explosionFX.SetActive(true);
      explosionFX.GetComponent<ParticleSystem>().Play();
      
      for (var i = 0; i < enemys.Length; i++) enemys[i].GetComponent<IDamageable>().TakeDamage(damage);
      Destroy(gameObject,1.2f);
    }
  }
}
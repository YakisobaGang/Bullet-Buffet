using GameMaster;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YakisobaGang.Scripts.Data
{
  [CreateAssetMenu(fileName = "gunData", menuName = "ScriptableObject/CreateGun")]
  public sealed class GunData : ScriptableObject
  {
    [SerializeField] private string gunName;

    [SerializeField] [PreviewField(ObjectFieldAlignment.Left, Height = 130)]
    private Sprite gunSprite;


    [HorizontalGroup("Split", 0.5f, LabelWidth = 170)]
    [SerializeField]
    [BoxGroup("Split/Ammunition")]
    [LabelWidth(130)]
    [PropertyRange(0, "maxMagazineSize")]
    private int currentAmmunition;

    [SerializeField] [BoxGroup("Split/Ammunition")] [LabelWidth(130)]
    private int maxMagazineSize;

    [SerializeField] [BoxGroup("Split/Ammunition")] [LabelWidth(55)]
    private GameObject bullet;


    [SerializeField] [BoxGroup("Split/Shots")]
    private float bulletSpeed = 20f;

    [SerializeField] [BoxGroup("Split/Shots")] [Tooltip("Lower is faster")]
    private float fireRate = 1f;

    public string GunName => gunName;
    public Sprite GunSprite => gunSprite;

    public int MaxMagazineSize
    {
      get => maxMagazineSize;
      set => maxMagazineSize = value;
    }

    public int Ammunition
    {
      get => currentAmmunition;
      set => currentAmmunition = Mathf.Clamp(value, 0, maxMagazineSize);
    }

    public GameObject Bullet
    {
      get => bullet;
      private set => bullet = value;
    }

    public float BulletSpeed
    {
      get => bulletSpeed;
      set => bulletSpeed = value;
    }

    public float FireRate
    {
      get => fireRate;
      set => fireRate = value;
    }

    public void Shot(Transform firePoint)
    {
      var fireBullet = ObjectPooler.Instance.SpawnFromPool("Bullet", firePoint.position, firePoint.rotation);

      fireBullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * BulletSpeed, ForceMode2D.Impulse);
    }

    public void Shot(Transform[] firePoint)
    {
      var objectPooler = ObjectPooler.Instance;
      
      for (var i = 0; i < firePoint.Length; i++)
      {
        var fireBullet =
          objectPooler.SpawnFromPool("Bullet", firePoint[i].position, firePoint[i].localRotation);

        fireBullet.GetComponent<Rigidbody2D>().AddForce(firePoint[i].up * BulletSpeed, ForceMode2D.Impulse);
      }
    }
  }
}
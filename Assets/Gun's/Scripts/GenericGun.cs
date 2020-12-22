using System.Collections;
using GameMaster;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace YakisobaGang.Scripts
{
  [System.Serializable]
  public class GunInfo
  {
    ObjectPooler objectPooler = ObjectPooler.Instance;
    
    [SerializeField] private string gunName;

    [SerializeField] [PreviewField(ObjectFieldAlignment.Left, Height = 130)]
    private Sprite gunSprite;

    [HorizontalGroup("Split", 0.5f, LabelWidth = 170)]
    [SerializeField, BoxGroup("Split/Ammunition"),LabelWidth(130),PropertyRange(0, "maxMagazineSize")]
    private int currentAmmunition;
    [SerializeField, BoxGroup("Split/Ammunition"), LabelWidth(130)]
    private int maxMagazineSize;
    
    [SerializeField, BoxGroup("Split/Shots"), LabelWidth(130)]
    private float bulletSpeed = 20f;

    [SerializeField, BoxGroup("Split/Shots"),LabelWidth(130), Tooltip("Lower is faster")]
    private float fireRate;

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
      currentAmmunition -= 1;
      
      var fireBullet = objectPooler.SpawnFromPool("Bullet", firePoint.position, firePoint.rotation);
      fireBullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * BulletSpeed, ForceMode2D.Impulse);
    }

    public void Shot(Transform[] firePoint)
    {
      currentAmmunition -= 1;
      
      for (var i = 0; i < firePoint.Length; i++)
      {
        var fireBullet =
          objectPooler.SpawnFromPool("Bullet", firePoint[i].position, firePoint[i].localRotation);

        fireBullet.GetComponent<Rigidbody2D>().AddForce(firePoint[i].up * BulletSpeed, ForceMode2D.Impulse);
      }
    }
  }
  public class GenericGun : MonoBehaviour
  {
    #region Field's
    
    public GunInfo gunInfo;

    [SerializeField] [PropertyOrder(-1)] public Transform[] firePoint;
    
    [Header("Camera Shake Controls")]
    [SerializeField] private float cameraShakeIntensity = 0.6f;
    [SerializeField] private float cameraShakeDuration = 0.4f;
    
    [Header("Misc")]
    [SerializeField] private bool disableReload = false;
    [SerializeField] private bool disableCameraShake;
    
    [Header("Shot sound")]
    [SerializeField] private AudioSource shotSFX;
    
    private ICanShot _canShot;
    private Transform _gunTransform;
    private SpriteRenderer _renderer;
    private float defaultFireRate = 1f;
    private (bool hasLigt, Light2D light) _muzzleFlash;
    private readonly VCamera _vCamera = VCamera.Instance;

    #endregion
    
    public int CurrentAmmunition { get; private set; }

    private void Awake()
    {
      (_canShot, _renderer,_gunTransform) = 
        (GetComponentInParent<ICanShot>(), GetComponent<SpriteRenderer>(), GetComponent<Transform>());
      
      _renderer.sprite = gunInfo.GunSprite;
      gunInfo.FireRate = defaultFireRate;
      CurrentAmmunition = gunInfo.Ammunition;
    }

    private void Start() =>  
      _canShot.onShot += (sender, args) => gunInfo.Shot(firePoint);

    public void EditFireRate(float fireRate)
    {
      gunInfo.FireRate -= fireRate;
    }

    public void ReloadGun() => gunInfo.Ammunition = gunInfo.MaxMagazineSize;

    private void ShakeCamera()
    {
      if (disableCameraShake) return;
      
      _vCamera.ShakeCamera(cameraShakeIntensity, cameraShakeDuration);
    }
    public void FireBullet()
    {
      if (gunInfo.Ammunition <= 0 && !disableReload) return;
      
      _muzzleFlash = _muzzleFlash.hasLigt ? _muzzleFlash : GetMuzzleFlash(firePoint);

      ShakeCamera();

      if (_muzzleFlash.hasLigt)
      {
        StartCoroutine(nameof(Flash),0.1f);
      }
      
      CurrentAmmunition -= 1;
      shotSFX.Play();
      gunInfo.Shot(firePoint);
    }
    
    private (bool, Light2D) GetMuzzleFlash(Transform[] firePoint)
    {
      for (int i = 0; i < firePoint.Length; i++)
      {
        if (firePoint[i].TryGetComponent(out Light2D temp))
        {
          return (true, temp);
        }
      }
      return (true, null);
    }
    
    private IEnumerator Flash(float time)
    {
      _muzzleFlash.light.intensity = 1;
      yield return new WaitForSeconds(time);
      _muzzleFlash.light.intensity = 0;
    }
  }
}
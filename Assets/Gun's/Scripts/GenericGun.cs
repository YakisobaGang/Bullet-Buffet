using System;
using System.Collections;
using Ludiq.OdinSerializer.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using YakisobaGang;
using YakisobaGang.Scripts.Data;

namespace Scripts
{
  public class GenericGun : MonoBehaviour
  {
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    public GunData gunData;

    [SerializeField] [PropertyOrder(-1)] public Transform[] firePoint;
    [SerializeField] private float cameraShakeIntensity = 0.6f;
    [SerializeField] private float cameraShakeDuration = 0.4f;
    [SerializeField] private bool disableReload;
    [SerializeField] private bool disableCameraShake;
    [SerializeField] private AudioSource shotSFX;
    private ICanShot _canShot;
    private Transform _gunTransform;
    private SpriteRenderer _renderer;
    private float defaultFireRate = 1f;
    private (bool hasLigt, Light2D light) _muzzleFlash;

    public int CurrentAmmunition { get; private set; }
    public float currentFireRate = 1f;
    
    private void Awake()
    {
      
      _canShot = GetComponentInParent<ICanShot>();
      _renderer = GetComponent<SpriteRenderer>();
      _gunTransform = GetComponent<Transform>();
     
      _renderer.sprite = gunData.GunSprite;
      gunData.FireRate = defaultFireRate;
      gunData.FireRate = currentFireRate;
      CurrentAmmunition = gunData.Ammunition;
    }

    private void Start()
    {
      _canShot.onShot += (sender, args) => gunData.Shot(firePoint);
    }

    public void EditFireRate(float fireRate)
    {
      currentFireRate -= fireRate;
    }

    public void ReloadGun()
    {
      CurrentAmmunition = gunData.Ammunition;
    }

    public void FireBullet()
    {
      void ShakeCamera()
      {
        if (disableCameraShake) return;
        VCamera.Instance.ShakeCamera(cameraShakeIntensity, cameraShakeDuration);
      }

      _muzzleFlash = _muzzleFlash.hasLigt ? _muzzleFlash : GetMuzzleFlash(firePoint);
      
      if (CurrentAmmunition <= 0 && !disableReload) return;

      ShakeCamera();

      if (_muzzleFlash.hasLigt)
      {
        StartCoroutine("Flash",0.1f);
      }
      CurrentAmmunition -= 1;
      shotSFX.Play();
      gunData.Shot(firePoint);
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
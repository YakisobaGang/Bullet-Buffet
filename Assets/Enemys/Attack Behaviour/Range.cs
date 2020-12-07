using System;
using Pathfinding;
using Scripts;
using Unity.Mathematics;
using UnityEngine;
using YakisobaGang;

namespace Enemys.Attack_Behaviour
{
  [RequireComponent(typeof(EnemyBase))]
  public class Range : MonoBehaviour, ICanShot
  {
    [SerializeField] private float timeToNextShot = 1f;
    [SerializeField] private Transform gunHolder;
    
    private float _timeCopy;
    private Vector3 _lookDir;
    private Transform _target;
    private EnemyBase _myData;
    private AIPath _ai;
    private GenericGun _gun;
    
    private void Awake()
    {
      _timeCopy = timeToNextShot;
      _ai = GetComponent<AIPath>();
      _myData = GetComponent<EnemyBase>();
      _gun = gunHolder.GetComponentInChildren<GenericGun>();
      _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
      timeToNextShot -= Time.deltaTime;
      if (_ai.reachedDestination && timeToNextShot <=0)
      {
        onShot?.Invoke(this, EventArgs.Empty);
        timeToNextShot = _timeCopy;
      }
    }

    private void FixedUpdate()
    {
      _lookDir = (_target.position - _myData.MyTransform.position).normalized;
      print($"_lookDir: {_lookDir}");
      var angle = Mathf.Atan2(_lookDir.y, _lookDir.x) * Mathf.Rad2Deg;
      
      _myData.MyTransform.localScale = _lookDir.x <= 0 ? 
        new Vector3(-0.50f,0.50f,0.50f) : 
        new Vector3(0.50f,0.50f,0.50f);
      
      gunHolder.localRotation = Quaternion.Euler(0, 0, Mathf.Clamp(angle, -30, 30));
    }

    public event EventHandler onShot;
  }
}
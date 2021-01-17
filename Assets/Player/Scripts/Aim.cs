using System;
using CodeMonkey.Utils;
using UnityEngine;

namespace Player.Scripts
{
  public class Aim : MonoBehaviour
  {
    [SerializeField] private Transform armTransform;

    public float mira;
    private CircleCollider2D _hend;
    private Transform[] _transform;
    public Action<bool> FlipPlayer;

    private void Awake()
    {
      _transform = GetComponents<Transform>();
    }
  
    private void Update()
    {
      for (int i = 0; i < _transform.Length; i++)
      {
        var mousePos = UtilsClass.GetMouseWorldPosition();
        var armDirection = (mousePos - _transform[i].position).normalized;
        var angle = Mathf.Atan2(armDirection.y, armDirection.x) * Mathf.Rad2Deg;
      
        armTransform.eulerAngles = new Vector3(0, 0, angle - mira);
      }

      if (armTransform.rotation.normalized.z <= 0.0f || armTransform.rotation.normalized.z >= -0.0f)
      {
        FlipPlayer?.Invoke(false);
        armTransform.localPosition = new Vector3(0.319f, 0.309f, 0);
        armTransform.localScale = new Vector3(0.51f,0.51f,0.51f);

        for (int i = 0; i < _transform.Length; i++)
        {
          _transform[i].localRotation = Quaternion.Euler(0,0,_transform[i].localRotation.z * -1);
        }
      }

      if (armTransform.rotation.normalized.z >= 0.9f || armTransform.rotation.normalized.z <= -0.9f)
      {
        FlipPlayer?.Invoke(true);
        armTransform.localPosition = new Vector3(-0.765f, 0.309f, 0);
        armTransform.localScale = new Vector3(0.51f,-0.51f,0.51f);
      
        for (int i = 0; i < _transform.Length; i++)
        {
          _transform[i].localRotation = Quaternion.Euler(0,0,_transform[i].localRotation.z * 1);
        }
      }
    }
  }
}
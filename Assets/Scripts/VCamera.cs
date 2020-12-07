using Cinemachine;
using UnityEngine;

public class VCamera : MonoBehaviour
{
  public static VCamera Instance;
  private float _shakeTime;
  private CinemachineVirtualCamera _virtualCamera;

  private void Awake()
  {
    _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    Instance = this;
  }

  private void Update()
  {
    if (_shakeTime > 0)
    {
      _shakeTime -= Time.deltaTime;
      if (_shakeTime <= 0f)
      {
        var perlinNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlinNoise.m_AmplitudeGain = 0f;
      }
    }
  }

  public void ShakeCamera(float intensity, float time)
  {
    var perlinNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    _shakeTime = time;
    perlinNoise.m_AmplitudeGain = intensity;
  }
}
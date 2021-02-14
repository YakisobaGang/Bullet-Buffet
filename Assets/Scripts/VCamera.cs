using Cinemachine;
using UnityEngine;

public class VCamera : MonoBehaviour
{
    public static VCamera Instance;
    private float _shakeTime;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (_shakeTime > 0)
        {
            _shakeTime -= Time.deltaTime;
            if (_shakeTime <= 0f)
            {
                var perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlinNoise.m_AmplitudeGain = 0f;
                perlinNoise.m_FrequencyGain = 0f;
            }
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        var perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _shakeTime = time;
        perlinNoise.m_FrequencyGain = _shakeTime;
        perlinNoise.m_AmplitudeGain = intensity;
    }
}
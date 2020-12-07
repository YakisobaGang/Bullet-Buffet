using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayAnimationAfter : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private Light2D _light2D;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _light2D = GetComponent<Light2D>();
    }

    private void Update()
    {
        if (_particleSystem.time >= 1)
        {
            _light2D.intensity = 2 ;
        }

        if (!_particleSystem.isEmitting)
        {
            _light2D.intensity = 0;
        }
    }
}

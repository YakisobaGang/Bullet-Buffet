using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Canvas.Scripts
{
    public class AmmoCount : MonoBehaviour
    {
        [SerializeField] private Color32 outOfAmmoColor;
        [SerializeField] private Color32 hasAmmoColor;
        [Space(20f)]
        [SerializeField,Tooltip("is a Multiplier")] private float colorChangeSpeed = 1;
        
        private TextMeshProUGUI _text;
        private string _ammoText;

        private void Awake() => _text =
            GetComponent<TextMeshProUGUI>();

        private void Update()
        {
            _ammoText =
                $"{Player.Scripts.Player.AmmoCountText.currentAmmo} / {Player.Scripts.Player.AmmoCountText.maxAmmo}";

            if (Player.Scripts.Player.AmmoCountText.currentAmmo == "0")
            {
                _text.faceColor = 
                    Color32.Lerp(hasAmmoColor, outOfAmmoColor, 
                        Mathf.PingPong(Time.time * colorChangeSpeed,1));
            }
            else
            {
                _text.faceColor = hasAmmoColor;
            }

            _text.SetText(_ammoText);
        }
    }
}

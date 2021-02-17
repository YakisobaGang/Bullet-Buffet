using System;
using GameMaster;
using TMPro;
using UnityEngine;

namespace Canvas.Scripts
{
    public class CashHandler : MonoBehaviour
    {
        private TextMeshProUGUI texto;
        [SerializeField] private GameObject playerPrefab = null;
        public static event Action<int> PlayerCash;
        void Awake()
        {
            texto = GetComponent<TextMeshProUGUI>();
        }

        private void LateUpdate()
        {
            texto.SetText("Cash: " + playerPrefab.GetComponent<IHaveCash>().Cash);
            PlayerCash?.Invoke(playerPrefab.GetComponent<IHaveCash>().Cash);
        }
    }
}

using GameMaster;
using TMPro;
using UnityEngine;

namespace Canvas.Scripts
{
    public class ScoreHandler : MonoBehaviour
    {
        private TextMeshProUGUI texto;

        void Awake()
        {
            texto = GetComponent<TextMeshProUGUI>();
            ScoreAndCashSystem.Score = 0;
        }

        void FixedUpdate()
        {
            texto.SetText("Score: " + ScoreAndCashSystem.Score);
        }
    }
}

using TMPro;
using UnityEngine;

namespace Canvas.Scripts
{
    public class CashHandler : MonoBehaviour
    {
        private TextMeshProUGUI texto;

        void Awake()
        {
            texto = GetComponent<TextMeshProUGUI>();
            ScoreAndCashSystem.Cash = 0;
        }

        void FixedUpdate()
        {
            texto.SetText("Cash: " + ScoreAndCashSystem.Cash);
        }
    }
}

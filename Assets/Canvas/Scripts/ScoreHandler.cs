using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

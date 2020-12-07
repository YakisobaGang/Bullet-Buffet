using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using YakisobaGang.GameMaster;
using TMPro;

namespace YakisobaGang.Canvas
{
    public class HighScoreHandler : MonoBehaviour
    {
        [InlineEditor(InlineEditorObjectFieldModes.Foldout), SerializeField] private HighScoreData HighScore;

        private TextMeshProUGUI texto;

        void Awake()
        {
            texto = GetComponent<TextMeshProUGUI>();
        }

        void FixedUpdate()
        {
            if (ScoreAndCashSystem.HighScore < ScoreAndCashSystem.Score)
            {
                ScoreAndCashSystem.HighScore = ScoreAndCashSystem.Score;
                HighScore.HighScore = ScoreAndCashSystem.Score;
            }

            texto.SetText("Highscore: " + ScoreAndCashSystem.HighScore);
        }
    }
}
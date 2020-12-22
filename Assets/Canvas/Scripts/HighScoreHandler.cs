using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using YakisobaGang.GameMaster;

namespace Canvas.Scripts
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
using UnityEngine;

namespace GameMaster
{
    [CreateAssetMenu(fileName = "HighScoreData", menuName = "ScriptableObject/HighScoreData")]

    public class HighScoreData : ScriptableObject
    {
       [SerializeField] private int highScore;

        public int HighScore 
        {
            get => highScore;
            set => highScore = value;
        }
    }
}

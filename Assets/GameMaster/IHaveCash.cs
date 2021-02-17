using UnityEngine;

namespace GameMaster
{
    public interface IHaveScoreAndCash
    {
        public int Score { get; }
        public int HighScore { get; }
        public int Cash { get; }
    }
}

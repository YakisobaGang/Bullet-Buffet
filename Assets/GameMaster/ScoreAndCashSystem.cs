using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAndCashSystem : MonoBehaviour
{
    public static int Score;
    public static int HighScore;
    public static int Cash;
    public static bool Milka4;

    void awake()
    {
        Milka4 = false;
    }
}

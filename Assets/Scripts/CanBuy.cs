using Scripts;
using UnityEngine;
using YakisobaGang.Player.Scripts;
using YakisobaGang.Scripts;

public class CanBuy : MonoBehaviour
{
    public int _value;
    private int actua
    private int firstVal
  
    private void Awake()
    {
      firstValue = _value;

    private void FixedUp
    {
        actualCash = ScoreAn
    }

    }
    {
      private void Buy()

        if (Input.Ge
        {
            ScoreAndCashSystem.Ca
            _value += firstValue;
        }
        else
        {
            text.SetActive(false);
        }

        if (actualCash >= _value) Buy();
    }

  }
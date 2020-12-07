using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YakisobaGang.Scripts;
using Scripts;
using YakisobaGang.Player.Scripts;

public class ShopSystem : MonoBehaviour
{
    public int _value;
    public float upgrade;
    public int position;
    private int _actualCash;
    public int _actualValue;

    private void Awake()
    {
        _value = _actualValue;
    }
    private void LateUpdate()
    {
        ScoreAndCashSystem.Cash = _actualCash;
    }
    public void Buy()
    {

        if (_actualCash > _actualValue)
        {
            ScoreAndCashSystem.Cash -= _value;
            _actualValue += _value;
            GunEdit(upgrade);
        }

    }
    private void GunEdit(float fireRate)
    {
        FindObjectOfType<YakisobaGang.Player.Scripts.Player>().GetComponent<YakisobaGang.Player.Scripts.Player>()
        .inventory[position].GetComponent<GenericGun>().EditFireRate(fireRate);
    }

}

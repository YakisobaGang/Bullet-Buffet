using Scripts;
using UnityEngine;
using YakisobaGang.Player.Scripts;

public class ShopSystem : MonoBehaviour
{
  public int _value;
  public float upgrade;
  public int position;
  public int _actualValue;
  private int _actualCash;
  private Player _player;
  public bool useCash;

  private void Awake()
  {
    _value = _actualValue;
    _player = GameObject.FindWithTag("Player").GetComponent<Player>();
  }

  private void LateUpdate()
  {
    ScoreAndCashSystem.Cash = _actualCash;
  }

  public void Buy()
  {
    if (_actualCash >= _actualValue)
    {
      ScoreAndCashSystem.Cash -= _value;
      _actualValue += _value;
      GunEdit(upgrade);
    }
  }

  private void GunEdit(float fireRate)
  {
    _player.inventory[position].GetComponent<GenericGun>().EditFireRate(fireRate);
  }
}
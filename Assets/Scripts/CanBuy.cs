using Scripts;
using UnityEngine;
using YakisobaGang.Player.Scripts;
using YakisobaGang.Scripts;

public class CanBuy : MonoBehaviour
{
  public int _value;
  public Shop Shop;
  public string _name;
  public GameObject text;
  private bool _onShop;
  private int actualCash;
  private int firstValue;

  private void Awake()
  {
    firstValue = _value;
  }

  private void FixedUpdate()
  {
    actualCash = ScoreAndCashSystem.Cash;

    if (_onShop)
    {
      text.SetActive(true);
      Shop.ShopInfo(_name, _value);
    }
    else
    {
      text.SetActive(false);
    }

    if (actualCash >= _value) Buy();
  }

  private void OnTriggerEnter2D(Collider2D col)
  {
    if (col.CompareTag("Player")) _onShop = true;
  }

  private void OnTriggerExit2D(Collider2D col)
  {
    if (col.CompareTag("Player")) _onShop = false;
  }

  private void Buy()
  {
    if (Input.GetKeyDown(KeyCode.E) && _onShop)
    {
      ScoreAndCashSystem.Cash -= _value;
      _value += firstValue;
      milkA4FireRate(0.2f);
    }
  }

  private void milkA4FireRate(float fireRate)
  {
    FindObjectOfType<YakisobaGang.Player.Scripts.Player>().GetComponent<YakisobaGang.Player.Scripts.Player>()
      .inventory[0].GetComponent<GenericGun>().EditFireRate(fireRate);
  }
}
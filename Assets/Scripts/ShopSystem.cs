using UnityEngine;
using YakisobaGang.Scripts;

public class ShopSystem : MonoBehaviour
{
    private Player.Scripts.Player _player;
    public float upgrade;
    public int upgradePrice;
    public string weaponName;
    private int _playerCash = 9999;
    private bool _firstTime = true;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player.Scripts.Player>();
    }

    // private void LateUpdate()
    // {
    //     ScoreAndCashSystem.Cash = _playerCash;
    // }

    public void Buy()
    {
        if ((_playerCash < upgradePrice)) return;
        
        switch (weaponName)
        {
            case "Milk-A4":
                _player._secondaryGun.genericGun.gunInfo.FireRate -= upgrade;
                break;
            case "12 Candy":
                _player._primaryGun.genericGun.gunInfo.FireRate -= upgrade;
                
                break;
            case"Ket-9MR":
                if (_firstTime)
                {
                    _player.unlockThirdGun = true;
                    _firstTime = false;
                    break;
                }
                _player._primaryGun.genericGun.gunInfo.FireRate -= upgrade;
                break;
            case"Torta":
                _player.currentBombsCount = 3;
                break;
        }
    }
}
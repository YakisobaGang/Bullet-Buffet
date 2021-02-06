using UnityEngine;
using UnityEngine.Serialization;
using YakisobaGang.Scripts;

public class ShopSystem : MonoBehaviour
{
    private Player.Scripts.Player _player;
    [Tooltip("First upgrade, but for the Ket-9MR this doesn't work")]
    [FormerlySerializedAs("firstUpgrade")] public float fireReteUpgrade;
    [FormerlySerializedAs("secondUpgrade"), Tooltip("Second upgrade")] public float damageUpgrade;
    private float _maxDamageUpgrade = 20f;
    public int upgradePrice;
    public string weaponName;
    private int _playerCash = 9999;
    private bool _firstTime = true;
    private bool _secondTime = false;

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
                if (_firstTime)
                {
                    _player.SecondaryGun.genericGun.gunInfo.FireRate -= fireReteUpgrade;
                    (_firstTime, _secondTime) = (false, true);
                }
                else if (_secondTime)
                {
                    _player.SecondaryGun.genericGun.gunInfo.Damage = (int) Mathf.Min(damageUpgrade, _maxDamageUpgrade);
                }
                break;
            case "12 Candy":
                if (_firstTime)
                {
                    _player.PrimaryGun.genericGun.gunInfo.FireRate -= fireReteUpgrade;
                    (_firstTime, _secondTime) = (false, true);
                }
                else if (_secondTime)
                {
                    _player.PrimaryGun.genericGun.gunInfo.Damage = (int) Mathf.Min(damageUpgrade, _maxDamageUpgrade);
                }
                break;
            case"Ket-9MR":
                if (_firstTime)
                {
                    _player.unlockThirdGun = true;
                    (_firstTime, _secondTime) = (false, true);
                }
                else if (_secondTime)
                {
                    _player.ThirdGun.genericGun.gunInfo.Damage = (int) Mathf.Min(damageUpgrade, _maxDamageUpgrade);
                }
                break;
            case"Torta":
                _player.currentBombsCount = 3;
                break;
        }
    }
}
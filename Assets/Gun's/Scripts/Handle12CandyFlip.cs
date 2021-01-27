using Player.Scripts;
using UnityEngine;

namespace YakisobaGang.Scripts
{
    public class Handle12CandyFlip : MonoBehaviour
    {
        private void Start()
        {
            var gunData = GetComponent<GenericGun>().gunInfo;
            var firePoint = GetComponent<GenericGun>().firePoint;

            GetComponentInParent<Player.Scripts.Player>().ONFlip += delegate(bool b)
            {
                //print(!b);
                if (gunData.GunName != "12 Candy") return;
                
                if (!b)
                {
                    firePoint[0].localRotation = Quaternion.Euler(0f, 0f, -68.0f);
                    firePoint[1].localRotation = Quaternion.Euler(0f, 0f, -90.0f);
                    firePoint[2].localRotation = Quaternion.Euler(0f, 0f, -104.0f);
                }
                else
                {
                    firePoint[0].localRotation = Quaternion.Euler(0f, 0f, 68.0f);
                    firePoint[1].localRotation = Quaternion.Euler(0f, 0f, 90.0f);
                    firePoint[2].localRotation = Quaternion.Euler(0f, 0f, 104.0f);
                }
            };
        }
    }
}
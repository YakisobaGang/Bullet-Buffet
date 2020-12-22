using Player.Scripts;
using UnityEngine;

namespace YakisobaGang.Scripts
{
    public class Handle12CandyFlip : MonoBehaviour
    {
        void Start()
        {
            var gunData = GetComponent<GenericGun>().gunInfo;
            var firePoint = GetComponent<GenericGun>().firePoint;
        
            GetComponentInParent<Aim>().FlipPlayer += delegate(bool b)
            {
                try
                {
                    if (gunData.GunName != "12Candy") return;

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
                }
                catch (MissingReferenceException)
                {
                    return;
                }
            };
        }
    }
}

using Player.Scripts;
using UnityEngine;

namespace YakisobaGang.Scripts
{
  public class HanfleMilkA4Flip : MonoBehaviour
  {
    void Start()
    {
      var gunData = GetComponent<GenericGun>().gunInfo;
      var firePoint = GetComponent<GenericGun>().firePoint;
        
      GetComponentInParent<Aim>().FlipPlayer += delegate(bool b)
      {
        try
        {
          if (gunData.GunName != "Milk-A4") return;

          if (!b)
          {
            firePoint[0].localRotation = Quaternion.Euler(0f, 0f, -90.0f);
            
          }
          else
          {
            firePoint[0].localRotation = Quaternion.Euler(0f, 0f, 90.0f);
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
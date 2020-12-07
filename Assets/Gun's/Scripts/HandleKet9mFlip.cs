using UnityEngine;

namespace Scripts
{
  public class HandleKet9mFlip : MonoBehaviour
  {
    void Start()
    {
      var gunData = GetComponent<GenericGun>().gunData;
      var firePoint = GetComponent<GenericGun>().firePoint;
        
      GetComponentInParent<Aim>().FlipPlayer += delegate(bool b)
      {
        try
        {
          if (gunData.GunName != "Ket-9MR") return;

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
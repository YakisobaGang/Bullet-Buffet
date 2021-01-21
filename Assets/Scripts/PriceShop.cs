using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PriceShop : MonoBehaviour
{
    private TextMeshProUGUI texto;
    public GameObject Referenza;
    public int _referenza;

    void Awake()
    {
        texto = GetComponent<TextMeshProUGUI>();
        _referenza = Referenza.GetComponent<ShopSystem>().upgradePrice;
    }

    void LateUpdate()
    {
        texto.SetText(_referenza.ToString());
    }

}

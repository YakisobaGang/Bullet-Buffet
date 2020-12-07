using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    private TextMeshProUGUI texto;

    void Awake()
    {
        texto = GetComponent<TextMeshProUGUI>();
    }

    public void ShopInfo(string text, int _value) =>
        texto.SetText($"{text}:  ${_value}. Aperte E para comprar");

}

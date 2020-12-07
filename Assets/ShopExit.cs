using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopExit : MonoBehaviour
{
    public GameObject exit;

    public void Exit()
    {
        exit.SetActive(false);
        Time.timeScale = 1;
    }
}

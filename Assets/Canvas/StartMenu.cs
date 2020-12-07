using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public int index;
    public void StartGame() 
    {
        SceneManager.LoadScene(index);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}

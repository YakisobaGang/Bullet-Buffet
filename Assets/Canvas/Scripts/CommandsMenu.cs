﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace Canvas.Scripts
{
    public class CommandsMenu : MonoBehaviour
    {
        public int index;
        public void StartGame()
        {
            SceneManager.LoadScene(2);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

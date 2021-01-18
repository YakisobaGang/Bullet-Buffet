using UnityEngine;
using UnityEngine.SceneManagement;

namespace Canvas
{
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
}

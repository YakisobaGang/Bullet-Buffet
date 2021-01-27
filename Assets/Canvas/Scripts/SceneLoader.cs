using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace Canvas
{
    public class SceneLoader : MonoBehaviour
    {
        private readonly int _startTransition = Animator.StringToHash("Start");
        private readonly int _endTransition = Animator.StringToHash("End");
        
        [SerializeField] private Animator transition;
        [SerializeField,Tooltip("Time in seconds")] private float transitionTime;

        public void LoadNextScene()
        {
            LoadScene();
        }

        public void QuitGame() 
        {
            Application.Quit();
        }
        [ContextMenu("Load Scene")]
        void LoadScene()
        {
            StartCoroutine("CLoadScene", SceneManager.GetActiveScene().buildIndex + 1);
        }
        
        IEnumerator CLoadScene(int index)
        {
            print("start");
            transition.SetTrigger(_startTransition);

            // yield return new WaitForSeconds(transitionTime);
            var operation = SceneManager.LoadSceneAsync(index);
            while (!operation.isDone)
                yield return null;
            // SceneManager.LoadScene(index);
        }
    }
}

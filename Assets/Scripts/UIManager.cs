using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Planetile
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] string GameSceneName = string.Empty;

        public void StartGame()
        {
            SceneManager.LoadScene(GameSceneName);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void ContinueGame()
        {
            Time.timeScale = 1.0f;
            this.gameObject.SetActive(false);
        }
    }
}

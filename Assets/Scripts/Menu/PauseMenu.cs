using UnityEngine;
using UnityEngine.SceneManagement;

namespace TF2Jam.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _container;

        public static PauseMenu Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void Toggle()
        {
            if (_container.activeInHierarchy)
            {
                _container.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                ForcePause();
            }
        }

        public void ForcePause()
        {
            _container.SetActive(true);
            Time.timeScale = 0f;
        }

        public void Resume()
        {
            Toggle();
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void Menu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }
}

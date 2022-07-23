using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TF2Jam.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _container;

        [SerializeField]
        private TMP_Text _timerDisplay;

        public static PauseMenu Instance { get; private set; }

        private float _timer;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            _timerDisplay.text = $"{_timer:0.00}";
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void Menu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}

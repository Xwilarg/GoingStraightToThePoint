using TF2Jam.Translation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TF2Jam.Menu
{
    public class MainMenu : MonoBehaviour
    {
        public void SetFrench() => SetLanguage("french");

        public void SetEnglish() => SetLanguage("english");

        private void SetLanguage(string key)
        {
            Translate.Instance.CurrentLanguage = key;
        }

        public void DeleteAllSaves()
        {

        }

        public void StartLevel(string key)
        {
            SceneManager.LoadScene(key);
        }
    }
}

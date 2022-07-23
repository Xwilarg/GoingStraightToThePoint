using UnityEngine;
using UnityEngine.SceneManagement;

namespace TF2Jam.Menu
{
    public class MainMenu : MonoBehaviour
    {
        public void StartLevel(string level)
        {
            SceneManager.LoadScene(level);
        }
    }
}

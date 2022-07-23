using TF2Jam.Persistency;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TF2Jam.Menu
{
    public class LevelUI : MonoBehaviour
    {
        [SerializeField]
        private string _level;

        [SerializeField]
        private Button _playEasy, _playHard;

        private void Awake()
        {
            _playEasy.onClick.AddListener(new(() => { SceneManager.LoadScene(_level); }));
            _playHard.onClick.AddListener(new(() => { SceneManager.LoadScene($"{_level}H"); }));

            var data = PersistencyManager.Instance.GetLevelData(_level);
            if (!data.IsUnlocked)
            {
                gameObject.SetActive(false);
            }
            else if (!data.IsHardModeUnlocked)
            {
                _playHard.gameObject.SetActive(false);
            }
        }
    }
}

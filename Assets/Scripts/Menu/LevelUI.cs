using TF2Jam.Persistency;
using TMPro;
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

        [SerializeField]
        private TMP_Text _timerText;

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

        public void EasyEnter()
        {
            var time = PersistencyManager.Instance.GetLevelData(_level).BestTime;
            if (time > 0f)
            {
                _timerText.text = $"{time:0.00}";
                _timerText.gameObject.SetActive(true);
            }
        }
        public void EasyExit() { _timerText.gameObject.SetActive(false); }
        public void HardEnter()
        {
            var time = PersistencyManager.Instance.GetLevelData(_level).BestHardTime;
            if (time > 0f)
            {
                _timerText.text = $"{time:0.00}";
                _timerText.gameObject.SetActive(true);
            }
        }
        public void HardExit() { _timerText.gameObject.SetActive(false); }
    }
}

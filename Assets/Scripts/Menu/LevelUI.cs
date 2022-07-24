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

        [SerializeField]
        private GameObject _medalContainer;

        [SerializeField]
        private Image _medalEasy, _medalHard;

        [SerializeField]
        private Sprite _silverMedal, _goldMedal;

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

            (float targetEasy, float targetHard) = MedalManager.Medals[_level];
            if (data.BestTime < 0f)
            {
                _medalEasy.gameObject.SetActive(false);
            }
            else if (data.BestTime < targetEasy)
            {
                _medalEasy.sprite = _goldMedal;
            }
            else if (data.BestTime < targetEasy + MedalManager.GetSilver(targetEasy))
            {
                _medalEasy.sprite = _silverMedal;
            }
            if (data.BestHardTime < 0f)
            {
                _medalHard.gameObject.SetActive(false);
            }
            else if (data.BestHardTime < targetHard)
            {
                _medalHard.sprite = _goldMedal;
            }
            else if (data.BestHardTime < targetHard + MedalManager.GetSilver(targetHard))
            {
                _medalHard.sprite = _silverMedal;
            }
        }

        public void EasyEnter()
        {
            var time = PersistencyManager.Instance.GetLevelData(_level).BestTime;
            if (time > 0f)
            {
                _timerText.text = $"{time:0.00}";
                _timerText.gameObject.SetActive(true);
                _medalContainer.SetActive(false);
            }
        }
        public void EasyExit()
        {
            _timerText.gameObject.SetActive(false);
            _medalContainer.SetActive(true);
        }
        public void HardEnter()
        {
            var time = PersistencyManager.Instance.GetLevelData(_level).BestHardTime;
            if (time > 0f)
            {
                _timerText.text = $"{time:0.00}";
                _timerText.gameObject.SetActive(true);
                _medalContainer.SetActive(false);
            }
        }
        public void HardExit()
        {
            _timerText.gameObject.SetActive(false);
            _medalContainer.SetActive(true);
        }
    }
}

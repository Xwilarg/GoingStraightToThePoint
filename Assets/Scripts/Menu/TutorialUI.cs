using TF2Jam.Persistency;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TF2Jam.Menu
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField]
        private string _level;

        [SerializeField]
        private TMP_Text _timerText;

        [SerializeField]
        private GameObject _medalContainer;

        [SerializeField]
        private Image _medalEasy;

        [SerializeField]
        private Sprite _silverMedal, _goldMedal;

        private void Awake()
        {
            var data = PersistencyManager.Instance.GetLevelData(_level);
            if (!data.IsUnlocked)
            {
                gameObject.SetActive(false);
            }
            (float targetEasy, _) = MedalManager.Medals[_level];
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
    }
}
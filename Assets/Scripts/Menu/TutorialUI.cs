using TF2Jam.Persistency;
using TMPro;
using UnityEngine;

namespace TF2Jam.Menu
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField]
        private string _level;

        [SerializeField]
        private TMP_Text _timerText;

        private void Awake()
        {
            var data = PersistencyManager.Instance.GetLevelData(_level);
            if (!data.IsUnlocked)
            {
                gameObject.SetActive(false);
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
    }
}
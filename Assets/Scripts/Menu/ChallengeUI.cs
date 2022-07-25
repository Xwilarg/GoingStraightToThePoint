using TF2Jam.Persistency;
using TMPro;
using UnityEngine;

namespace TF2Jam.Menu
{
    public class ChallengeUI : MonoBehaviour
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
            var time = PersistencyManager.Instance.GetLevelData(_level).BestTime;
            if (time > 0f)
            {
                _timerText.text = _level == "4-3" ? $"{time:0.00}m" : $"{time:0.00}";
                _timerText.gameObject.SetActive(true);
            }
        }
    }
}
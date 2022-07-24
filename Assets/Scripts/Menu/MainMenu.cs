using TF2Jam.Persistency;
using TF2Jam.Player;
using TF2Jam.Translation;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TF2Jam.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _modeToggle;

        [SerializeField]
        private Image _emblemDisplay;

        [SerializeField]
        private Sprite _emblemSoldier, _emblemDemoman;

        [SerializeField]
        private TMP_Text _currentClassText;

        private void Awake()
        {
            if (!PersistencyManager.Instance.GetLevelData("3-3").IsHardModeUnlocked)
            {
                _modeToggle.gameObject.SetActive(false);
            }
            else
            {
                UpdateClassDisplay();
            }
        }

        private void UpdateClassDisplay()
        {
            _emblemDisplay.sprite = PersistencyManager.Instance.CurrentClass == PlayerClass.Soldier ? _emblemSoldier : _emblemDemoman;
            _currentClassText.text = PersistencyManager.Instance.CurrentClass == PlayerClass.Soldier ? "Soldier" : "Demoman";
        }

        public void NextClass()
        {
            PersistencyManager.Instance.CurrentClass = PersistencyManager.Instance.CurrentClass == PlayerClass.Soldier ? PlayerClass.Demoman : PlayerClass.Soldier;
            UpdateClassDisplay();
        }

        public void SetFrench() => SetLanguage("french");

        public void SetEnglish() => SetLanguage("english");

        private void SetLanguage(string key)
        {
            Translate.Instance.CurrentLanguage = key;
        }

        public void DeleteAllSaves()
        {
            PersistencyManager.Instance.ResetAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void StartLevel(string key)
        {
            SceneManager.LoadScene(key);
        }
    }
}

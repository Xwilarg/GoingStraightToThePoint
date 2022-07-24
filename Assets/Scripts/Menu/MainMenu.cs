using TF2Jam.Audio;
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

        [SerializeField]
        private TMP_Text _bgmX, _soundsX;

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
            UpdateSoundsDisplay();
        }

        private void UpdateClassDisplay()
        {
            _emblemDisplay.sprite = PersistencyManager.Instance.CurrentClass == PlayerClass.Soldier ? _emblemSoldier : _emblemDemoman;
            _currentClassText.text = PersistencyManager.Instance.CurrentClass == PlayerClass.Soldier ? "Soldier" : "Demoman";
        }

        private void UpdateSoundsDisplay()
        {
            _bgmX.text = PersistencyManager.Instance.IsBGMActive ? "X" : string.Empty;
            _soundsX.text = PersistencyManager.Instance.IsSoundsActive ? "X" : string.Empty;
            GameObject.FindGameObjectWithTag("BGM_Menu").GetComponent<AudioSource>().volume = PersistencyManager.Instance.IsBGMActive ? 1f : 0f;
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

        public void ToggleBGM()
        {
            PersistencyManager.Instance.IsBGMActive = !PersistencyManager.Instance.IsBGMActive;
            UpdateSoundsDisplay();
        }

        public void ToggleSounds()
        {
            PersistencyManager.Instance.IsSoundsActive = !PersistencyManager.Instance.IsSoundsActive;
            UpdateSoundsDisplay();
        }
    }
}

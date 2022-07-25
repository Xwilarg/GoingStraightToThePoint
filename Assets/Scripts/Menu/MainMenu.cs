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
        private Sprite _emblemSoldier, _emblemDemoman, _emblemEngineer, _emblemFairy;

        [SerializeField]
        private TMP_Text _currentClassText;

        [SerializeField]
        private TMP_Text _bgmX, _soundsX;

        [SerializeField]
        private GameObject _expClassButton;

        [SerializeField]
        private TMP_Text _helpText;

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

            if (PersistencyManager.Instance.AllowAdditionalClasses)
            {
                _expClassButton.gameObject.SetActive(false);
            }
        }

        public void ShowHelpText()
        {
            _helpText.text = PersistencyManager.Instance.CurrentClass switch
            {
                PlayerClass.Soldier => Translate.Instance.Tr("helpSoldier"),
                PlayerClass.Demoman => Translate.Instance.Tr("helpDemoman"),
                PlayerClass.Engineer => Translate.Instance.Tr("helpEngineer"),
                PlayerClass.IceFairy => Translate.Instance.Tr("helpIceFairy"),
                _ => string.Empty
            };
        }

        private void UpdateClassDisplay()
        {
            if (PersistencyManager.Instance.CurrentClass == PlayerClass.Soldier)
            {
                _emblemDisplay.sprite = _emblemSoldier;
                _currentClassText.text = "Soldier";
            }
            else if (PersistencyManager.Instance.CurrentClass == PlayerClass.Demoman)
            {
                _emblemDisplay.sprite = _emblemDemoman;
                _currentClassText.text = "Demoman";
            }
            else if (PersistencyManager.Instance.CurrentClass == PlayerClass.Engineer)
            {
                _emblemDisplay.sprite = _emblemEngineer;
                _currentClassText.text = "Engineer";
            }
            else if (PersistencyManager.Instance.CurrentClass == PlayerClass.IceFairy)
            {
                _emblemDisplay.sprite = _emblemFairy;
                _currentClassText.text = Translate.Instance.Tr("iceFairy");
            }
        }

        private void UpdateSoundsDisplay()
        {
            _bgmX.text = PersistencyManager.Instance.IsBGMActive ? "X" : string.Empty;
            _soundsX.text = PersistencyManager.Instance.IsSoundsActive ? "X" : string.Empty;
            GameObject.FindGameObjectWithTag("BGM_Menu").GetComponent<AudioSource>().volume = PersistencyManager.Instance.IsBGMActive ? 1f : 0f;
        }

        public void NextClass()
        {
            PersistencyManager.Instance.CurrentClass = PersistencyManager.Instance.CurrentClass switch
            {
                PlayerClass.Soldier => PersistencyManager.Instance.GetLevelData("3-3").IsHardModeUnlocked ? PlayerClass.Demoman : PlayerClass.Engineer,
                PlayerClass.Demoman => PersistencyManager.Instance.AllowAdditionalClasses ? PlayerClass.Engineer : PlayerClass.Soldier,
                PlayerClass.Engineer => PlayerClass.IceFairy,
                PlayerClass.IceFairy => PlayerClass.Soldier
            };
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

        public void ActivateExperimentalClasses()
        {
            _expClassButton.gameObject.SetActive(false);
            PersistencyManager.Instance.AllowAdditionalClasses = true;
            _modeToggle.gameObject.SetActive(true);
        }
    }
}

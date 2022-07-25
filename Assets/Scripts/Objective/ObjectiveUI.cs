using System.Linq;
using TF2Jam.Audio;
using TF2Jam.Menu;
using TF2Jam.Persistency;
using TF2Jam.Player;
using TF2Jam.Translation;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TF2Jam.Objective
{
    public class ObjectiveUI : MonoBehaviour
    {
        public static ObjectiveUI Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public bool IsTimerActive { set; private get; }

        public bool DidWin { private set; get; }

        [SerializeField]
        private Transform _container;

        [SerializeField]
        private GameObject _objPrefab;

        [SerializeField]
        private Sprite _sprUnlocked, _sprCaptured;

        [SerializeField]
        private TMP_Text _timerDisplay;

        [SerializeField]
        private GameObject _winMenu;

        [SerializeField]
        private Image _silverMedal, _goldMedal;

        [SerializeField]
        private GameObject _newRecord;

        [SerializeField]
        private AudioClip _captureSound;

        [SerializeField]
        private TMP_Text _nextMedalInfo;

        [SerializeField]
        private GameObject _medalContainer;

        private float _timer;

        private CPUI[] _controlPoints;

        private int _index;

        public bool IsDistLevel { set; get; }

        private void Start()
        {
            _controlPoints = GameObject.FindGameObjectsWithTag("ControlPoint")
                .Select(x => new CPUI()
                {
                    CP = x.GetComponent<ControlPoint>()
                })
                .OrderBy(x => x.CP.transform.position.x).ToArray();
            for (int i = 0; i < _controlPoints.Length; i++)
            {
                var obj = _controlPoints[i];
                var ui = Instantiate(_objPrefab, _container);
                obj.CP.Init(this);
                obj.UI = ui.GetComponent<Image>();
            }

            if (_controlPoints.Any())
            {
                _controlPoints[0].UI.sprite = _sprUnlocked;
            }

            IsDistLevel = SceneManager.GetActiveScene().name == "4-3";
        }

        private float _meterDist;
        public void UpdateMeterDisplay(float x)
        {
            if (!DidWin)
            {
                _meterDist = x > 5f ? x - 5f : 0f;
                _timerDisplay.text = $"{_meterDist:0.00}m";
            }
        }

        private void Update()
        {
            if (!IsDistLevel && IsTimerActive && !DidWin)
            {
                _timer += Time.deltaTime;
                _timerDisplay.text = $"{_timer:0.00}";
            }
        }

        public void ResetTimer()
        {
            _timer = 0f;
            _timerDisplay.text = IsDistLevel ? "0.00m" : "0.00";
            IsTimerActive = false;
        }

        public Vector2? LatestCaptured => _index == 0 ? null : ((Vector2)_controlPoints[_index - 1].CP.transform.position + Vector2.up); // TODO: Fix pos if index is 0

        public void TriggerGameEnd()
        {
            foreach (var pc in GameObject.FindGameObjectsWithTag("Player").Select(x => x.GetComponent<PlayerController>()))
            {
                pc.DidWin = true;
            }
            DidWin = true;

            if (IsDistLevel)
            {
                _timer = _meterDist;
            }

            _winMenu.SetActive(true);
            _winMenu.GetComponent<VictoryMenu>().Init(_timer);
            _timerDisplay.text = IsDistLevel ? $"{_timer:0.00}m" : $"{_timer:0.00}";

            if (PersistencyManager.Instance.CurrentClass == PlayerClass.Soldier)
            {
                var levelName = SceneManager.GetActiveScene().name;
                var isHard = levelName.EndsWith('H');
                var data = PersistencyManager.Instance.GetLevelData(isHard ? levelName[..^1] : levelName);
                if ((isHard ? data.BestHardTime : data.BestTime) < 0f ||
                    (!IsDistLevel && _timer < (isHard ? data.BestHardTime : data.BestTime)) ||
                    (IsDistLevel && _timer > data.BestTime))
                {
                    _newRecord.gameObject.SetActive(true);
                }
                PersistencyManager.Instance.FinishLevel(levelName, _timer, IsDistLevel);
                var tMed = MedalManager.Medals[isHard ? levelName[..^1] : levelName];
                var target = isHard ? tMed.Hard : tMed.Easy;

                if (target < 0f)
                {
                    _medalContainer.SetActive(false);
                }
                else
                {
                    if (_timer < target)
                    {
                        _goldMedal.color = Color.white;
                        _silverMedal.color = Color.white;
                    }
                    else if (_timer < target + MedalManager.GetSilver(target))
                    {
                        _silverMedal.color = Color.white;
                        _nextMedalInfo.text = Translate.Instance.Tr("goldAt", $"{target:0.00}");
                    }
                    else
                    {
                        _nextMedalInfo.text = Translate.Instance.Tr("silverAt", $"{(target + MedalManager.GetSilver(target)):0.00}");
                    }
                }
            }
            else
            {
                _medalContainer.SetActive(false);
            }
        }

        public bool Capture(ControlPoint cp)
        {
            if (cp.GetInstanceID() == _controlPoints[_index].CP.GetInstanceID())
            {
                _controlPoints[_index].UI.sprite = _sprCaptured;
                _index++;
                AudioManager.Instance.PlayClip(_captureSound);
                if (_index < _controlPoints.Length)
                {
                    _controlPoints[_index].UI.sprite = _sprUnlocked;
                }
                else // We got all CP
                {
                    TriggerGameEnd();
                }
                return true;
            }
            return false;
        }

        private class CPUI
        {
            public Image UI { set; get; }
            public ControlPoint CP { set; get; }
        }
    }
}

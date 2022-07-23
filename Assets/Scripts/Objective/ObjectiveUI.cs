using System.Linq;
using TF2Jam.Player;
using UnityEngine;
using UnityEngine.UI;

namespace TF2Jam.Objective
{
    public class ObjectiveUI : MonoBehaviour
    {
        [SerializeField]
        private Transform _container;

        [SerializeField]
        private GameObject _objPrefab;

        [SerializeField]
        private Sprite _sprUnlocked, _sprCaptured;

        private CPUI[] _controlPoints;

        private int _index;

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

            _controlPoints[0].UI.sprite = _sprUnlocked;
        }

        public bool Capture(ControlPoint cp)
        {
            if (cp.GetInstanceID() == _controlPoints[_index].CP.GetInstanceID())
            {
                _controlPoints[_index].UI.sprite = _sprCaptured;
                _index++;
                if (_index < _controlPoints.Length)
                {
                    _controlPoints[_index].UI.sprite = _sprUnlocked;
                }
                else // We got all CP
                {
                    foreach (var pc in GameObject.FindGameObjectsWithTag("Player").Select(x => x.GetComponent<PlayerController>()))
                    {
                        pc.DidWin = true;
                    }
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

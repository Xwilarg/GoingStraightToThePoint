using UnityEngine;

namespace TF2Jam.Objective
{
    public class ControlPoint : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _pointSR, _iconSR;

        [SerializeField]
        private Sprite _pointRed, _iconRed;

        private ObjectiveUI _manager;

        public void Init(ObjectiveUI manager)
        {
            _manager = manager;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && _manager.Capture(this))
            {
                _pointSR.sprite = _pointRed;
                _iconSR.sprite = _iconRed;
            }
        }
    }
}

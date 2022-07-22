using UnityEngine;

namespace TF2Jam
{
    public class ControlPoint : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _pointSR, _iconSR;

        [SerializeField]
        private Sprite _pointRed, _iconRed;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _pointSR.sprite = _pointRed;
                _iconSR.sprite = _iconRed;
            }
        }
    }
}

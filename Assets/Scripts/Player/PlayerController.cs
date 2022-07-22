using System.Collections;
using TF2Jam.SO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TF2Jam.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _info;

        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private float _mov;
        private Camera _cam;
        private int _jumpIgnoreMask;

        private bool _canShoot = true;

        private Vector2 _lastValidPos;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _cam = Camera.main;
            _jumpIgnoreMask = (1 << LayerMask.NameToLayer("Player"));
            _jumpIgnoreMask |= (1 << LayerMask.NameToLayer("Projectile"));
            _jumpIgnoreMask = ~_jumpIgnoreMask;
        }

        private void FixedUpdate()
        {
            if (IsOnFloor(out string tag))
            {
                _rb.velocity = new Vector2(_info.Speed * Time.deltaTime * _mov, _rb.velocity.y);
                if (tag == "Floor")
                {
                    _lastValidPos = transform.position;
                }
            }
            Debug.Log(_rb.velocity);
        }

        public void ResetPos()
        {
            transform.position = _lastValidPos;
            _rb.velocity = Vector2.zero;
        }

        public void AddPropulsionForce(float force, Vector2 direction)
        {
            _rb.AddForce(direction * force, ForceMode2D.Impulse);
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(_info.ReloadTime);
            _canShoot = true;
        }

        public void OnMovement(InputAction.CallbackContext value)
        {
            _mov = value.ReadValue<Vector2>().x;
            if (_mov > 0f)
            {
                _sr.flipX = false;
            }
            else if (_mov < 0f)
            {
                _sr.flipX = true;
            }
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (value.performed && IsOnFloor(out _))
            {
                _rb.AddForce(Vector2.up * _info.JumpForce, ForceMode2D.Impulse);
            }
        }

        public void OnAction(InputAction.CallbackContext value)
        {
            if (value.performed && _canShoot)
            {
                var screenPos = _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

                var go = Instantiate(_info.RocketPrefab, transform.position, Quaternion.identity);
                Vector3 relPos = screenPos - transform.position;
                float angle = Mathf.Atan2(relPos.y, relPos.x) * Mathf.Rad2Deg;
                go.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                go.GetComponent<Rigidbody2D>().AddForce(go.transform.right * _info.RocketSpeed);
                go.GetComponent<Bullet>().Init(this);
                Destroy(go, 10f);

                _canShoot = false;
                StartCoroutine(Reload());
            }
        }

        private bool IsOnFloor(out string tag)
        {
            var ray = Physics2D.Raycast(transform.position, Vector2.down, float.PositiveInfinity, _jumpIgnoreMask);
            if (ray.collider != null && ray.distance < _info.FloorDistanceForJump)
            {
                tag = ray.collider.tag;
                return true;
            }
            tag = null;
            return false;
        }
    }
}

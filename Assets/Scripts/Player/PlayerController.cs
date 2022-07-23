using System.Collections;
using TF2Jam.Menu;
using TF2Jam.Objective;
using TF2Jam.SO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace TF2Jam.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _info;

        private bool _didWin;
        public bool DidWin
        {
            set
            {
                _didWin = value;
                if (_didWin)
                {
                    Time.timeScale = 0f;
                }
            }
            private get => _didWin;
        }

        private Rigidbody2D _rb;
        private Animator _anim;
        private float _mov;
        private Camera _cam;
        private int _jumpIgnoreMask;
        private Vector2 _spawnPos;

        private bool _canShoot = true;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();
            _cam = Camera.main;
            _jumpIgnoreMask = (1 << LayerMask.NameToLayer("Player"));
            _jumpIgnoreMask |= (1 << LayerMask.NameToLayer("Projectile"));
            _jumpIgnoreMask = ~_jumpIgnoreMask;
            _spawnPos = transform.position;

            SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        }

        private void FixedUpdate()
        {
            if (!DidWin && IsOnFloor(out string tag))
            {
                _rb.velocity = new Vector2(_info.Speed * Time.deltaTime * _mov, _rb.velocity.y);
                _anim.SetBool("IsWalking", _rb.velocity.x != 0f);
            }
            else
            {
                _anim.SetBool("IsWalking", false);
            }
            if (transform.position.y < -5f)
            {
                ResetPos();
            }
        }

        public void ResetPos()
        {
            transform.position = ObjectiveUI.Instance.LatestCaptured ?? _spawnPos;
            _rb.velocity = Vector2.zero;
        }

        public void AddPropulsionForce(float force, Vector2 direction)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Abs(_rb.velocity.y));
            _rb.AddForce(direction * force, ForceMode2D.Impulse);
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(_info.ReloadTime);
            _canShoot = true;
        }

        public void OnMovement(InputAction.CallbackContext value)
        {
            if (DidWin)
            {
                return;
            }
            _mov = value.ReadValue<Vector2>().x;
            if (_mov > 0f)
            {
                _anim.SetBool("IsGoingRight", true);
                ObjectiveUI.Instance.IsTimerActive = true;
            }
            else if (_mov < 0f)
            {
                _anim.SetBool("IsGoingRight", false);
                ObjectiveUI.Instance.IsTimerActive = true;
            }
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (value.performed && !DidWin && IsOnFloor(out _))
            {
                _rb.AddForce(Vector2.up * _info.JumpForce, ForceMode2D.Impulse);
                ObjectiveUI.Instance.IsTimerActive = true;
            }
        }

        public void OnAction(InputAction.CallbackContext value)
        {
            if (value.performed && !DidWin && _canShoot)
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
                ObjectiveUI.Instance.IsTimerActive = true;
            }
        }

        public void OnPause(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                PauseMenu.Instance.Toggle();
            }
        }

        public void OnRestart(InputAction.CallbackContext value)
        {
            if (value.performed && !DidWin)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        public void OnApplicationFocus(bool focus)
        {
            if (!focus)
            {
                PauseMenu.Instance.ForcePause();
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

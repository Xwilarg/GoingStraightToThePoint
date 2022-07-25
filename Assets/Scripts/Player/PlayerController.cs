using System.Collections;
using System.Collections.Generic;
using TF2Jam.Audio;
using TF2Jam.Menu;
using TF2Jam.Objective;
using TF2Jam.Persistency;
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

        private RuntimeAnimatorController GetAnimator()
        {
            return PersistencyManager.Instance.CurrentClass switch
            {
                PlayerClass.Soldier => _info.SoldierAnim,
                PlayerClass.Demoman => _info.DemomanAnim,
                PlayerClass.Engineer => _info.EngineerAnim,
                PlayerClass.IceFairy => _info.IceFairyAnim,
                _ => null
            };
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();
            _anim.runtimeAnimatorController = GetAnimator();
            _cam = Camera.main;
            if (SceneManager.GetActiveScene().name[0] == '3')
            {
                _cam.backgroundColor = new Color32(212, 149, 192, 255);
            }
            else if (SceneManager.GetActiveScene().name == "4-2")
            {
                _cam.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
            _jumpIgnoreMask = (1 << LayerMask.NameToLayer("Player"));
            _jumpIgnoreMask |= (1 << LayerMask.NameToLayer("Projectile"));
            _jumpIgnoreMask = ~_jumpIgnoreMask;
            _spawnPos = transform.position;

            SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        }

        private void FixedUpdate()
        {
            if (!DidWin)
            {
                if (IsOnFloor(out string _))
                {
                    if (ObjectiveUI.Instance.IsDistLevel && transform.position.x > 5f)
                    {
                        ObjectiveUI.Instance.TriggerGameEnd();
                    }
                    _rb.velocity = new Vector2(_info.Speed * Time.deltaTime * _mov, _rb.velocity.y);
                    if (_rb.velocity.x != 0f)
                    {
                        _anim.SetBool("IsWalking", true);
                        ObjectiveUI.Instance.IsTimerActive = true;
                    }
                    else
                    {
                        _anim.SetBool("IsWalking", false);
                    }
                }
                else
                {
                    var maxSpeed = _info.Speed * Time.deltaTime;
                    var curr = _info.Speed * Time.deltaTime * _mov * .01f + _rb.velocity.x;
                    if (Mathf.Abs(curr) < maxSpeed)
                    {
                        _rb.velocity = new Vector2(curr, _rb.velocity.y);
                    }
                    _anim.SetBool("IsWalking", false);
                }

                if (!_hasSentry)
                {
                    var screenPos = _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    var posX = Mathf.Abs(_sentry.transform.localScale.x);
                    _sentry.transform.localScale =
                        new Vector3(
                            posX * (screenPos.x < _sentry.transform.position.x ? -1f : 1f),
                            _sentry.transform.localScale.y,
                            _sentry.transform.localScale.z);
                }
            }
            else
            {
                _anim.SetBool("IsWalking", false);
            }
            if (transform.position.y < -5f)
            {
                if (ObjectiveUI.Instance.IsDistLevel)
                {
                    ObjectiveUI.Instance.TriggerGameEnd();
                }
                else
                {
                    ResetPos();
                }
            }

            if (ObjectiveUI.Instance.IsDistLevel)
            {
                ObjectiveUI.Instance.UpdateMeterDisplay(transform.position.x);
            }
        }

        public void ResetPos()
        {
            var lastCP = ObjectiveUI.Instance.LatestCaptured;
            if (lastCP == null)
            {
                ObjectiveUI.Instance.ResetTimer();
            }
            transform.position = lastCP ?? _spawnPos;
            _rb.velocity = Vector2.zero;
            foreach (var go in GameObject.FindGameObjectsWithTag("Projectile"))
            {
                Destroy(go);
            }
            _bombs.Clear();
            if (!_hasSentry)
            {
                _hasSentry = true;
                Destroy(_sentry.transform.parent.gameObject);
            }
        }

        public void AddPropulsionForce(float force, Vector2 direction, Vector2 contactPoint)
        {
            if (contactPoint.y < transform.position.y)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Abs(_rb.velocity.y));
            }
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

        private readonly List<StickyBomb> _bombs = new();
        private bool _hasSentry = true;
        private GameObject _sentry;

        public void OnAction(InputAction.CallbackContext value)
        {
            if (value.performed && !DidWin && _canShoot)
            {
                var screenPos = _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

                if (PersistencyManager.Instance.CurrentClass == PlayerClass.Engineer)
                {
                    if (!_hasSentry)
                    {
                        var go = Instantiate(_info.SentryRocketPrefab, _sentry.transform.position, Quaternion.identity);
                        Vector3 relPos = screenPos - _sentry.transform.position;
                        float angle = Mathf.Atan2(relPos.y, relPos.x) * Mathf.Rad2Deg;
                        go.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                        go.GetComponent<Rigidbody2D>().AddForce(go.transform.right * (PersistencyManager.Instance.CurrentClass == PlayerClass.Soldier ? _info.RocketSpeed : _info.StickySpeed));
                        go.GetComponent<Bullet>().Init(this);

                        Destroy(go, PersistencyManager.Instance.CurrentClass == PlayerClass.IceFairy ? 20f : 10f);
                        AudioManager.Instance.PlayClip(_info._shootAudio);

                        _canShoot = false;
                        StartCoroutine(Reload());
                    }
                }
                else
                {
                    var go = Instantiate(PersistencyManager.Instance.CurrentClass switch
                    {
                        PlayerClass.Soldier => _info.RocketPrefab,
                        PlayerClass.Demoman => _info.StickyPrefab,
                        PlayerClass.IceFairy => _info.IceRocketPrefab
                    }, transform.position, Quaternion.identity);
                    Vector3 relPos = screenPos - transform.position;
                    float angle = Mathf.Atan2(relPos.y, relPos.x) * Mathf.Rad2Deg;
                    go.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    go.GetComponent<Rigidbody2D>().AddForce(go.transform.right * (PersistencyManager.Instance.CurrentClass == PlayerClass.Soldier ? _info.RocketSpeed : _info.StickySpeed));
                    if (PersistencyManager.Instance.CurrentClass == PlayerClass.Soldier) // TODO: Add inheritance or smth
                    {
                        go.GetComponent<Bullet>().Init(this);
                    }
                    else if (PersistencyManager.Instance.CurrentClass == PlayerClass.IceFairy)
                    {
                        go.GetComponent<BulletIce>().Init(this);
                    }
                    else
                    {
                        var bomb = go.GetComponent<StickyBomb>();
                        bomb.Init(this);
                        _bombs.Add(bomb);
                        if (_bombs.Count > 2)
                        {
                            Destroy(_bombs[0].gameObject);
                            _bombs.RemoveAt(0);
                        }
                    }
                    Destroy(go, 10f);
                    AudioManager.Instance.PlayClip(_info._shootAudio);

                    _canShoot = false;
                    StartCoroutine(Reload());
                }
                ObjectiveUI.Instance.IsTimerActive = true;
            }
        }

        public void OnAction2(InputAction.CallbackContext value)
        {
            if (value.performed && !DidWin && _canShoot)
            {
                if (PersistencyManager.Instance.CurrentClass == PlayerClass.Engineer)
                {
                    var screenPos = _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    var info = Physics2D.Raycast(transform.position, screenPos - transform.position, float.PositiveInfinity, _jumpIgnoreMask);
                    if (info.collider != null)
                    {
                        if (_hasSentry)
                        {
                            _sentry = Instantiate(_info.Sentry, info.point + Vector2.up * 2.2f, Quaternion.identity).transform.GetChild(0).gameObject;
                            _hasSentry = false;
                            /*
                            var ray = Physics2D.Raycast(info.point + Vector2.down, Vector2.down, float.PositiveInfinity, _jumpIgnoreMask);
                            if (ray.collider != null && ray.distance < .1f)
                            {
                            }
                            */
                        }
                        else
                        {
                            if (info.collider.CompareTag("Sentry"))
                            {
                                _hasSentry = true;
                                Destroy(info.collider.gameObject);
                            }
                        }
                    }
                }
                else if (PersistencyManager.Instance.CurrentClass == PlayerClass.Demoman)
                {
                    foreach (var bomb in _bombs)
                    {
                        bomb.Explodes();
                    }
                    _bombs.Clear();
                }
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

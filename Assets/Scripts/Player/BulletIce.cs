using System.Linq;
using TF2Jam.Audio;
using TF2Jam.SO;
using UnityEngine;

namespace TF2Jam.Player
{
    public class BulletIce : MonoBehaviour
    {
        [SerializeField]
        private RocketJumpInfo _info;

        private PlayerController _owner;

        public void Init(PlayerController owner)
        {
            _owner = owner;
        }

        private bool _isPlanted;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isPlanted)
            {
                var contact = collision.contacts.First().point;
                var force = Vector2.Distance(contact, _owner.transform.position);
                if (force < _info.RocketImpactMaxDistance)
                {
                    var propForce = (1f - force / _info.RocketImpactMaxDistance) * _info.RocketPropulsionForce;
                    _owner.AddPropulsionForce(propForce, (Vector2)_owner.transform.position - contact, contact);
                }
                AudioManager.Instance.PlayClip(_info.ExplosionSound);
                Destroy(Instantiate(_info.ExplosionPrefab, contact, Quaternion.identity), 1f);
                Destroy(gameObject);
            }
            else
            {
                var rb = GetComponent<Rigidbody2D>();
                rb.isKinematic = true;
                rb.velocity = Vector2.zero;
                gameObject.layer = 0;
                Destroy(gameObject, 10f);
                _isPlanted = true;
            }
        }
    }
}

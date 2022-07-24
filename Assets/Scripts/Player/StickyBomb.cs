using TF2Jam.Audio;
using TF2Jam.SO;
using UnityEngine;

namespace TF2Jam.Player
{
    public class StickyBomb : MonoBehaviour
    {
        [SerializeField]
        private RocketJumpInfo _info;

        private PlayerController _owner;

        public void Init(PlayerController owner)
        {
            _owner = owner;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var rb = GetComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
        }

        public void Explodes()
        {
            var contact = (Vector2)transform.position;
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
    }
}

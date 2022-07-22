using System.Linq;
using TF2Jam.Player;
using TF2Jam.SO;
using UnityEngine;

namespace TF2Jam
{
    public class Bullet : MonoBehaviour
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
            var contact = collision.contacts.First().point;
            var force = Vector2.Distance(contact, _owner.transform.position);
            if (force < _info.RocketImpactMaxDistance)
            {
                var propForce = (1f - force / _info.RocketImpactMaxDistance) * _info.RocketPropulsionForce;
                _owner.AddPropulsionForce(propForce, (Vector2)_owner.transform.position - contact);
            }
            Destroy(gameObject);
        }
    }
}

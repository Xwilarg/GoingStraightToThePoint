using UnityEngine;

namespace TF2Jam.Player
{
    public class Spawn : MonoBehaviour
    {
        [SerializeField]
        private GameObject _playerPrefab;

        private void Awake()
        {
            Instantiate(_playerPrefab, transform.position, Quaternion.identity);
        }
    }
}

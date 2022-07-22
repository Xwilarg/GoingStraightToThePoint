using UnityEngine;

namespace TF2Jam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        public float Speed;

        public float RocketSpeed;

        public float JumpForce;

        public GameObject RocketPrefab;
    }
}

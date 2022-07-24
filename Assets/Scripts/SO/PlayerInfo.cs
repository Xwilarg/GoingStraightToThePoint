using UnityEngine;

namespace TF2Jam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [Tooltip("Player speed")]
        public float Speed;

        [Tooltip("Rocket speed")]
        public float RocketSpeed;
        public float StickySpeed;

        [Tooltip("Force applied when jumping")]
        public float JumpForce;

        [Tooltip("Distance between player and floor to allow jump")]
        public float FloorDistanceForJump;

        [Tooltip("Prefab used when firing a rocket")]
        public GameObject RocketPrefab;
        public GameObject StickyPrefab;

        [Tooltip("Time in seconds between 2 shoots")]
        public float ReloadTime;

        [Header("Class info")]
        public RuntimeAnimatorController DemomanAnim;
        public RuntimeAnimatorController SoldierAnim;
    }
}

using UnityEngine;

namespace TF2Jam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/RocketJumpInfo", fileName = "RocketJumpInfo")]
    public class RocketJumpInfo : ScriptableObject
    {
        public float RocketImpactMaxDistance;
        public float RocketPropulsionForce;

        public GameObject ExplosionPrefab;
    }
}

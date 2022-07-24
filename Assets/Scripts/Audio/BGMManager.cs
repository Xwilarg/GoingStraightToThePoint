using TF2Jam.Persistency;
using UnityEngine;

namespace TF2Jam.Audio
{
    public class BGMManager : MonoBehaviour
    {
        private void Awake()
        {
            if (!PersistencyManager.Instance.IsBGMActive)
            {
                GetComponent<AudioSource>().gameObject.SetActive(false);
            }
        }
    }
}

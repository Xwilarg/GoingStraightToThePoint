using TF2Jam.Persistency;
using UnityEngine;

namespace TF2Jam.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        private AudioSource _source;

        private void Awake()
        {
            Instance = this;
            _source = GetComponent<AudioSource>();
        }

        public void PlayClip(AudioClip clip)
        {
            if (PersistencyManager.Instance.IsSoundsActive)
            {
                _source.PlayOneShot(clip);
            }
        }
    }
}

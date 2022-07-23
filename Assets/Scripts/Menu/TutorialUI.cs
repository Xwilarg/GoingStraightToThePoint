using TF2Jam.Persistency;
using UnityEngine;

namespace TF2Jam.Menu
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField]
        private string _level;

        private void Awake()
        {
            var data = PersistencyManager.Instance.GetLevelData(_level);
            if (!data.IsUnlocked)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
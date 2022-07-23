using TMPro;
using UnityEngine;

namespace TF2Jam.Menu
{
    public class VictoryMenu : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _timerText;

        public void Init(float timer)
        {
            _timerText.text = $"{timer:0.00}";
        }
    }
}

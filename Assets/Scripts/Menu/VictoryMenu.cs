using TF2Jam.Objective;
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
            _timerText.text = ObjectiveUI.Instance.IsDistLevel ? $"{timer:0.0m}" : $"{timer:0.00}";
        }
    }
}

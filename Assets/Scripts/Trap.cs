using UnityEngine;

namespace TF2Jam.Player
{
    public class Trap : MonoBehaviour
    {
        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                collision.collider.GetComponent<PlayerController>().ResetPos();
            }
        }
    }
}

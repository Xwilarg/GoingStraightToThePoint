﻿using UnityEngine;

namespace TF2Jam
{
    public class Bullet : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Destroy(gameObject);
        }
    }
}

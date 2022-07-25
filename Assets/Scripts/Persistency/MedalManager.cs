using System.Collections.Generic;
using UnityEngine;

namespace TF2Jam.Persistency
{
    public static class MedalManager
    {
        public static Dictionary<string, (float Easy, float Hard)> Medals = new()
        {
            { "Tutorial1", (12f, 12f) },
            { "Tutorial2", (15f, 15f) },
            { "1-1", (4.5f, 5f) },
            { "1-2", (4.5f, 4.5f) },
            { "1-3", (2.5f, 2.5f) },
            { "2-1", (7f, 7.5f) },
            { "2-2", (10f, 13f) },
            { "2-3", (12f, 13f) },
            { "3-1", (8f, 11.5f) },
            { "3-2", (10.5f, 10.5f) },
            { "3-3", (15f, 15f) },
            { "4-1", (-1f, -1f) },
        };

        public static int GetSilver(float reference)
            => Mathf.CeilToInt(reference * 50f / 100f);
    }
}

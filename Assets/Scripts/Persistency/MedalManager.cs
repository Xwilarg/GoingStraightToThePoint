using System.Collections.Generic;
using UnityEngine;

namespace TF2Jam.Persistency
{
    public static class MedalManager
    {
        public static Dictionary<string, (float Easy, float Hard)> Medals = new()
        {
            { "Tutorial1", (12f, 12f) },
            { "Tutorial2", (12f, 12f) },
            { "1-1", (4.5f, 5f) },
            { "1-2", (5f, 5f) },
            { "1-3", (3f, 3f) },
            { "2-1", (7f, 8f) },
            { "2-2", (10f, 10f) },
            { "2-3", (12f, 12f) },
            { "3-1", (8f, 11.5f) },
            { "3-2", (11f, 11f) },
        };

        public static int GetSilver(float reference)
            => Mathf.FloorToInt(reference * 30f / 100f);
    }
}

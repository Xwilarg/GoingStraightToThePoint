using System.Collections.Generic;

namespace TF2Jam.Persistency
{
    public static class MedalManager
    {
        public static Dictionary<string, int> Medals = new()
        {
            { "Tutorial1", 12 },
            { "Tutorial2", 12 },
            { "1-1", 5 },
            { "1-2", 5 },
            { "1-3", 3 },
            { "2-1", 7 },
            { "2-2", 10 },
            { "2-3", 12 },
            { "3-1", 8 },
            { "3-2", 11 },
        };
    }
}

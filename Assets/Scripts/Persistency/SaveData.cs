using System.Collections.Generic;

namespace TF2Jam.Persistency
{
    public class SaveData
    {
        public Dictionary<string, LevelData> Levels = new()
        {
            {
                "Tutorial1", new()
                {
                    IsUnlocked = true
                }
            },
            {
                "1-1", new()
                {
                    IsUnlocked = true
                }
            }
        };
        public bool DidUnlockDemoman = false;
    }

    public class LevelData
    {
        public bool IsUnlocked { set; get; } = false;
        public bool IsHardModeUnlocked { set; get; } = false;
        public float BestTime { set; get; } = -1f;
        public float BestHardTime { set; get; } = -1f;
    }
}

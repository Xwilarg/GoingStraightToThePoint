using Newtonsoft.Json;
using System.IO;
using System.Text;
using TF2Jam.Player;
using UnityEngine;

namespace TF2Jam.Persistency
{
    public class PersistencyManager
    {
        private static PersistencyManager _instance;

        public static PersistencyManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new()
                    {
                        _path = $"{Application.persistentDataPath}/data.bin"
                    };
                    _instance.Load();
                }
                return _instance;
            }
        }

        public PlayerClass CurrentClass { set; get; } = PlayerClass.Soldier;

        private void Load()
        {
            if (File.Exists(_path))
            {
                _saveData = JsonConvert.DeserializeObject<SaveData>(Encoding.UTF8.GetString(File.ReadAllBytes(_path)));
            }
            else
            {
                _saveData = new();
            }
        }

        public void Save()
        {
            File.WriteAllBytes(_path, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_saveData)));
        }

        public LevelData GetLevelData(string key)
        {
            return _saveData.Levels.ContainsKey(key) ? _saveData.Levels[key] : new();
        }

        private void UnlockLevel(string key)
        {
            if (!_saveData.Levels.ContainsKey(key))
            {
                _saveData.Levels.Add(key, new()
                {
                    IsUnlocked = true
                });
            }
        }

        public void ResetAll()
        {
            _saveData = new();
            Save();
        }

        public void FinishLevel(string key, float time)
        {
            var data = GetLevelData(key.EndsWith('H') ? key[..^1] : key);
            if (key == "1-3")
            {
                UnlockLevel("Tutorial2");
            }
            if (!key.StartsWith("Tutorial"))
            {
                var isHard = key.EndsWith('H');
                if (isHard)
                {
                    if (data.BestHardTime < 0f || time < data.BestHardTime)
                    {
                        data.BestHardTime = time;
                    }
                }
                else
                {
                    if (data.BestTime < 0f || time < data.BestTime)
                    {
                        data.BestTime = time;
                    }
                    data.IsHardModeUnlocked = true;
                    var world = int.Parse($"{key[0]}");
                    var level = int.Parse($"{key[2]}");

                    if (level < 3)
                    {
                        UnlockLevel($"{world}-{level + 1}");
                    }
                    else
                    {
                        UnlockLevel($"{world + 1}-{1}");
                    }
                }
            }
            else
            {
                data.BestTime = time;
            }
            Save();
        }

        public bool IsBGMActive { set; get; } = true;
        public bool IsSoundsActive { set; get; } = true;

        public bool AllowAdditionalClasses { set; get; }

        private SaveData _saveData;
        private string _path;
    }
}

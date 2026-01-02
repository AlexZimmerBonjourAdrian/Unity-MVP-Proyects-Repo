using System;
using System.Collections.Generic;
using UnityEngine; // For PlayerPrefs and JsonUtility
using System.Linq;

namespace HorrorEngine.Events
{
    [Serializable]
    public class FlagData
    {
        public List<string> flagNames = new List<string>();
        public List<bool> flagValues = new List<bool>();
        
        public void FromDictionary(Dictionary<string, bool> flags)
        {
            flagNames.Clear();
            flagValues.Clear();
            foreach (var flag in flags)
            {
                flagNames.Add(flag.Key);
                flagValues.Add(flag.Value);
            }
        }
        
        public Dictionary<string, bool> ToDictionary()
        {
            var result = new Dictionary<string, bool>();
            for (int i = 0; i < flagNames.Count; i++)
            {
                if (i < flagValues.Count)
                {
                    result[flagNames[i]] = flagValues[i];
                }
            }
            return result;
        }
    }

    public static class CFlagManager
    {
        private static readonly Dictionary<string, bool> flags = new Dictionary<string, bool>();

        public static event Action<string, bool> OnFlagChanged;

        public static void SetFlag(string flagName, bool value)
        {
            if (string.IsNullOrWhiteSpace(flagName))
            {
                Debug.LogWarning("Flag name cannot be null or whitespace.");
                return;
            }

            if (flags.TryGetValue(flagName, out var currentValue) && currentValue == value)
            {
                return; // No change, no need to trigger event
            }

            flags[flagName] = value;
            OnFlagChanged?.Invoke(flagName, value); // Trigger event
        }

        public static bool GetFlag(string flagName)
        {
            return flags.TryGetValue(flagName, out var value) && value;
        }

        public static void ClearFlag(string flagName)
        {
            if (flags.Remove(flagName))
            {
                OnFlagChanged?.Invoke(flagName, false); // Trigger event for removal
            }
        }

        public static void ClearAllFlags()
        {
            flags.Clear();
        }

        public static void ResetFlagsOnLevelRestart()
        {
            ClearAllFlags();
        }

        public static void SaveFlags()
        {
            try
            {
                var flagData = new FlagData();
                flagData.FromDictionary(flags);
                string json = JsonUtility.ToJson(flagData);
                PlayerPrefs.SetString("FlagsState", json);
                PlayerPrefs.Save();
                Debug.Log($"Flags saved successfully: {json}"); // Mensaje de depuración
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to save flags: {ex.Message}");
            }
        }

        public static void LoadFlags()
        {
            if (PlayerPrefs.HasKey("FlagsState"))
            {
                string json = PlayerPrefs.GetString("FlagsState");
                Debug.Log($"Flags loaded from PlayerPrefs: {json}"); // Mensaje de depuración
                try
                {
                    var flagData = JsonUtility.FromJson<FlagData>(json);
                    if (flagData != null)
                    {
                        var loadedFlags = flagData.ToDictionary();
                        foreach (var flag in loadedFlags)
                        {
                            flags[flag.Key] = flag.Value;
                        }
                        Debug.Log("Flags successfully deserialized and loaded."); // Mensaje de depuración
                    }
                    else
                    {
                        Debug.LogWarning("Loaded flags are null after deserialization.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Failed to load flags: {ex.Message}");
                }
            }
            else
            {
                Debug.LogWarning("No FlagsState found in PlayerPrefs.");
            }
        }
    }
}

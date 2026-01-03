using System.Collections.Generic;
using UnityEngine;

namespace RolEngine
{
    /// <summary>
    /// Implementación de persistencia usando PlayerPrefs.
    /// Simple y funcional para la mayoría de casos.
    /// </summary>
    public class PlayerPrefsStatPersistence : IStatPersistence
    {
        private const string STATS_KEY_PREFIX = "RolEngine_Stat_";
        private const string HAS_DATA_KEY = "RolEngine_HasData";

        public void SaveStats(Dictionary<string, int> stats)
        {
            foreach (var stat in stats)
            {
                PlayerPrefs.SetInt(STATS_KEY_PREFIX + stat.Key, stat.Value);
            }
            PlayerPrefs.SetInt(HAS_DATA_KEY, 1);
            PlayerPrefs.Save();
        }

        public Dictionary<string, int> LoadStats()
        {
            Dictionary<string, int> loadedStats = new Dictionary<string, int>();

            if (!HasSavedData())
            {
                return loadedStats;
            }

            // Cargar todas las stats guardadas
            string[] statNames = { "Sanity", "Charm", "Wits", "Composure", "Empathy" };
            
            foreach (var statName in statNames)
            {
                string key = STATS_KEY_PREFIX + statName;
                if (PlayerPrefs.HasKey(key))
                {
                    loadedStats[statName] = PlayerPrefs.GetInt(key, 5);
                }
            }

            return loadedStats;
        }

        public bool HasSavedData()
        {
            return PlayerPrefs.GetInt(HAS_DATA_KEY, 0) == 1;
        }

        public void ClearSavedData()
        {
            string[] statNames = { "Sanity", "Charm", "Wits", "Composure", "Empathy" };
            
            foreach (var statName in statNames)
            {
                PlayerPrefs.DeleteKey(STATS_KEY_PREFIX + statName);
            }
            
            PlayerPrefs.DeleteKey(HAS_DATA_KEY);
            PlayerPrefs.Save();
        }
    }
}


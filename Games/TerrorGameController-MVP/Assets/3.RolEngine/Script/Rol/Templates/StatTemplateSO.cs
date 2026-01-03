using System.Collections.Generic;
using UnityEngine;
using RolEngine;

namespace RolEngine
{
    /// <summary>
    /// ScriptableObject para templates de estadísticas.
    /// Permite crear templates desde el editor sin modificar código.
    /// </summary>
    [CreateAssetMenu(fileName = "New Stat Template", menuName = "RolEngine/Stat Template")]
    public class StatTemplateSO : ScriptableObject, IStatTemplate<CMICILSPSystem.Stats>
    {
        [Header("Template Configuration")]
        [SerializeField] private string templateName = "New Template";

        [Header("Stats")]
        [Range(1, 10)]
        [SerializeField] private int sanity = 5;
        [Range(1, 10)]
        [SerializeField] private int charm = 5;
        [Range(1, 10)]
        [SerializeField] private int wits = 5;
        [Range(1, 10)]
        [SerializeField] private int composure = 5;
        [Range(1, 10)]
        [SerializeField] private int empathy = 5;

        public string Name => templateName;

        public Dictionary<CMICILSPSystem.Stats, int> BaseStats => new Dictionary<CMICILSPSystem.Stats, int>
        {
            { CMICILSPSystem.Stats.Sanity, sanity },
            { CMICILSPSystem.Stats.Charm, charm },
            { CMICILSPSystem.Stats.Wits, wits },
            { CMICILSPSystem.Stats.Composure, composure },
            { CMICILSPSystem.Stats.Empathy, empathy }
        };

        /// <summary>
        /// Convierte este ScriptableObject a un StatTemplate (para compatibilidad)
        /// </summary>
        public CMICILSPSystem.StatTemplate ToStatTemplate()
        {
            return new CMICILSPSystem.StatTemplate(templateName, BaseStats);
        }
    }
}


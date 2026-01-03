using UnityEngine;

namespace RetroFPS
{
    /// <summary>
    /// Decorator Pattern - Decorator para items dañados.
    /// Reduce la efectividad del item y cambia su apariencia.
    /// </summary>
    public class DamagedItemDecorator : ItemDecorator
    {
        private float damageModifier;
        private float effectivenessModifier;

        /// <summary>
        /// Constructor para decorar un item como dañado
        /// </summary>
        /// <param name="item">Item a decorar</param>
        /// <param name="damageLevel">Nivel de daño (0.0 = completamente roto, 1.0 = sin daño)</param>
        public DamagedItemDecorator(IItem item, float damageLevel = 0.5f)
            : base(item)
        {
            // Clamp damage level entre 0.1 y 0.9
            damageLevel = Mathf.Clamp(damageLevel, 0.1f, 0.9f);

            damageModifier = damageLevel;
            effectivenessModifier = damageLevel;

            LogDebug($"Item damaged with modifier: {damageModifier}");
        }

        #region IItem Overrides

        public override string Name => $"{wrappedItem.Name} (Dañado)";

        public override string Description
        {
            get
            {
                string damageDescription = GetDamageDescription();
                return $"{wrappedItem.Description} {damageDescription}";
            }
        }

        public override Sprite Icon
        {
            get
            {
                // TODO: Retornar un icono modificado (más oscuro, roto, etc.)
                // Por ahora retornamos el icono original
                return wrappedItem.Icon;
            }
        }

        public override void Use()
        {
            LogDebug("Using damaged item - effectiveness reduced");

            // Aplicar penalty de daño antes de usar
            OnDamagePenalty();

            // Usar el item con efectividad reducida
            base.Use();

            // Chance de que el item se rompa más
            if (Random.value < GetBreakChance())
            {
                IncreaseDamage();
            }
        }

        #endregion

        #region Hook Methods

        protected override void OnBeforeUse()
        {
            // Aplicar efectos visuales de daño (partículas, sonidos, etc.)
            PlayDamageEffect();
        }

        protected override void OnAfterUse()
        {
            // Posible fallo adicional
            if (Random.value < 0.1f) // 10% chance
            {
                LogDebug("Item failed due to damage!");
                // TODO: Implementar fallo del item
            }
        }

        #endregion

        #region Factory Method

        protected override ItemDecorator CreateDecorator(IItem item)
        {
            return new DamagedItemDecorator(item, damageModifier);
        }

        #endregion

        #region Damage Logic

        /// <summary>
        /// Aumenta el daño del item (lo hace menos efectivo)
        /// </summary>
        public void IncreaseDamage(float amount = 0.1f)
        {
            damageModifier = Mathf.Max(0.1f, damageModifier - amount);
            effectivenessModifier = damageModifier;

            LogDebug($"Damage increased. New modifier: {damageModifier}");

            // Si el item está muy dañado, podría romperse completamente
            if (damageModifier <= 0.2f)
            {
                LogDebug("Item is critically damaged!");
            }
        }

        /// <summary>
        /// Repara parcialmente el item
        /// </summary>
        public void Repair(float amount = 0.2f)
        {
            damageModifier = Mathf.Min(1.0f, damageModifier + amount);
            effectivenessModifier = damageModifier;

            LogDebug($"Item repaired. New modifier: {damageModifier}");
        }

        /// <summary>
        /// Obtiene el modificador de daño actual
        /// </summary>
        public float DamageModifier => damageModifier;

        /// <summary>
        /// Obtiene el modificador de efectividad actual
        /// </summary>
        public float EffectivenessModifier => effectivenessModifier;

        /// <summary>
        /// Verifica si el item está roto
        /// </summary>
        public bool IsBroken => damageModifier <= 0.1f;

        #endregion

        #region Private Methods

        private string GetDamageDescription()
        {
            if (damageModifier > 0.8f)
                return "\n[Ligeramente dañado]";
            else if (damageModifier > 0.6f)
                return "\n[Moderadamente dañado]";
            else if (damageModifier > 0.4f)
                return "\n[Gravemente dañado]";
            else if (damageModifier > 0.2f)
                return "\n[Críticamente dañado]";
            else
                return "\n[ROTO - Inutilizable]";
        }

        private float GetBreakChance()
        {
            // Mayor daño = mayor chance de romperse más
            return (1f - damageModifier) * 0.3f; // Máximo 30% chance
        }

        private void OnDamagePenalty()
        {
            // Aplicar penalty basado en el daño
            // Esto podría afectar stats como daño, precisión, etc.
            float penalty = 1f - effectivenessModifier;

            // TODO: Aplicar penalty al sistema relevante
            // Ejemplo: reducir daño de arma, precisión, etc.
            LogDebug($"Applying damage penalty: {penalty * 100}%");
        }

        private void PlayDamageEffect()
        {
            // TODO: Reproducir efectos visuales/sonoros de daño
            // Ejemplo: partículas de óxido, sonido de item roto, etc.
            LogDebug("Playing damage effect");
        }

        private void LogDebug(string message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[DamagedItemDecorator: {wrappedItem.Name}] {message}");
#endif
        }

        #endregion
    }
}

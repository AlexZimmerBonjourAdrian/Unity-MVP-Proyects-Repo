using UnityEngine;
using System.Collections.Generic;

namespace RetroFPS
{
    /// <summary>
    /// Decorator Pattern - Decorator para items encantados.
    /// Agrega bonificaciones y efectos especiales a los items.
    /// </summary>
    public class EnchantedItemDecorator : ItemDecorator
    {
        public enum EnchantmentType
        {
            FireDamage,
            IceDamage,
            LightningDamage,
            PoisonDamage,
            Healing,
            SpeedBoost,
            DamageBoost,
            AccuracyBoost,
            DurabilityBoost,
            LuckBoost
        }

        private EnchantmentType enchantmentType;
        private int enchantmentLevel;
        private Color enchantmentColor;

        // Cache de colores por tipo de encantamiento
        private static readonly Dictionary<EnchantmentType, Color> EnchantmentColors = new Dictionary<EnchantmentType, Color>
        {
            { EnchantmentType.FireDamage, new Color(1f, 0.3f, 0f) },     // Naranja rojizo
            { EnchantmentType.IceDamage, new Color(0.3f, 0.8f, 1f) },    // Azul hielo
            { EnchantmentType.LightningDamage, new Color(1f, 1f, 0.3f) }, // Amarillo
            { EnchantmentType.PoisonDamage, new Color(0.3f, 1f, 0.3f) },  // Verde
            { EnchantmentType.Healing, new Color(1f, 0.5f, 1f) },        // Rosa
            { EnchantmentType.SpeedBoost, new Color(1f, 1f, 1f) },       // Blanco
            { EnchantmentType.DamageBoost, new Color(1f, 0f, 0f) },      // Rojo
            { EnchantmentType.AccuracyBoost, new Color(0f, 1f, 0f) },    // Verde
            { EnchantmentType.DurabilityBoost, new Color(0.5f, 0.5f, 0.5f) }, // Gris
            { EnchantmentType.LuckBoost, new Color(1f, 0.8f, 0f) }       // Dorado
        };

        /// <summary>
        /// Constructor para decorar un item con encantamiento
        /// </summary>
        /// <param name="item">Item a encantar</param>
        /// <param name="type">Tipo de encantamiento</param>
        /// <param name="level">Nivel del encantamiento (1-5)</param>
        public EnchantedItemDecorator(IItem item, EnchantmentType type, int level = 1)
            : base(item)
        {
            enchantmentType = type;
            enchantmentLevel = Mathf.Clamp(level, 1, 5);
            enchantmentColor = EnchantmentColors[type];

            LogDebug($"Item enchanted with {enchantmentType} level {enchantmentLevel}");
        }

        #region IItem Overrides

        public override string Name => $"{wrappedItem.Name} +{enchantmentLevel}";

        public override string Description
        {
            get
            {
                string enchantmentDesc = GetEnchantmentDescription();
                return $"{wrappedItem.Description}\n{enchantmentDesc}";
            }
        }

        public override Sprite Icon
        {
            get
            {
                // TODO: Retornar un icono con efecto de encantamiento (brillo, partículas, etc.)
                // Por ahora retornamos el icono original
                return wrappedItem.Icon;
            }
        }

        public override void Use()
        {
            LogDebug($"Using enchanted item with {enchantmentType}");

            // Aplicar efectos de encantamiento
            ApplyEnchantmentEffect();

            // Usar el item normalmente
            base.Use();

            // Efectos adicionales post-uso
            ApplyPostUseEffects();
        }

        public override void Equip()
        {
            LogDebug($"Equipping enchanted item with {enchantmentType}");

            // Aplicar bonificaciones de encantamiento al equipar
            ApplyEnchantmentBonuses();

            base.Equip();

            // Efectos visuales de encantamiento
            PlayEnchantmentEffects();
        }

        public override void Unequip()
        {
            LogDebug($"Unequipping enchanted item");

            // Remover bonificaciones de encantamiento
            RemoveEnchantmentBonuses();

            base.Unequip();
        }

        #endregion

        #region Hook Methods

        protected override void OnBeforeUse()
        {
            // Preparar efectos de encantamiento
            PrepareEnchantmentEffects();
        }

        protected override void OnAfterUse()
        {
            // Limpiar efectos temporales
            CleanupTemporaryEffects();
        }

        protected override void OnBeforeEquip()
        {
            // Verificar compatibilidad de encantamientos
            CheckEnchantmentCompatibility();
        }

        protected override void OnAfterEquip()
        {
            // Activar efectos pasivos
            ActivatePassiveEffects();
        }

        protected override void OnBeforeUnequip()
        {
            // Preparar remoción de efectos
            PrepareEffectRemoval();
        }

        #endregion

        #region Factory Method

        protected override ItemDecorator CreateDecorator(IItem item)
        {
            return new EnchantedItemDecorator(item, enchantmentType, enchantmentLevel);
        }

        #endregion

        #region Enchantment Logic

        /// <summary>
        /// Aplica el efecto principal del encantamiento
        /// </summary>
        private void ApplyEnchantmentEffect()
        {
            float effectPower = GetEffectPower();

            switch (enchantmentType)
            {
                case EnchantmentType.FireDamage:
                    // Agregar daño de fuego al ataque
                    ApplyFireDamage(effectPower);
                    break;

                case EnchantmentType.IceDamage:
                    // Aplicar efecto de congelamiento
                    ApplyIceEffect(effectPower);
                    break;

                case EnchantmentType.LightningDamage:
                    // Cadena de rayos a múltiples objetivos
                    ApplyLightningChain(effectPower);
                    break;

                case EnchantmentType.PoisonDamage:
                    // Aplicar daño por veneno over time
                    ApplyPoisonEffect(effectPower);
                    break;

                case EnchantmentType.Healing:
                    // Curar al usuario
                    ApplyHealingEffect(effectPower);
                    break;

                // Otros tipos de encantamientos...
                default:
                    LogDebug($"Unknown enchantment effect: {enchantmentType}");
                    break;
            }
        }

        /// <summary>
        /// Aplica bonificaciones pasivas del encantamiento
        /// </summary>
        private void ApplyEnchantmentBonuses()
        {
            float bonusPower = GetBonusPower();

            switch (enchantmentType)
            {
                case EnchantmentType.SpeedBoost:
                    // Aumentar velocidad de movimiento
                    ApplySpeedBonus(bonusPower);
                    break;

                case EnchantmentType.DamageBoost:
                    // Aumentar daño base
                    ApplyDamageBonus(bonusPower);
                    break;

                case EnchantmentType.AccuracyBoost:
                    // Mejorar precisión
                    ApplyAccuracyBonus(bonusPower);
                    break;

                case EnchantmentType.DurabilityBoost:
                    // Reducir desgaste
                    ApplyDurabilityBonus(bonusPower);
                    break;

                case EnchantmentType.LuckBoost:
                    // Mejorar suerte (drops, critical hits, etc.)
                    ApplyLuckBonus(bonusPower);
                    break;

                // Otros tipos...
            }
        }

        /// <summary>
        /// Remueve las bonificaciones del encantamiento
        /// </summary>
        private void RemoveEnchantmentBonuses()
        {
            // Revertir todas las bonificaciones aplicadas
            // TODO: Implementar sistema de revertir bonificaciones
            LogDebug("Removing enchantment bonuses");
        }

        #endregion

        #region Effect Implementation (Stubs)

        // Estos métodos serían implementados con la lógica real del juego

        private void ApplyFireDamage(float power) => LogDebug($"Applying fire damage: {power}");
        private void ApplyIceEffect(float power) => LogDebug($"Applying ice effect: {power}");
        private void ApplyLightningChain(float power) => LogDebug($"Applying lightning chain: {power}");
        private void ApplyPoisonEffect(float power) => LogDebug($"Applying poison effect: {power}");
        private void ApplyHealingEffect(float power) => LogDebug($"Applying healing effect: {power}");

        private void ApplySpeedBonus(float bonus) => LogDebug($"Applying speed bonus: {bonus}");
        private void ApplyDamageBonus(float bonus) => LogDebug($"Applying damage bonus: {bonus}");
        private void ApplyAccuracyBonus(float bonus) => LogDebug($"Applying accuracy bonus: {bonus}");
        private void ApplyDurabilityBonus(float bonus) => LogDebug($"Applying durability bonus: {bonus}");
        private void ApplyLuckBonus(float bonus) => LogDebug($"Applying luck bonus: {bonus}");

        private void PrepareEnchantmentEffects() => LogDebug("Preparing enchantment effects");
        private void CleanupTemporaryEffects() => LogDebug("Cleaning up temporary effects");
        private void CheckEnchantmentCompatibility() => LogDebug("Checking enchantment compatibility");
        private void ActivatePassiveEffects() => LogDebug("Activating passive effects");
        private void PrepareEffectRemoval() => LogDebug("Preparing effect removal");
        private void PlayEnchantmentEffects() => LogDebug("Playing enchantment effects");
        private void ApplyPostUseEffects() => LogDebug("Applying post-use effects");

        #endregion

        #region Utility Methods

        /// <summary>
        /// Obtiene el poder del efecto basado en el nivel del encantamiento
        /// </summary>
        private float GetEffectPower()
        {
            return enchantmentLevel * 0.2f; // 20% por nivel
        }

        /// <summary>
        /// Obtiene el poder de la bonificación basada en el nivel del encantamiento
        /// </summary>
        private float GetBonusPower()
        {
            return enchantmentLevel * 0.15f; // 15% por nivel
        }

        /// <summary>
        /// Obtiene la descripción del encantamiento
        /// </summary>
        private string GetEnchantmentDescription()
        {
            string levelStars = new string('+', enchantmentLevel);
            return $"[Encantado: {enchantmentType} {levelStars}]";
        }

        /// <summary>
        /// Obtiene el tipo de encantamiento
        /// </summary>
        public EnchantmentType Type => enchantmentType;

        /// <summary>
        /// Obtiene el nivel del encantamiento
        /// </summary>
        public int Level => enchantmentLevel;

        /// <summary>
        /// Obtiene el color del encantamiento
        /// </summary>
        public Color EnchantmentColor => enchantmentColor;

        /// <summary>
        /// Mejora el nivel del encantamiento
        /// </summary>
        public void UpgradeEnchantment(int levels = 1)
        {
            enchantmentLevel = Mathf.Min(5, enchantmentLevel + levels);
            LogDebug($"Enchantment upgraded to level {enchantmentLevel}");
        }

        /// <summary>
        /// Verifica si el encantamiento puede ser mejorado
        /// </summary>
        public bool CanUpgrade()
        {
            return enchantmentLevel < 5;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Obtiene un encantamiento aleatorio
        /// </summary>
        public static EnchantmentType GetRandomEnchantment()
        {
            var values = System.Enum.GetValues(typeof(EnchantmentType));
            return (EnchantmentType)values.GetValue(Random.Range(0, values.Length));
        }

        /// <summary>
        /// Obtiene el color de un tipo de encantamiento
        /// </summary>
        public static Color GetEnchantmentColor(EnchantmentType type)
        {
            return EnchantmentColors.TryGetValue(type, out Color color) ? color : Color.white;
        }

        #endregion

        private void LogDebug(string message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[EnchantedItemDecorator: {wrappedItem.Name}] {message}");
#endif
        }
    }
}

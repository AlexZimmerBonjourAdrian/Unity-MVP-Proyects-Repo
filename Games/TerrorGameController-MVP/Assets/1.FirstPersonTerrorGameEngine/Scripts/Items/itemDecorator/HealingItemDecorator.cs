using UnityEngine;

// Decorador concreto: Añade un efecto de curación
public class HealingItemDecorator : ItemDecorator
{
    public int HealingAmount = 20;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Healing for {HealingAmount} HP.");
        // Aquí podrías añadir lógica para curar al jugador
    }
}

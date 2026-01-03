using UnityEngine;

namespace HorrorEngine
{
    // Decorador concreto: Añade un efecto de aumento de daño
    public class DamageBoostItemDecorator : ItemDecorator
{
    public float DamageMultiplier = 1.5f;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Boosting damage by {DamageMultiplier}x.");
        // Aquí podrías añadir lógica para aumentar el daño del jugador
    }
    }
}
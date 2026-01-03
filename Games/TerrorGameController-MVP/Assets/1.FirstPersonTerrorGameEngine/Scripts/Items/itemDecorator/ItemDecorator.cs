using UnityEngine;

namespace HorrorEngine
{
    // Decorador abstracto para los ítems
    public abstract class ItemDecorator : MonoBehaviour, IItem
{
    protected IItem _item;

    public void SetItem(IItem item)
    {
        _item = item;
    }

    public virtual void Use()
    {
        _item?.Use();
    }
}

// Ejemplo de implementación concreta de un decorador
public class DamageBoostDecorator : ItemDecorator
{
    private float _damageMultiplier = 2.0f;

    public override void Use()
    {
        base.Use();
        ApplyDamageBoost();
    }

    private void ApplyDamageBoost()
    {
        Debug.Log($"Applying damage boost with multiplier: {_damageMultiplier}");
        // Lógica para aplicar el aumento de daño
    }
    }
}

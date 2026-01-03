using UnityEngine;

namespace HorrorEngine
{
    public class ItemManager : MonoBehaviour
{
    void Start()
    {
        // Crear un ítem base
        IItem baseItem = new BaseItem();

        // Añadir decoradores dinámicamente
        HealingItemDecorator healingItem = new GameObject("HealingItem").AddComponent<HealingItemDecorator>();
        healingItem.SetItem(baseItem);

        DamageBoostItemDecorator damageBoostItem = new GameObject("DamageBoostItem").AddComponent<DamageBoostItemDecorator>();
        damageBoostItem.SetItem(healingItem);

        // Usar el ítem con todos los decoradores aplicados
        damageBoostItem.Use();
    }
    }
}

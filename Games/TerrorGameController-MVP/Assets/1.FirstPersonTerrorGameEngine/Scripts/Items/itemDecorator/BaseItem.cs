using UnityEngine;

namespace HorrorEngine
{
    public class BaseItem : MonoBehaviour, IItem, Iinteract
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsUsable { get; set; }

    public virtual void Oninteract()
    {
         Debug.Log($"Interacting with item: {Name}");
    }

    public virtual void Use()
    {
        if (IsUsable)
        {
            Debug.Log($"Using item: {Name}");
        }
        else
        {
            Debug.Log($"Item {Name} is not usable.");
        }
    }
    }
}

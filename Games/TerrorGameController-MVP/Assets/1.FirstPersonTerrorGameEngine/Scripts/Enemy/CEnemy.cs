using UnityEngine;

namespace HorrorEngine
{
    public abstract class CEnemy : MonoBehaviour
{
    protected float currentHealth = 100;
    protected float lastAttackTime;
    protected bool isDead = false;

    public float maxHealth = 100f; // Valor máximo de salud

    public  abstract void DiscountLife(float damage);
    public abstract void Die();
   
    }
}

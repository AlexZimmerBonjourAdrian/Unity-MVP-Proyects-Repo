using UnityEngine;
using System.Collections;

namespace HorrorEngine
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }

        [Header("Player Settings")]
        public float moveSpeed = 5f;
        public float sprintSpeed = 8f;

        public int maxHealth = 100;

        [Header("References")]
        public Camera playerCamera;
        public LayerMask interactableLayer;

        private int currentHealth;
        private Coroutine healingCoroutine;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            currentHealth = maxHealth;
        }

        private void StartHealingOverTime()
        {
            if (healingCoroutine == null)
            {
                healingCoroutine = StartCoroutine(HealOverTime());
            }
        }

        private void StopHealingOverTime()
        {
            if (healingCoroutine != null)
            {
                StopCoroutine(healingCoroutine);
                healingCoroutine = null;
            }
        }

        private IEnumerator HealOverTime()
        {
            yield return new WaitForSeconds(4f);

            while (currentHealth < maxHealth)
            {
                Heal(1); // Heal 1 health point per tick
                yield return new WaitForSeconds(0.5f); // Heal every 0.5 seconds
            }

            healingCoroutine = null;
        }

        public void TakeDamage(int damage)
        {
            // Verificar si se está bloqueando con melee para reducir daño
            int finalDamage = damage;
            var meleeController = GetComponent<MeleeWeaponController>();
            if (meleeController != null && meleeController.HasWeapon())
            {
                var blockBehavior = GetComponent<MeleeBlockBehavior>();
                if (blockBehavior != null && blockBehavior.IsBlocking())
                {
                    finalDamage = blockBehavior.ProcessBlockedDamage(damage);
                }
            }

            currentHealth -= finalDamage;
            StopHealingOverTime(); // Stop healing when taking damage

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                StartHealingOverTime(); // Restart healing countdown
            }
        }

        private void Die()
        {
            Debug.Log("Player has died.");
            // Add death logic here (e.g., respawn, game over screen)
        }

        public void Heal(int amount)
        {
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        }
    }
}



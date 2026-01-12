using UnityEngine;
using RetroFPS;
using System.Collections;

namespace HorrorEngine
{
    // Define el enum fuera de la clase para que sea accesible globalmente si es necesario,
    // o puedes ponerlo dentro de la clase CBodyPart si solo se usará allí.
    public enum EBodyPartType
    {
        Head = 0,
        Torso = 4,
        Arm_Left = 1,
        Arm_Right = 2,
        Leg_Left = 3,
        Leg_Right = 5,
        Other = 6// Para partes no especificadas ejemplo cola
    }


    public class CBodyPart : MonoBehaviour, IDamage
{
   
    [Tooltip("Tipo de parte del cuerpo.")]
    public EBodyPartType partType = EBodyPartType.Other; // Valor por defecto

  
    

    [Tooltip("Multiplicador de daño para esta parte del cuerpo (ej: 2 para la cabeza).")]
    public float damageMultiplier = 1.0f;
     private Animator animator;
      void Awake()
    {
        // Intentar obtener el componente Animator en los padres.
        // Asume que el Animator está en el objeto raíz del enemigo o en un padre directo.
        animator = GetComponentInParent<Animator>();

        if (animator == null)
        {
            Debug.LogError($"CBodyPart en '{gameObject.name}' no pudo encontrar un componente Animator en sus padres. Las animaciones de hit no funcionarán.", this);
        }
    }

   public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
{
    // Calcular el daño final aplicando el multiplicador de la parte del cuerpo
    float finalDamage = damage * damageMultiplier;
    
    Debug.Log($"Damage received on {partType} with multiplier {damageMultiplier}. Final damage: {finalDamage}.");

    // Intentar activar la animación si se encontró el Animator
    if (animator != null)
    {
        // Convertir el enum EBodyPartType a su valor entero
        int bodyPartIndex = (int)partType;

        // Establecer el parámetro Integer en el Animator
        // Asegúrate de que exista un parámetro Integer con este nombre en tu Animator Controller.
        animator.SetInteger("BodyPart", bodyPartIndex);
        animator.SetBool("HitPart", true);

        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        Debug.Log($"Current Animator State: {stateInfo.fullPathHash}, Normalized Time: {stateInfo.normalizedTime}");

        // Verificar si el estado es "Idle" o si la animación ha pasado el 50% de su duración
        if (stateInfo.IsName("Idle") || stateInfo.normalizedTime >= 0.5f)
        {
            Debug.Log($"Animator is in 'Idle' state or past 50% of the animation for {gameObject.name}. Hit animation will play.");
        }
        else
        {
            Debug.LogWarning($"Animator state is not 'Idle' and normalized time is <= 0.5 for {gameObject.name}. Hit animation reset.");
        }

        // Reiniciar el parámetro "HitPart" después de un breve retraso
        StartCoroutine(ResetHitPart());
    }
    else
    {
        Debug.LogError($"Animator is null for {gameObject.name}. Cannot process hit animation.");
    }

    // Notificar al CEnemyController principal para que descuente vida
    // Buscar el componente CEnemy en los padres
    CEnemy enemyController = GetComponentInParent<CEnemy>();
    if (enemyController != null)
    {
        enemyController.DiscountLife(finalDamage);
    }
}

// Corrutina para reiniciar el parámetro "HitPart"
private IEnumerator ResetHitPart()
{
    yield return new WaitForSeconds(0.1f); // Ajusta el tiempo según sea necesario
    animator.SetBool("HitPart", false);
}

    }
}



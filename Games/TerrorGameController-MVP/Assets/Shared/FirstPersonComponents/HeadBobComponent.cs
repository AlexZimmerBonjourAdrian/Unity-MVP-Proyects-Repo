using UnityEngine;

/// <summary>
/// Componente modular para sistema de headbob en controladores de primera persona.
/// Extraído de ModularFirstPersonController y adaptado para reutilización.
/// </summary>
public class HeadBobComponent : MonoBehaviour
{
    [Header("Head Bob Settings")]
    [SerializeField] private Transform joint;
    [SerializeField] private float bobSpeed = 10f;
    [SerializeField] private Vector3 bobAmount = new Vector3(0.15f, 0.05f, 0f);
    
    [Header("Sprint Settings")]
    [SerializeField] private float sprintSpeedMultiplier = 1.5f;
    
    [Header("Crouch Settings")]
    [SerializeField] private float crouchSpeedReduction = 0.5f;

    private Vector3 jointOriginalPos;
    private float timer = 0;
    
    private bool isWalking = false;
    private bool isSprinting = false;
    private bool isCrouched = false;

    private void Awake()
    {
        if (joint == null)
        {
            Debug.LogWarning("HeadBobComponent: Joint Transform no asignado. Buscando automáticamente...");
            joint = GetComponentInChildren<Transform>();
        }
        
        if (joint != null)
        {
            jointOriginalPos = joint.localPosition;
        }
        else
        {
            Debug.LogError("HeadBobComponent: No se encontró Transform para headbob. Asigna un joint en el Inspector.");
        }
    }

    private void Update()
    {
        if (joint == null) return;

        if (isWalking)
        {
            float currentBobSpeed = bobSpeed;
            
            // Calculates HeadBob speed during sprint
            if (isSprinting)
            {
                currentBobSpeed = bobSpeed * sprintSpeedMultiplier;
            }
            // Calculates HeadBob speed during crouched movement
            else if (isCrouched)
            {
                currentBobSpeed = bobSpeed * crouchSpeedReduction;
            }
            
            timer += Time.deltaTime * currentBobSpeed;
            
            // Applies HeadBob movement
            joint.localPosition = new Vector3(
                jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x,
                jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y,
                jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z
            );
        }
        else
        {
            // Resets when player stops moving
            timer = 0;
            joint.localPosition = Vector3.Lerp(
                joint.localPosition,
                jointOriginalPos,
                Time.deltaTime * bobSpeed
            );
        }
    }

    /// <summary>
    /// Establece si el jugador está caminando
    /// </summary>
    public void SetWalking(bool walking)
    {
        isWalking = walking;
    }

    /// <summary>
    /// Establece si el jugador está corriendo
    /// </summary>
    public void SetSprinting(bool sprinting)
    {
        isSprinting = sprinting;
    }

    /// <summary>
    /// Establece si el jugador está agachado
    /// </summary>
    public void SetCrouched(bool crouched)
    {
        isCrouched = crouched;
    }

    /// <summary>
    /// Obtiene la posición original del joint
    /// </summary>
    public Vector3 GetOriginalPosition()
    {
        return jointOriginalPos;
    }

    /// <summary>
    /// Resetea la posición del joint a su posición original
    /// </summary>
    public void ResetPosition()
    {
        if (joint != null)
        {
            joint.localPosition = jointOriginalPos;
            timer = 0;
        }
    }
}

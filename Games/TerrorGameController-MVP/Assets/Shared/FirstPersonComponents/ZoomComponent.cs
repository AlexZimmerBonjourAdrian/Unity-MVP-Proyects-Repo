using UnityEngine;

/// <summary>
/// Componente modular para sistema de zoom en controladores de primera persona.
/// Extraído de ModularFirstPersonController y adaptado para reutilización.
/// </summary>
public class ZoomComponent : MonoBehaviour
{
    [Header("Zoom Settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private bool enableZoom = true;
    [SerializeField] private bool holdToZoom = false;
    [SerializeField] private KeyCode zoomKey = KeyCode.Mouse1;
    [SerializeField] private float zoomFOV = 30f;
    [SerializeField] private float zoomStepTime = 5f;
    [SerializeField] private float normalFOV = 60f;

    private bool isZoomed = false;
    private bool isSprinting = false;

    private void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                playerCamera = GetComponentInChildren<Camera>();
            }
        }

        if (playerCamera != null)
        {
            normalFOV = playerCamera.fieldOfView;
        }
        else
        {
            Debug.LogError("ZoomComponent: No se encontró Camera. Asigna una cámara en el Inspector.");
        }
    }

    private void Update()
    {
        if (!enableZoom || playerCamera == null) return;

        // Cambiar estado de zoom cuando se presiona la tecla
        // Comportamiento para toggle zoom
        if (Input.GetKeyDown(zoomKey) && !holdToZoom && !isSprinting)
        {
            if (!isZoomed)
            {
                ZoomIn();
            }
            else
            {
                ZoomOut();
            }
        }

        // Comportamiento para hold to zoom
        if (holdToZoom && !isSprinting)
        {
            if (Input.GetKeyDown(zoomKey))
            {
                ZoomIn();
            }
            else if (Input.GetKeyUp(zoomKey))
            {
                ZoomOut();
            }
        }

        // Aplicar lerp del FOV para transición suave
        if (isZoomed && !isSprinting)
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomFOV, zoomStepTime * Time.deltaTime);
        }
        else if (!isZoomed && !isSprinting)
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, zoomStepTime * Time.deltaTime);
        }
    }

    /// <summary>
    /// Activa el zoom
    /// </summary>
    public void ZoomIn()
    {
        if (!isSprinting)
        {
            isZoomed = true;
        }
    }

    /// <summary>
    /// Desactiva el zoom
    /// </summary>
    public void ZoomOut()
    {
        isZoomed = false;
    }

    /// <summary>
    /// Establece si el jugador está corriendo (desactiva zoom automáticamente)
    /// </summary>
    public void SetSprinting(bool sprinting)
    {
        isSprinting = sprinting;
        if (sprinting && isZoomed)
        {
            ZoomOut();
        }
    }

    /// <summary>
    /// Verifica si el zoom está activo
    /// </summary>
    public bool IsZoomed()
    {
        return isZoomed;
    }

    /// <summary>
    /// Establece el FOV normal (usado cuando se cambia el FOV base)
    /// </summary>
    public void SetNormalFOV(float fov)
    {
        normalFOV = fov;
        if (!isZoomed)
        {
            playerCamera.fieldOfView = normalFOV;
        }
    }

    /// <summary>
    /// Obtiene el FOV normal actual
    /// </summary>
    public float GetNormalFOV()
    {
        return normalFOV;
    }
}

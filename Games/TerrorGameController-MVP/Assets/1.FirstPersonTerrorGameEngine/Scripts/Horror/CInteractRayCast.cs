using System;
using UnityEngine;
using HorrorEngine.Interfaces;

[ExecuteAlways]
[RequireComponent(typeof(CHorrorController))]
public class CInteractRayCast : MonoBehaviour
{
    private CHorrorController _controller;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask ignoreLayers; // Layers to ignore during raycasting
    [SerializeField] private bool interactionsEnabled = true; // Permite desactivar puntualmentela interacción

    public float interactionDistance = 3f; // Distancia de interacción
    public Color gizmoColor = Color.yellow; // Color del Gizmo

    // Variables para los rayos adicionales
    public float rayOffset = 0.5f; // Distancia de separación de los rayos
    public Color secondaryRayColor = Color.green; // Color de los rayos secundarios

    private void Start()
    {
        _controller = GetComponent<CHorrorController>();
        // mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!interactionsEnabled || mainCamera == null)
        {
            return;
        }
        // Rayos adicionales
        Vector3 right = mainCamera.transform.right;
        Vector3 up = mainCamera.transform.up;

        // Puntos de origen de los rayos
        Vector3[] rayOrigins = new Vector3[]
        {
            mainCamera.transform.position, // Central
            mainCamera.transform.position + up * rayOffset, // Superior
            mainCamera.transform.position - up * rayOffset, // Inferior
            mainCamera.transform.position + right * rayOffset, // Derecho
            mainCamera.transform.position - right * rayOffset // Izquierdo
        };

        // Realizar los raycasts y almacenar los resultados
        RaycastHit[] hits = new RaycastHit[rayOrigins.Length];
        for (int i = 0; i < rayOrigins.Length; i++)
        {
            Physics.Raycast(rayOrigins[i], mainCamera.transform.forward, out hits[i], interactionDistance, ~ignoreLayers);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWithObject(hits);
        }
    }

    public void SetInteractionsEnabled(bool enabled)
    {
        interactionsEnabled = enabled;
    }

    private void InteractWithObject(RaycastHit[] hits)
    {
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider != null)
            {
                Iinteract interactable = hits[i].collider.GetComponent<Iinteract>();
                if (interactable != null)
                {
                    interactable.Oninteract();
                    return; // Interactuamos solo con el primer objeto que encontremos
                }
            }
        }
    }

    void gizmos()
    {
        // Dibuja un rayo en la escena para visualizar la distancia de interacción
        Gizmos.color = gizmoColor;
        Gizmos.DrawLine(mainCamera.transform.position, mainCamera.transform.position + mainCamera.transform.forward * interactionDistance);

        // Dibuja los rayos adicionales
        Gizmos.color = secondaryRayColor;
        Vector3 right = mainCamera.transform.right;
        Vector3 up = mainCamera.transform.up;

        // Rayo superior
        Vector3 upRayOrigin = mainCamera.transform.position + up * rayOffset;
        Gizmos.DrawLine(upRayOrigin, upRayOrigin + mainCamera.transform.forward * interactionDistance);

        // Rayo inferior
        Vector3 downRayOrigin = mainCamera.transform.position - up * rayOffset;
        Gizmos.DrawLine(downRayOrigin, downRayOrigin + mainCamera.transform.forward * interactionDistance);

        // Rayo derecho
        Vector3 rightRayOrigin = mainCamera.transform.position + right * rayOffset;
        Gizmos.DrawLine(rightRayOrigin, rightRayOrigin + mainCamera.transform.forward * interactionDistance);

        // Rayo izquierdo
        Vector3 leftRayOrigin = mainCamera.transform.position - right * rayOffset;
        Gizmos.DrawLine(leftRayOrigin, leftRayOrigin + mainCamera.transform.forward * interactionDistance);
    }

    private void OnDrawGizmos()
    {
        // Llama a la función gizmos para dibujar el rayo
        gizmos();
    }
}




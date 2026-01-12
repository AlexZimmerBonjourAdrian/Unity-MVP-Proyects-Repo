using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HorrorEngine
{
    public class UIinteractiveParameter : MonoBehaviour
{
    [SerializeField] private Sprite Normal;
    [SerializeField] private Sprite Interactive;
    [SerializeField] private Image image;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rayDistance = 5f;
    [SerializeField] private LayerMask interactableLayerMask;

    private bool isCurrentlyInteractive = false;

    private void Start()
    {
        image = GetComponentInChildren<Image>();
        if (image == null)
        {
            Debug.LogError("Image component not found in children.");
        }
        else
        {
            image.sprite = Normal;
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera not assigned and no Camera tagged as MainCamera found.");
            }
        }
    }

    private void Update()
    {
        if (mainCamera != null)
        {
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            RaycastHit hit;

            bool hitInteractable = Physics.Raycast(ray, out hit, rayDistance, interactableLayerMask) && hit.collider.CompareTag("Interactable");

            if (hitInteractable != isCurrentlyInteractive)
            {
                UpdateInteractionState(hitInteractable);
                isCurrentlyInteractive = hitInteractable;
            }
        }
    }

    public void UpdateInteractionState(bool canInteract)
    {
        if (image != null)
        {
            image.sprite = canInteract ? Interactive : Normal;
        }
    }
    }
}

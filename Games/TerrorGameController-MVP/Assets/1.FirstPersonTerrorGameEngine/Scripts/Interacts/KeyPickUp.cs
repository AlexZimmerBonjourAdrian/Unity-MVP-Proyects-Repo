using UnityEngine;
using HorrorEngine.Interfaces;
using HorrorEngine.Events;
// Asegúrate de que tienes definida la interfaz Iinteract en algún lugar
// public interface Iinteract { void Oninteract(); }

[RequireComponent(typeof(Collider))] // Asegura que tenga un collider para el Raycast
public class KeyPickup : MonoBehaviour, Iinteract
{
    [Tooltip("Identificador único para esta llave (ej: 'BasementKey', 'OfficeKey')")]
    public string keyId = "DefaultKey";

    [Tooltip("ID del primer sonido a reproducir al recoger (índice en CManagerSFX.ListSFX)")]
    public int pickupSoundId1 = 0; // Ejemplo: Sonido de 'jingle'

    [Tooltip("ID del segundo sonido a reproducir al recoger (índice en CManagerSFX.ListSFX)")]
    public int pickupSoundId2 = 1; // Ejemplo: Sonido de confirmación 'item get'

    public void Oninteract()
    {
        Debug.Log($"Player picked up key: {keyId}");

        // 1. Notificar al sistema que la llave fue recogida (para CFlagManager o Inventory)
        CGameEvents.OnPickupKey.Publish(keyId);

        // 2. Publicar los eventos para reproducir los dos sonidos
        CGameEvents.OnPlaySound.Publish(pickupSoundId1);
        CGameEvents.OnPlaySound.Publish(pickupSoundId2);

        // 3. Desactivar o destruir el objeto llave
        gameObject.SetActive(false); // O Destroy(gameObject);
    }
}


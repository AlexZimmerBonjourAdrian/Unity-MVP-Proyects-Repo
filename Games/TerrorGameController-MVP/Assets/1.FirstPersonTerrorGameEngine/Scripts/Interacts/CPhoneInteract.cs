using UnityEngine;
using RetroFPS;
namespace HorrorEngine
{
    public class CPhoneInteract : MonoBehaviour, Iinteract
    {
     [SerializeField] private CSFX phoneSFX; // Referencia al script CSFX que contiene el sonido del teléfono
    private void Start()
    {
        // Busca el componente CSFX en el objeto actual
        phoneSFX = GetComponent<CSFX>();
        
        // Verifica si el componente CSFX fue encontrado
        if (phoneSFX == null)
        {
            Debug.LogWarning("CSFX component not found on this GameObject.");
        }
    }
    public void Oninteract()
    {
        // Verifica si el objeto phoneSFX no es nulo
        if (phoneSFX != null)
        {
            // Reproduce el sonido del teléfono
            phoneSFX.StopSFX();
            CManagerSFX.Inst.PlaySound(29); // Reproduce el sonido del teléfono
            
            CManagerMusic.Inst.PlayMusicBackground(74); // Reproduce el sonido de la música
        }
        else
        {
            Debug.LogWarning("phoneSFX is not assigned in the inspector.");
        }
    } 
}
}

using UnityEngine;

/// <summary>
/// Componente responsable de reproducir el sonido de disparo.
/// Requiere un componente AudioSource en el mismo GameObject.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ShootSoundPlayer : MonoBehaviour
{
    [Tooltip("El clip de audio que se reproducirá al disparar.")]
    public AudioClip shootSoundClip; // Asigna tu clip de audio aquí en el Inspector

    private AudioSource audioSource;

    void Awake()
    {
        // Obtenemos la referencia al componente AudioSource
        audioSource = GetComponent<AudioSource>();

       
    }

 
    public void PlayShootSound()
    {
        if (shootSoundClip != null && audioSource != null)
        {
          
            audioSource.PlayOneShot(shootSoundClip);
        }
        else
        {
            if (shootSoundClip == null)
            {
                Debug.LogWarning("No se ha asignado un AudioClip para el disparo.", this);
            }
            if (audioSource == null)
            {
                Debug.LogError("Falta el componente AudioSource.", this);
            }
        }
    }
}


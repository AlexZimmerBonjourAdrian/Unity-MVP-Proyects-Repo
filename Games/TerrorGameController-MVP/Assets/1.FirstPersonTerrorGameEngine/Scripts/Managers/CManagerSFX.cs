using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace HorrorEngine
{

public class CManagerSFX : MonoBehaviour
{   
    public static CManagerSFX Inst
    {
        get
        {
            if (_inst == null)
            {
                _inst = FindFirstObjectByType<CManagerSFX>();
                if (_inst == null)
                {
                    GameObject obj = new GameObject("ManagerSFX");
                    _inst = obj.AddComponent<CManagerSFX>();
                }
            }
            return _inst;
        }
    }

    private static CManagerSFX _inst;


    public void Awake()
    {
        if(_inst != null && _inst != this)
        {
            Destroy(gameObject);
            return;
        }
        _inst = this;
        ListSounds = new List<GameObject>();

    }

    public void Start()
    {
        CGameEvents.OnPlaySound.Subscribe(PlaySound);
        
        // Suscribirse a los eventos del LineView
       
    }

    private AudioSource _mainAudioSource;
    [SerializeField] public List<AudioClip> ListSFX;
    [SerializeField] public AudioMixer audioMixer;

    private List<GameObject> ListSounds;

    [SerializeField]public Dictionary<AudioClip, string> soundMap = new Dictionary<AudioClip, string>();

    public void AddSound()
    {
        GameObject soundObject = new GameObject("Sound");
        soundObject.AddComponent<AudioSource>();
        ListSounds.Add(soundObject);
    }
  
    public void PlaySound(int id)
    {
        // Buscar el AudioClip correspondiente al id
        AudioClip clip = ListSFX[id];
        AudioSource soundObject = GetComponent<AudioSource>();
        soundObject.clip = clip;
        soundObject.Play();
    }

    public void StopSFX()
    {
        foreach (GameObject sound in ListSounds)
        {
            if (sound != null)
            {
                AudioSource audioSource = sound.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.Stop();
                }
                Destroy(sound);
            }
        }
        ListSounds.Clear();
    }


     public AudioClip GetCurrentPlayingSound()
    {
        if (_mainAudioSource != null && _mainAudioSource.isPlaying)
        {
            return _mainAudioSource.clip;
        }
        // Return null if the source doesn't exist, isn't playing, or has no clip assigned while playing (less likely)
        return null;
    }

    private void Update()
    {
        DebugSounds();
    }
    // Nueva función para reproducir sonido de caracteres
    public void PlayCharacterSound(int characterIndex)
    {
        // Si no hay sonidos configurados, no hacemos nada
        if (ListSFX.Count == 0) return;

        // Usamos el índice del carácter para seleccionar un sonido de la lista
        // Si el índice es mayor que la cantidad de sonidos, usamos el módulo
        int soundIndex = characterIndex % ListSFX.Count;
        
        AudioClip clip = ListSFX[soundIndex];
        if (clip != null)
        {
            GameObject soundObject = new GameObject("CharacterSound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = 0.3f; // Volumen más bajo para sonidos de caracteres
            audioSource.Play();
            ListSounds.Add(soundObject);
        }
    }

    // Nueva función para reproducir sonidos de reacción
    public void PlayReactionSound(string reactionType)
    {
        AudioClip clip = ListSFX.Find(c => c.name == reactionType);
        if (clip != null)
        {
            GameObject soundObject = new GameObject("ReactionSound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
            ListSounds.Add(soundObject);
        }
    }

   private void DebugSounds()
    {
        // Comprueba si se presiona la tecla '1'
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 1. Leer el valor ACTUAL guardado en PlayerPrefs. Si no existe, empieza en -1 para que el primer incremento sea 0.
            //    O empieza en 0 si quieres que la primera vez que pulses '1' se reproduzca el sonido 1.
            int currentDebugIndex = PlayerPrefs.GetInt("DebugSounds", -1); // Empezar en -1 para que el primer ++ sea 0

            // 2. Incrementar el índice
            currentDebugIndex++;

            // 3. (Opcional pero recomendado) Asegurarse de que el índice no se salga de los límites de la lista
            if (ListSFX != null && ListSFX.Count > 0)
            {
                // Si quieres que vuelva al principio al llegar al final (wrap around)
                currentDebugIndex = currentDebugIndex % ListSFX.Count;

                // Si prefieres que se quede en el último sonido en lugar de volver al principio:
                // currentDebugIndex = Mathf.Min(currentDebugIndex, ListSFX.Count - 1);
            }
            else
            {
                // Si no hay sonidos, no hagas nada o resetea el índice
                 Debug.LogWarning("DebugSounds: No hay sonidos en ListSFX para depurar.");
                 currentDebugIndex = 0; // O -1 dependiendo de tu lógica inicial
                 // No guardes ni reproduzcas si no hay sonidos
                 return;
            }


            // 4. Guardar el NUEVO valor incrementado (y posiblemente ajustado) de vuelta en PlayerPrefs
            PlayerPrefs.SetInt("DebugSounds", currentDebugIndex);
            // PlayerPrefs.Save(); // Opcional: Forza el guardado inmediato (útil para depurar o si el juego puede cerrarse inesperadamente)

            // 5. Usar el nuevo índice para reproducir el sonido
            Debug.Log($"DebugSounds: Incrementado a índice {currentDebugIndex}. Reproduciendo sonido.");
            PlaySound(currentDebugIndex);
        }

        // (Opcional) Añadir una forma de resetear el contador si es necesario
        if (Input.GetKeyDown(KeyCode.Alpha0)) // Por ejemplo, con la tecla '0'
        {
             PlayerPrefs.SetInt("DebugSounds", -1); // Resetea al valor inicial (-1 o 0)
             // PlayerPrefs.Save(); // Opcional
             Debug.Log("DebugSounds: Índice reseteado a -1.");
             // Podrías reproducir el sonido 0 aquí si quieres una confirmación
             // if (ListSFX != null && ListSFX.Count > 0) PlaySound(0);
        }
    }

    public float GetDuration()
    {
        if (_mainAudioSource != null && _mainAudioSource.clip != null)
        {
            return _mainAudioSource.clip.length;
        }
        return 0f; // O un valor por defecto si no hay clip
    }
}
}

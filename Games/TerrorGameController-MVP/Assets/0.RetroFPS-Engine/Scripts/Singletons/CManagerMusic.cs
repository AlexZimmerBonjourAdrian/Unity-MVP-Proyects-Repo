using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace RetroFPS
{
    public class CManagerMusic : MonoBehaviour
    {
        // Start is called before the first frame update

        
            public static CManagerMusic Inst
        {
            get
            {
                if (_inst == null)
                {
                    GameObject obj = new GameObject("Music");
                    return obj.AddComponent<CManagerMusic>();
                }
                return _inst;

            }
        }
        private static CManagerMusic _inst;
       [SerializeField] private int idexMusic = 0;

      public void Awake()
        {
        if(_inst != null && _inst != this)
            {
                Destroy(gameObject);
                return;
            }
            //DontDestroyOnLoad(this.gameObject);
            _inst = this;
        }
        [SerializeField] public List<AudioClip> musicLists;

        [SerializeField] public AudioMixer audioMixer;

          [SerializeField] public bool IsAutoMusic= true;
    
       public void Start()
       {
        if(IsAutoMusic == true)
        {
            PlayMusicBackground(0);
        }

       }

        public void PlayMusic()
    {
        if (musicLists.Count == 0) return;

        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = musicLists[0];
        audioSource.Play();
    }

      public void StopMusic()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource!= null)
        {
            audioSource.Stop();
        }
    }

      public void PauseMusic()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource!= null)
        {
            audioSource.Pause();
        }
    }

    public void PlayMusicBackground(int id)
    {
        // Buscar el AudioClip correspondiente al id
        AudioClip clip = musicLists[id];
        AudioSource soundObject = GetComponent<AudioSource>();
        soundObject.clip = clip;
        soundObject.Play();

    }

       public void FadeIn(float duration = 1f)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource!= null)
        {
            audioSource.volume = 0f;
            StartCoroutine(FadeInCoroutine(duration));
        }
    }

    private IEnumerator FadeInCoroutine(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float volume = Mathf.Lerp(0f, 1f, timer / duration);
            GetComponent<AudioSource>().volume = volume;
            yield return null;
        }
    }
    public void FadeOut(float duration = 1f)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource!= null)
        {
            audioSource.volume = 1f;
            StartCoroutine(FadeOutCoroutine(duration));
        }
    }

    private void Update()
    {
        // Llama a la función DebugSounds en cada frame
        DebugSounds();
    }
    private IEnumerator FadeOutCoroutine(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float volume = Mathf.Lerp(1f, 0f, timer / duration);
            GetComponent<AudioSource>().volume = volume;
            yield return null;
        }
        GetComponent<AudioSource>().Stop();
    }

      private void DebugSounds()
        {
            // Comprueba si se presiona la tecla '1'
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                // 1. Leer el valor ACTUAL guardado en PlayerPrefs. Si no existe, empieza en -1 para que el primer incremento sea 0.
                //    O empieza en 0 si quieres que la primera vez que pulses '1' se reproduzca el sonido 1.
                int currentDebugIndex = PlayerPrefs.GetInt("DebugSounds", -1); // Empezar en -1 para que el primer ++ sea 0

                // 2. Incrementar el índice
                currentDebugIndex++;

                idexMusic = currentDebugIndex;

                // 3. (Opcional pero recomendado) Asegurarse de que el índice no se salga de los límites de la lista
                if (musicLists != null && musicLists.Count > 0)
                {
                    // Si quieres que vuelva al principio al llegar al final (wrap around)
                    currentDebugIndex = currentDebugIndex % musicLists.Count;

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
                PlayMusicBackground(currentDebugIndex);
            }

            // (Opcional) Añadir una forma de resetear el contador si es necesario
            if (Input.GetKeyDown(KeyCode.Alpha4)) // Por ejemplo, con la tecla '0'
            {
                 PlayerPrefs.SetInt("DebugSounds", -1); // Resetea al valor inicial (-1 o 0)
                 PlayerPrefs.Save(); 
                 Debug.Log("DebugSounds: Índice reseteado a -1.");
                 // Podrías reproducir el sonido 0 aquí si quieres una confirmación
                 // if (ListSFX != null && ListSFX.Count > 0) PlaySound(0);
            }
        }

    }
    }



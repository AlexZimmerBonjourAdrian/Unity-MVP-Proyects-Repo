using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace RetroFPS
{
    /// <summary>
    /// Ejemplo de Manager de SFX usando Template Method Pattern.
    /// Extiende BaseManager para obtener el algoritmo de inicialización estándar,
    /// mientras personaliza la lógica específica de manejo de audio.
    /// </summary>
    public class CManagerSFXTemplate : BaseManager
    {
        public static CManagerSFXTemplate Instance { get; private set; }

        // Configuración del manager
        [Header("Audio Configuration")]
        [SerializeField] private List<AudioClip> soundEffects = new List<AudioClip>();
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private int maxConcurrentSounds = 10;
        [SerializeField] private float defaultVolume = 1.0f;

        // Estado interno
        private List<GameObject> activeSoundObjects = new List<GameObject>();
        private Dictionary<string, AudioClip> soundMap = new Dictionary<string, AudioClip>();
        private ObjectPool<AudioSource> audioSourcePool;

        // ============================================
        // IMPLEMENTACIÓN DE MÉTODOS ABSTRACTOS
        // ============================================

        protected override void OnInitialize()
        {
            LogDebug("Initializing SFX Manager...");

            // Crear mapeo de sonidos por nombre
            foreach (var clip in soundEffects)
            {
                if (clip != null && !soundMap.ContainsKey(clip.name))
                {
                    soundMap[clip.name] = clip;
                    LogDebug($"Registered sound: {clip.name}");
                }
            }

            // Crear pool de AudioSources
            CreateAudioSourcePool();

            LogDebug($"SFX Manager initialized with {soundMap.Count} sounds and pool of {maxConcurrentSounds} sources");
        }

        protected override void OnSetup()
        {
            LogDebug("Setting up SFX Manager...");

            // Suscribirse a cambios de volumen
            GameObservers.SFXVolumeChanged.Attach(OnVolumeChanged);

            // Aplicar configuración inicial
            ApplyCurrentVolume();

            LogDebug("SFX Manager setup completed");
        }

        protected override void OnUpdate()
        {
            // Limpiar AudioSources que ya no están reproduciendo
            CleanupFinishedSounds();
        }

        protected override void OnCleanup()
        {
            LogDebug("Cleaning up SFX Manager...");

            // Desuscribirse de observers
            GameObservers.SFXVolumeChanged.Detach(OnVolumeChanged);

            // Detener todos los sonidos
            StopAllSounds();

            // Limpiar pool
            if (audioSourcePool != null)
            {
                audioSourcePool.Clear();
            }

            // Limpiar colecciones
            activeSoundObjects.Clear();
            soundMap.Clear();

            LogDebug("SFX Manager cleanup completed");
        }

        // ============================================
        // SOBRESCRITURA DE MÉTODOS VIRTUALES
        // ============================================

        protected override bool ValidateDependencies()
        {
            bool isValid = true;

            // Validar AudioMixer (opcional)
            if (audioMixer == null)
            {
                LogDebug("AudioMixer not assigned - using default audio routing");
            }

            // Validar que haya al menos un clip de sonido
            if (soundEffects.Count == 0)
            {
                LogWarning("No sound effects assigned - manager will have limited functionality");
            }

            return isValid;
        }

        protected override void InitializeSingleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            LogDebug("SFX Manager singleton initialized");
        }

        protected override void CleanupSingleton()
        {
            if (Instance == this)
            {
                Instance = null;
                LogDebug("SFX Manager singleton cleaned up");
            }
        }

        protected override void RegisterEvents()
        {
            // Aquí se registrarían suscripciones a EventBus si fuera necesario
            // EventBus.Subscribe<WeaponFiredEvent>(OnWeaponFired);
        }

        protected override void UnregisterEvents()
        {
            // Desuscribirse de eventos
            // EventBus.Unsubscribe<WeaponFiredEvent>(OnWeaponFired);
        }

        protected override void LoadConfiguration()
        {
            // Cargar configuración guardada (si existe)
            float savedVolume = PlayerPrefs.GetFloat("SFXVolume", defaultVolume);
            GameObservers.SFXVolumeChanged.SetValue(savedVolume);
        }

        protected override void EnableSystem()
        {
            // Habilitar cualquier componente adicional si es necesario
            LogDebug("SFX system enabled");
        }

        protected override void DisableSystem()
        {
            // Deshabilitar sonidos, pausar reproducciones, etc.
            StopAllSounds();
            LogDebug("SFX system disabled");
        }

        protected override bool ShouldRunPeriodicChecks()
        {
            return true; // Ejecutar verificaciones cada frame
        }

        protected override void PerformPeriodicChecks()
        {
            // Verificar que no excedamos el límite de sonidos concurrentes
            if (activeSoundObjects.Count > maxConcurrentSounds * 1.5f)
            {
                LogWarning($"Too many active sound objects: {activeSoundObjects.Count}. Consider increasing maxConcurrentSounds.");
            }

            // Verificar estado del pool
            if (audioSourcePool != null && !ValidateAudioSourcePool())
            {
                LogWarning("Audio source pool validation failed - recreating pool");
                RecreateAudioSourcePool();
            }
        }

        // ============================================
        // MÉTODOS ESPECÍFICOS DEL MANAGER
        // ============================================

        /// <summary>
        /// Reproduce un sonido por nombre
        /// </summary>
        public void PlaySound(string soundName, float volume = -1f)
        {
            if (string.IsNullOrEmpty(soundName))
            {
                LogWarning("Cannot play sound: sound name is null or empty");
                return;
            }

            if (!soundMap.TryGetValue(soundName, out AudioClip clip))
            {
                LogWarning($"Sound '{soundName}' not found in sound map");
                return;
            }

            PlaySound(clip, volume);
        }

        /// <summary>
        /// Reproduce un AudioClip directamente
        /// </summary>
        public void PlaySound(AudioClip clip, float volume = -1f)
        {
            if (clip == null)
            {
                LogWarning("Cannot play sound: AudioClip is null");
                return;
            }

            if (audioSourcePool == null)
            {
                LogError("Cannot play sound: audio source pool not initialized");
                return;
            }

            // Obtener AudioSource del pool
            AudioSource source = audioSourcePool.Get();
            if (source == null)
            {
                LogWarning("Cannot play sound: no available audio sources in pool");
                return;
            }

            // Configurar y reproducir
            source.clip = clip;
            source.volume = volume >= 0 ? volume : defaultVolume;
            source.Play();

            // Crear objeto contenedor para seguimiento
            GameObject soundObject = CreateChildObject($"Sound_{clip.name}");
            soundObject.AddComponent<AudioSource>().clip = clip;

            activeSoundObjects.Add(soundObject);

            // Programar retorno al pool cuando termine
            StartCoroutine(ReturnToPoolWhenFinished(soundObject, source, clip.length));

            LogDebug($"Playing sound: {clip.name}");
        }

        /// <summary>
        /// Detiene todos los sonidos activos
        /// </summary>
        public void StopAllSounds()
        {
            foreach (var soundObj in activeSoundObjects)
            {
                if (soundObj != null)
                {
                    AudioSource source = soundObj.GetComponent<AudioSource>();
                    if (source != null && source.isPlaying)
                    {
                        source.Stop();
                    }
                }
            }

            LogDebug($"Stopped all {activeSoundObjects.Count} active sounds");
        }

        /// <summary>
        /// Verifica si un sonido está registrado
        /// </summary>
        public bool HasSound(string soundName)
        {
            return soundMap.ContainsKey(soundName);
        }

        /// <summary>
        /// Obtiene la lista de nombres de sonidos disponibles
        /// </summary>
        public string[] GetAvailableSounds()
        {
            string[] sounds = new string[soundMap.Count];
            soundMap.Keys.CopyTo(sounds, 0);
            return sounds;
        }

        // ============================================
        // MÉTODOS PRIVADOS
        // ============================================

        private void CreateAudioSourcePool()
        {
            // Crear prefab temporal para el pool
            GameObject audioSourcePrefab = CreateChildObject("AudioSourcePrefab");
            AudioSource prefabSource = audioSourcePrefab.AddComponent<AudioSource>();
            prefabSource.playOnAwake = false;
            prefabSource.spatialBlend = 0f; // 2D sound by default

            if (audioMixer != null)
            {
                prefabSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
            }

            // Crear pool
            Transform poolParent = CreateChildObject("AudioSourcePool").transform;
            audioSourcePool = new ObjectPool<AudioSource>(
                prefabSource,
                maxConcurrentSounds / 2, // Initial size
                poolParent,
                maxConcurrentSounds * 2, // Max size
                true // Auto expand
            );

            // Destruir prefab temporal (el pool crea sus propias copias)
            Destroy(audioSourcePrefab);
        }

        private void RecreateAudioSourcePool()
        {
            if (audioSourcePool != null)
            {
                audioSourcePool.Clear();
            }
            CreateAudioSourcePool();
        }

        private bool ValidateAudioSourcePool()
        {
            if (audioSourcePool == null) return false;

            try
            {
                // Intentar obtener y retornar un objeto para validar
                AudioSource testSource = audioSourcePool.Get();
                if (testSource == null) return false;
                audioSourcePool.Return(testSource);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void CleanupFinishedSounds()
        {
            for (int i = activeSoundObjects.Count - 1; i >= 0; i--)
            {
                var soundObj = activeSoundObjects[i];
                if (soundObj == null)
                {
                    activeSoundObjects.RemoveAt(i);
                    continue;
                }

                AudioSource source = soundObj.GetComponent<AudioSource>();
                if (source != null && !source.isPlaying)
                {
                    // Sonido terminó, limpiar
                    Destroy(soundObj);
                    activeSoundObjects.RemoveAt(i);
                }
            }
        }

        private System.Collections.IEnumerator ReturnToPoolWhenFinished(GameObject soundObj, AudioSource source, float duration)
        {
            yield return new WaitForSeconds(duration + 0.1f); // Pequeño buffer

            // Retornar AudioSource al pool
            if (source != null && audioSourcePool != null)
            {
                audioSourcePool.Return(source);
            }

            // Destruir objeto contenedor
            if (soundObj != null)
            {
                Destroy(soundObj);
                activeSoundObjects.Remove(soundObj);
            }
        }

        private void OnVolumeChanged(float newVolume)
        {
            // Aplicar nuevo volumen a sonidos activos (si es necesario)
            // Nota: Los nuevos sonidos usarán el volumen actual automáticamente
            LogDebug($"SFX volume changed to: {newVolume}");
        }

        private void ApplyCurrentVolume()
        {
            // El volumen se aplica automáticamente en PlaySound()
            // Aquí podríamos actualizar sonidos ya reproduciéndose si fuera necesario
        }

        // ============================================
        // SOBRESCRITURA DE DEBUG
        // ============================================

        public override string GetDebugInfo()
        {
            string info = base.GetDebugInfo();
            info += $"\n- Sounds registered: {soundMap.Count}\n";
            info += $"- Active sounds: {activeSoundObjects.Count}\n";
            info += $"- Max concurrent: {maxConcurrentSounds}\n";
            info += $"- Current volume: {GameObservers.SFXVolumeChanged.GetValue()}\n";

            if (audioSourcePool != null)
            {
                var stats = audioSourcePool.GetStatistics();
                info += $"- Pool: {stats.TotalCreated} created, {stats.CurrentActive} active, {stats.CurrentAvailable} available";
            }

            return info;
        }
    }
}

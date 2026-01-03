# Guía de Addressables - Retro FPS Engine

## 📦 Sistema de Addressables Básico

El sistema de Addressables en Retro FPS Engine está diseñado para ser **simple y eficiente**, sin la complejidad del motor de terror. Se enfoca en carga básica de assets para mantener el estilo retro del motor.

## 🏗️ Arquitectura

### **CAssetManager**
Clase principal que maneja la carga de assets:

```csharp
namespace RetroFPS
{
    public class CAssetManager : MonoBehaviour
    {
        // Singleton para acceso global
        public static CAssetManager Instance { get; private set; }

        // Cache de assets cargados
        private Dictionary<string, Object> assetCache = new Dictionary<string, Object>();
    }
}
```

## 🚀 Configuración Inicial

### 1. **Instalar Addressables**
```bash
# En Unity Package Manager
Addressables 1.21.0+
```

### 2. **Configurar Grupos**
1. Abre `Window → Asset Management → Addressables → Groups`
2. Crea grupo: **"RetroFPS_Assets"**
3. Arrastra prefabs/sprites/audio al grupo
4. **Build**: `Window → Asset Management → Addressables → Build → New Build`

### 3. **Agregar a Escena**
```csharp
// Crear GameObject vacío llamado "AssetManager"
// Agregar componente CAssetManager
// (Opcional) Configurar enableDebugLogs = true para desarrollo
```

## 🎮 Uso Básico

### **Carga Síncrona (Assets Críticos)**
```csharp
// Para assets que necesitas inmediatamente
GameObject enemyPrefab = CAssetManager.Instance.LoadAsset<GameObject>("Enemy_Basic");

if (enemyPrefab != null)
{
    Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
}
```

### **Carga Asíncrona (Recomendado)**
```csharp
// Para mejor rendimiento
async Task LoadEnemyAsync()
{
    GameObject enemyPrefab = await CAssetManager.Instance.LoadAssetAsync<GameObject>("Enemy_Basic");

    if (enemyPrefab != null)
    {
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
```

### **Instanciación Directa**
```csharp
// Cargar e instanciar en un solo paso
GameObject enemy = await CAssetManager.Instance.InstantiateAssetAsync(
    "Enemy_Basic",
    spawnPosition,
    Quaternion.identity,
    parentTransform
);
```

### **Carga de Escenas**
```csharp
// Cambiar de nivel
await CAssetManager.Instance.LoadSceneAsync("Level_02", LoadSceneMode.Single);
```

## 📂 Estructura de Assets Recomendada

```
Assets/0.RetroFPS-Engine/Resources/Addressables/
├── Prefabs/
│   ├── Enemies/
│   │   ├── Enemy_Basic.prefab
│   │   ├── Enemy_Fast.prefab
│   │   └── Enemy_Tank.prefab
│   ├── Weapons/
│   │   ├── Pistol.prefab
│   │   └── Shotgun.prefab
│   └── Props/
│       ├── AmmoBox.prefab
│       └── HealthPack.prefab
├── Audio/
│   ├── SFX/
│   │   ├── shoot.wav
│   │   └── reload.wav
│   └── Music/
│       ├── level1.mp3
│       └── boss.mp3
└── UI/
    ├── HUD.prefab
    └── PauseMenu.prefab
```

## 🎯 Mejores Prácticas

### ✅ **Convenciones de Naming**
```csharp
// Prefabs
"Enemy_{Type}"          // Enemy_Basic, Enemy_Fast
"Weapon_{Type}"         // Weapon_Pistol, Weapon_Shotgun
"Prop_{Type}"           // Prop_AmmoBox, Prop_HealthPack

// Audio
"SFX_{Action}"          // SFX_Shoot, SFX_Reload
"MUSIC_{Context}"       // MUSIC_Level1, MUSIC_Boss

// UI
"UI_{Screen}"           // UI_HUD, UI_PauseMenu
```

### ✅ **Gestión de Memoria**
```csharp
// Liberar assets cuando no se necesiten
CAssetManager.Instance.UnloadAsset("Enemy_Basic");

// Limpiar cache entre niveles
CAssetManager.Instance.ClearCache();
```

### ✅ **Carga Asíncrona**
```csharp
// Siempre usar async para mejor rendimiento
public async void SpawnEnemy()
{
    var enemy = await CAssetManager.Instance.LoadAndInstantiateAsync(
        "Enemy_Basic",
        transform.position
    );

    // Configurar enemy aquí
    enemy.GetComponent<EnemyController>().Initialize();
}
```

## 🔧 Integración con Otros Sistemas

### **Con Sistema de Enemigos**
```csharp
public class EnemySpawner : MonoBehaviour
{
    public async void SpawnRandomEnemy()
    {
        string[] enemyTypes = { "Enemy_Basic", "Enemy_Fast", "Enemy_Tank" };
        string randomEnemy = enemyTypes[Random.Range(0, enemyTypes.Length)];

        GameObject enemy = await CAssetManager.Instance.InstantiateAssetAsync(
            randomEnemy,
            transform.position
        );

        // Configurar comportamiento
        enemy.GetComponent<EnemyAI>().SetDifficulty(difficulty);
    }
}
```

### **Con Sistema de Armas**
```csharp
public class WeaponPickup : MonoBehaviour, Iinteract
{
    [SerializeField] private string weaponAddress = "Weapon_Shotgun";

    public async void Oninteract()
    {
        GameObject weaponPrefab = await CAssetManager.Instance.LoadAssetAsync<GameObject>(weaponAddress);

        if (weaponPrefab != null)
        {
            // Dar arma al jugador
            PlayerInventory.Instance.AddWeapon(weaponPrefab.GetComponent<CWeapon>());
            Destroy(gameObject); // Destruir pickup
        }
    }
}
```

## 🐛 Troubleshooting

### **Error: "Address not found"**
```
Solución:
1. Verificar que el asset esté marcado como Addressable
2. Confirmar que el nombre coincida exactamente
3. Revisar que el build de Addressables esté actualizado
```

### **Error: "Out of memory"**
```
Solución:
1. Liberar assets no utilizados: CAssetManager.Instance.UnloadAsset()
2. Limpiar cache periódicamente: CAssetManager.Instance.ClearCache()
3. Usar object pooling para assets reutilizables
```

### **Performance Issues**
```
Solución:
1. Preferir carga asíncrona sobre síncrona
2. Precargar assets críticos al inicio del nivel
3. Usar cache inteligente para assets frecuentes
```

## 📊 Rendimiento

### **Recomendaciones**
- **Assets Críticos**: Carga síncrona al inicio
- **Assets Opcionales**: Carga asíncrona bajo demanda
- **Cache Size**: Mantener < 50 assets simultáneos
- **Unload**: Liberar assets de niveles anteriores

### **Debug Tools**
```csharp
// Ver estado del sistema
Debug.Log(CAssetManager.Instance.GetDebugInfo());

// Verificar si asset está cargado
bool isLoaded = CAssetManager.Instance.IsAssetLoaded("Enemy_Basic");
```

## 🎨 Casos de Uso en FPS Retro

### **Spawning de Enemigos**
```csharp
public class WaveSpawner : MonoBehaviour
{
    private async void SpawnWave()
    {
        for (int i = 0; i < 5; i++)
        {
            await CAssetManager.Instance.InstantiateAssetAsync(
                "Enemy_Basic",
                GetRandomSpawnPoint()
            );
            await Task.Delay(500); // Pequeño delay entre spawns
        }
    }
}
```

### **Cambio de Niveles**
```csharp
public class LevelManager : MonoBehaviour
{
    public async void LoadNextLevel(string levelName)
    {
        // Limpiar assets del nivel actual
        CAssetManager.Instance.ClearCache();

        // Cargar nuevo nivel
        await CAssetManager.Instance.LoadSceneAsync($"Level_{levelName}");

        // Precargar assets del nuevo nivel
        await PreloadLevelAssets(levelName);
    }
}
```

## 🔄 Migración desde Resources

### **Antes (Resources)**
```csharp
GameObject prefab = Resources.Load<GameObject>("Prefabs/Enemy");
Instantiate(prefab, position, rotation);
```

### **Después (Addressables)**
```csharp
GameObject prefab = await CAssetManager.Instance.LoadAssetAsync<GameObject>("Enemy");
GameObject instance = Instantiate(prefab, position, rotation);
```

### **Beneficios de la Migración**
- ✅ Builds más pequeños
- ✅ Carga bajo demanda
- ✅ Actualizaciones sin rebuild
- ✅ Mejor organización de assets
- ✅ CDN support (futuro)

---

**Versión**: 1.0.0  
**Complejidad**: Baja (vs Sistema avanzado del motor de terror)  
**Performance**: Optimizado para juegos retro  
**Mantenimiento**: Simple y directo

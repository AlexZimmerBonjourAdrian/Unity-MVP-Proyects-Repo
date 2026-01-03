# RolEngine - Sistema de Rol Reutilizable

Sistema de estadísticas de rol mantenible, desacoplado y 100% reutilizable para múltiples proyectos.

## 🎯 Características

- ✅ **Desacoplado**: Interfaces para integración fácil
- ✅ **Reutilizable**: Funciona en cualquier proyecto Unity
- ✅ **Mantenible**: Código organizado y extensible
- ✅ **Notificaciones**: Eventos y Observers para integración
- ✅ **Persistencia**: Guardado/carga automático
- ✅ **Templates**: ScriptableObjects configurables desde el editor

## 📁 Estructura

```
RolEngine/
├── Interfaces/
│   ├── IStatSystem.cs          # Interface principal del sistema
│   └── IStatTemplate.cs         # Interface para templates
├── Events/
│   └── StatEvents.cs            # Eventos del sistema
├── Observers/
│   └── StatObservers.cs          # Observers para UI reactiva
├── Templates/
│   └── StatTemplateSO.cs        # ScriptableObjects para templates
├── Persistence/
│   ├── IStatPersistence.cs      # Interface de persistencia
│   └── PlayerPrefsStatPersistence.cs  # Implementación con PlayerPrefs
├── Examples/
│   └── StatSystemIntegrationExample.cs  # Ejemplos de integración
└── CMICILSPSystem.cs            # Sistema principal
```

## 🚀 Uso Básico

### Inicialización

```csharp
using RolEngine;

// Obtener instancia (Singleton)
CMICILSPSystem statSystem = CMICILSPSystem.Instance;

// Aplicar template
statSystem.ApplyTemplate(statSystem.Detective);

// O usar ScriptableObject
StatTemplateSO templateSO = // Cargar desde Resources o Addressables
statSystem.ApplyTemplate(templateSO);
```

### Leer y Modificar Stats

```csharp
// Leer
int sanity = statSystem.GetStat(CMICILSPSystem.Stats.Sanity);

// Modificar
statSystem.IncreaseStat(CMICILSPSystem.Stats.Charm, 2);
statSystem.DecreaseStat(CMICILSPSystem.Stats.Sanity, 1);
statSystem.SetStat(CMICILSPSystem.Stats.Wits, 9);

// Verificar requisitos
bool canUse = statSystem.CheckStatRequirement(
    CMICILSPSystem.Stats.Charm, 7
);
```

## 🔌 Integración con Otros Sistemas

### 1. Usando Eventos

```csharp
// Suscribirse a cambios
CMICILSPSystem.Instance.OnStatChanged += (evt) => {
    Debug.Log($"{evt.StatName} cambió: {evt.OldValue} -> {evt.NewValue}");
};

// Suscribirse a cambios de template
CMICILSPSystem.Instance.OnTemplateApplied += (evt) => {
    Debug.Log($"Template aplicado: {evt.TemplateName}");
};
```

### 2. Usando Observers (UI Reactiva)

```csharp
// Suscribirse a cambios de Sanity
StatObservers.SanityChanged.Attach((newValue) => {
    sanityText.text = $"Sanity: {newValue}";
});

// Suscribirse a cambios de template
StatObservers.TemplateChanged.Attach((templateName) => {
    Debug.Log($"Template: {templateName}");
});
```

### 3. Usando la Interface (Desacoplado)

```csharp
// Trabajar con la interfaz en lugar de la clase concreta
IStatSystem<CMICILSPSystem.Stats> statSystem = CMICILSPSystem.Instance;

// Esto permite cambiar la implementación sin modificar el código
int charm = statSystem.GetStat(CMICILSPSystem.Stats.Charm);
bool canDo = statSystem.CheckStatRequirement(CMICILSPSystem.Stats.Wits, 8);
```

## 💾 Persistencia

El sistema guarda automáticamente cuando:
- La aplicación pierde el foco
- La aplicación se pausa
- El objeto se destruye

```csharp
// Guardar manualmente
CMICILSPSystem.Instance.SaveStats();

// Cargar manualmente
CMICILSPSystem.Instance.LoadStats();

// Limpiar datos guardados
CMICILSPSystem.Instance.ClearSavedData();
```

## 📝 Crear Templates desde el Editor

1. Click derecho en el Project
2. `Create > RolEngine > Stat Template`
3. Configurar los valores de las stats
4. Usar en código:

```csharp
StatTemplateSO myTemplate = // Cargar desde Resources
CMICILSPSystem.Instance.ApplyTemplate(myTemplate);
```

## 🔧 Configuración

En el Inspector de `CMICILSPSystem` puedes configurar:

- **Min Stat Value**: Valor mínimo (default: 1)
- **Max Stat Value**: Valor máximo (default: 10)
- **Enable Notifications**: Activar eventos/observers
- **Enable Persistence**: Activar guardado automático
- **Auto Load On Start**: Cargar stats al iniciar
- **Use Persistence**: Usar sistema de persistencia

## 🎮 Integración con Yarn Spinner

El sistema está preparado para integración con Yarn Spinner. Los métodos comentados en el código muestran cómo se puede integrar.

## 📚 Ejemplos

Ver `RolSystemExample.cs` y `StatSystemIntegrationExample.cs` para ejemplos completos de uso.

## 🔄 Migración desde Versión Anterior

El sistema mantiene compatibilidad con el código existente. Los métodos antiguos siguen funcionando, pero se recomienda migrar a:

- Usar `IStatSystem` interface cuando sea posible
- Suscribirse a eventos en lugar de polling
- Usar Observers para UI reactiva
- Crear templates como ScriptableObjects

## 🐛 Bugs Corregidos

- ✅ `IncreaseStat` ahora hace clamp correctamente
- ✅ Notificaciones automáticas de cambios
- ✅ Persistencia funcional

## 📦 Dependencias

- Unity Engine
- System.Collections.Generic
- System (para Action y eventos)

## 🔮 Futuras Mejoras

- Integración con EventBus del RetroFPS Engine
- Sistema de experiencia/leveling
- Modificadores temporales de stats
- Sistema de habilidades/skills


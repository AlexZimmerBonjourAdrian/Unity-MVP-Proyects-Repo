# Documentación ECS - Eldrick Super Food Market MVP

## Índice

1. [Introducción](#introducción)
2. [Arquitectura ECS](#arquitectura-ecs)
3. [Core ECS](#core-ecs)
4. [Componentes](#componentes)
5. [Sistemas](#sistemas)
6. [ECSManager](#ecsmanager)
7. [Ejemplos de Uso](#ejemplos-de-uso)
8. [Guía de Implementación](#guía-de-implementación)

---

## Introducción

Este proyecto utiliza una arquitectura **Entity Component System (ECS)** para gestionar la lógica del juego. El ECS separa los datos (Componentes) de la lógica (Sistemas), permitiendo un diseño modular y escalable.

### Ventajas del ECS

- **Modularidad**: Los componentes son independientes y reutilizables
- **Escalabilidad**: Fácil añadir nuevos componentes y sistemas
- **Performance**: Procesamiento eficiente de múltiples entidades
- **Mantenibilidad**: Cambios en un sistema no afectan a otros

---

## Arquitectura ECS

### Conceptos Fundamentales

- **Entity (Entidad)**: Identificador único que agrupa componentes
- **Component (Componente)**: Contenedor de datos sin lógica
- **System (Sistema)**: Contiene la lógica que procesa componentes
- **World (Mundo)**: Gestiona todas las entidades, componentes y sistemas

### Flujo de Trabajo

```
1. Crear Entity → 2. Añadir Componentes → 3. Sistemas procesan entidades con componentes específicos
```

---

## Core ECS

### Entity

**Ubicación**: `Assets/ECS/Core/Entity.cs`

Estructura simple que representa una entidad con un ID único.

```csharp
Entity entity = world.CreateEntity();
```

**Propiedades**:
- `Id`: Identificador único de la entidad

### World

**Ubicación**: `Assets/ECS/Core/World.cs`

Gestiona todas las entidades, componentes y sistemas del juego.

**Métodos Principales**:

```csharp
// Crear entidad
Entity entity = world.CreateEntity();

// Añadir componente
world.AddComponent<CustomerComponent>(entity, customerComponent);

// Obtener componente
CustomerComponent customer = world.GetComponent<CustomerComponent>(entity);

// Verificar si tiene componente
bool hasComponent = world.HasComponent<CustomerComponent>(entity);

// Eliminar componente
world.RemoveComponent<CustomerComponent>(entity);

// Obtener todas las entidades con un componente específico
foreach (Entity e in world.GetEntitiesWithComponent<CustomerComponent>())
{
    // Procesar entidad
}

// Añadir sistema
world.AddSystem(customerSystem);

// Actualizar sistemas
world.Update(deltaTime);
```

### IComponent

**Ubicación**: `Assets/ECS/Core/IComponent.cs`

Interfaz que deben implementar todos los componentes.

```csharp
public class MyComponent : IComponent
{
    // Propiedades de datos
}
```

### ISystem

**Ubicación**: `Assets/ECS/Core/ISystem.cs`

Interfaz que deben implementar todos los sistemas.

```csharp
public class MySystem : ISystem
{
    public void Initialize(World world) { }
    public void Update(float deltaTime) { }
    public void Shutdown() { }
}
```

---

## Componentes

### CustomerComponent

**Ubicación**: `Assets/ECS/Components/Customer/CustomerComponent.cs`

Almacena información sobre un cliente.

**Propiedades**:
- `CustomerType Type`: Tipo de entidad (Lovecraftian, Angel, Demon)
- `PersonalityType Personality`: Personalidad (Friendly, Sarcastic, Dry)
- `string Name`: Nombre del cliente
- `string OrderDescription`: Descripción del pedido
- `bool HasBeenServed`: Si el cliente ha sido atendido
- `bool WillGiveTip`: Si el cliente dará propina

**Enums**:
```csharp
public enum CustomerType
{
    Lovecraftian,
    Angel,
    Demon
}

public enum PersonalityType
{
    Friendly,
    Sarcastic,
    Dry
}
```

**Ejemplo de Uso**:
```csharp
var customer = new CustomerComponent
{
    Type = CustomerType.Lovecraftian,
    Personality = PersonalityType.Sarcastic,
    Name = "Cthulhu",
    OrderDescription = "Hamburguesa con almas de niños"
};
world.AddComponent<CustomerComponent>(entity, customer);
```

### AngerComponent

**Ubicación**: `Assets/ECS/Components/Anger/AngerComponent.cs`

Gestiona el nivel de ira del jugador.

**Propiedades**:
- `float CurrentAnger`: Ira actual (0-100)
- `float MaxAnger`: Ira máxima (100)
- `bool HasExploded`: Si el jugador explotó de ira

**Métodos**:
- `float GetAngerPercentage()`: Retorna el porcentaje de ira (0-100)

**Ejemplo de Uso**:
```csharp
var anger = new AngerComponent
{
    CurrentAnger = 50f,
    MaxAnger = 100f,
    HasExploded = false
};
world.AddComponent<AngerComponent>(playerEntity, anger);

// Obtener porcentaje
float percentage = anger.GetAngerPercentage(); // 50%
```

### MoneyComponent

**Ubicación**: `Assets/ECS/Components/Money/MoneyComponent.cs`

Gestiona el dinero y ahorros del jugador.

**Propiedades**:
- `float CurrentMoney`: Dinero actual disponible
- `float TotalSavings`: Ahorros totales acumulados
- `float DailyIncome`: Ingresos del día
- `float DailyExpenses`: Gastos diarios ($325 por defecto)
- `float SavingsGoal`: Meta de ahorros ($5,000 por defecto)

**Ejemplo de Uso**:
```csharp
var money = new MoneyComponent
{
    CurrentMoney = 500f,
    TotalSavings = 0f,
    DailyIncome = 0f,
    DailyExpenses = 325f,
    SavingsGoal = 5000f
};
world.AddComponent<MoneyComponent>(playerEntity, money);
```

### OrderComponent

**Ubicación**: `Assets/ECS/Components/Order/OrderComponent.cs`

Gestiona un pedido de un cliente.

**Propiedades**:
- `string OrderDescription`: Descripción del pedido
- `OrderComplexity Complexity`: Complejidad (Simple, Medium, Complex)
- `List<string> RequiredComponents`: Componentes requeridos
- `List<string> SelectedComponents`: Componentes seleccionados por el jugador
- `bool IsCompleted`: Si el pedido está completado
- `bool IsCorrect`: Si el pedido es correcto

**Enums**:
```csharp
public enum OrderComplexity
{
    Simple,
    Medium,
    Complex
}
```

**Ejemplo de Uso**:
```csharp
var order = new OrderComponent
{
    OrderDescription = "Hamburguesa con almas de niños",
    Complexity = OrderComplexity.Medium,
    RequiredComponents = new List<string> { "Hamburguesa", "Almas", "Niños" },
    SelectedComponents = new List<string>(),
    IsCompleted = false,
    IsCorrect = false
};
world.AddComponent<OrderComponent>(customerEntity, order);
```

### DialogueComponent

**Ubicación**: `Assets/ECS/Components/Dialogue/DialogueComponent.cs`

Gestiona diálogos e interacciones.

**Propiedades**:
- `string Name`: Nombre del hablante
- `string InitialText`: Texto inicial del diálogo
- `List<DialogueOption> Options`: Opciones de diálogo
- `bool HasSpoken`: Si ya habló
- `bool WillDefend`: Si se defenderá
- `bool WillSurrender`: Si se rendirá

---

## Sistemas

### CustomerSystem

**Ubicación**: `Assets/ECS/Systems/CustomerSystem.cs`

Gestiona la creación, aparición y eliminación de clientes.

**Métodos Públicos** (sin implementar):
- `Entity CreateCustomer(CustomerType type, PersonalityType personality)`: Crea un cliente
- `void SpawnCustomer()`: Genera un cliente aleatorio
- `void RemoveCustomer(Entity customer)`: Elimina un cliente

**Ejemplo de Implementación**:
```csharp
public Entity CreateCustomer(CustomerType type, PersonalityType personality)
{
    Entity customer = world.CreateEntity();
    var customerComponent = new CustomerComponent
    {
        Type = type,
        Personality = personality,
        Name = GetCustomerName(type),
        HasBeenServed = false
    };
    world.AddComponent<CustomerComponent>(customer, customerComponent);
    activeCustomers.Add(customer);
    return customer;
}
```

### OrderSystem

**Ubicación**: `Assets/ECS/Systems/OrderSystem.cs`

Procesa y verifica pedidos de clientes.

**Métodos Públicos** (sin implementar):
- `void CreateOrder(Entity customer, string description, OrderComplexity complexity)`: Crea un pedido
- `bool ProcessOrder(Entity orderEntity, List<string> selectedComponents)`: Procesa y verifica un pedido
- `void CompleteOrder(Entity orderEntity)`: Completa un pedido

**Ejemplo de Implementación**:
```csharp
public bool ProcessOrder(Entity orderEntity, List<string> selectedComponents)
{
    var order = world.GetComponent<OrderComponent>(orderEntity);
    if (order == null) return false;

    order.SelectedComponents = selectedComponents;
    
    // Verificar si los componentes coinciden
    bool isCorrect = order.RequiredComponents.SequenceEqual(
        selectedComponents.OrderBy(x => x)
    );
    
    order.IsCorrect = isCorrect;
    order.IsCompleted = true;
    
    return isCorrect;
}
```

### AngerSystem

**Ubicación**: `Assets/ECS/Systems/AngerSystem.cs`

Gestiona el sistema de ira del jugador.

**Métodos Públicos** (sin implementar):
- `void IncreaseAnger(float amount)`: Aumenta la ira
- `void DecreaseAnger(float amount)`: Reduce la ira
- `float GetCurrentAnger()`: Obtiene la ira actual
- `bool HasExploded()`: Verifica si explotó
- `string GetAngerWarning()`: Obtiene mensaje de advertencia

**Ejemplo de Implementación**:
```csharp
public void IncreaseAnger(float amount)
{
    var anger = world.GetComponent<AngerComponent>(playerEntity);
    if (anger == null) return;

    anger.CurrentAnger += amount;
    if (anger.CurrentAnger > anger.MaxAnger)
    {
        anger.CurrentAnger = anger.MaxAnger;
        anger.HasExploded = true;
    }
}

public string GetAngerWarning()
{
    var anger = world.GetComponent<AngerComponent>(playerEntity);
    if (anger == null) return "";

    float percentage = anger.GetAngerPercentage();
    
    if (percentage >= 90) return "¡CUIDADO! Una más y explotas";
    if (percentage >= 75) return "¡Respira! Estás al límite";
    if (percentage >= 50) return "Mantén la calma...";
    
    return "";
}
```

### EconomySystem

**Ubicación**: `Assets/ECS/Systems/EconomySystem.cs`

Gestiona el sistema económico del juego.

**Métodos Públicos** (sin implementar):
- `void AddMoney(float amount)`: Añade dinero
- `void SubtractMoney(float amount)`: Resta dinero
- `void ProcessDailyExpenses()`: Procesa gastos diarios
- `void AddToSavings(float amount)`: Añade a ahorros
- `bool HasReachedGoal()`: Verifica si alcanzó la meta
- `float GetCurrentMoney()`: Obtiene dinero actual
- `float GetTotalSavings()`: Obtiene ahorros totales

**Ejemplo de Implementación**:
```csharp
public void ProcessDailyExpenses()
{
    var money = world.GetComponent<MoneyComponent>(playerEntity);
    if (money == null) return;

    money.CurrentMoney -= money.DailyExpenses;
    
    if (money.CurrentMoney > 0)
    {
        money.TotalSavings += money.CurrentMoney;
        money.CurrentMoney = 0;
    }
}

public bool HasReachedGoal()
{
    var money = world.GetComponent<MoneyComponent>(playerEntity);
    if (money == null) return false;
    
    return money.TotalSavings >= money.SavingsGoal;
}
```

### DialogueSystem

**Ubicación**: `Assets/ECS/Systems/DialogueSystem.cs`

Gestiona el sistema de diálogos (estructura básica, sin implementar).

---

## ECSManager

**Ubicación**: `Assets/ECS/ECSManager.cs`

MonoBehaviour que inicializa y gestiona el World ECS.

**Funcionalidad**:
- Crea el World en `Awake()`
- Inicializa todos los sistemas
- Actualiza los sistemas en `Update()`
- Limpia recursos en `OnDestroy()`

**Sistemas Registrados**:
1. CustomerSystem
2. OrderSystem
3. AngerSystem
4. EconomySystem
5. DialogueSystem

**Uso**:
```csharp
// En Unity, añadir ECSManager a un GameObject
// Los sistemas se inicializan automáticamente

// Obtener el World
World world = ecsManager.GetWorld();
```

---

## Ejemplos de Uso

### Crear un Cliente

```csharp
World world = ecsManager.GetWorld();
CustomerSystem customerSystem = // obtener referencia

Entity customer = customerSystem.CreateCustomer(
    CustomerType.Lovecraftian,
    PersonalityType.Sarcastic
);

var customerComponent = world.GetComponent<CustomerComponent>(customer);
customerComponent.Name = "Cthulhu";
customerComponent.OrderDescription = "Hamburguesa con almas de niños";
```

### Procesar un Pedido

```csharp
OrderSystem orderSystem = // obtener referencia

// Crear pedido
orderSystem.CreateOrder(customer, "Hamburguesa con almas", OrderComplexity.Medium);

// Procesar pedido
List<string> selected = new List<string> { "Hamburguesa", "Almas", "Niños" };
bool isCorrect = orderSystem.ProcessOrder(customer, selected);

if (isCorrect)
{
    // Añadir dinero
    economySystem.AddMoney(50f);
}
else
{
    // Aumentar ira y multa
    angerSystem.IncreaseAnger(15f);
    economySystem.SubtractMoney(10f);
}
```

### Gestionar Ira

```csharp
AngerSystem angerSystem = // obtener referencia

// Aumentar ira por cliente sarcástico
angerSystem.IncreaseAnger(20f);

// Verificar advertencia
string warning = angerSystem.GetAngerWarning();
if (!string.IsNullOrEmpty(warning))
{
    // Mostrar advertencia en UI
    UIManager.Instance.ShowWarning(warning);
}

// Verificar si explotó
if (angerSystem.HasExploded())
{
    // Game Over
    GameManager.Instance.GameOver();
}
```

### Gestionar Economía

```csharp
EconomySystem economySystem = // obtener referencia

// Añadir dinero por cliente atendido
economySystem.AddMoney(50f);

// Procesar gastos al final del día
economySystem.ProcessDailyExpenses();

// Verificar si alcanzó la meta
if (economySystem.HasReachedGoal())
{
    // Victoria
    GameManager.Instance.Victory();
}
```

---

## Guía de Implementación

### Paso 1: Implementar CustomerSystem

1. Implementar `CreateCustomer()`:
   - Crear entidad
   - Crear CustomerComponent
   - Añadir componente a entidad
   - Retornar entidad

2. Implementar `SpawnCustomer()`:
   - Generar tipo y personalidad aleatorios
   - Llamar a `CreateCustomer()`
   - Asignar pedido al cliente

3. Implementar `RemoveCustomer()`:
   - Eliminar de lista activa
   - Destruir entidad del world

### Paso 2: Implementar OrderSystem

1. Implementar `CreateOrder()`:
   - Obtener CustomerComponent del cliente
   - Crear OrderComponent
   - Asignar descripción y complejidad
   - Añadir componente a entidad

2. Implementar `ProcessOrder()`:
   - Obtener OrderComponent
   - Comparar componentes requeridos vs seleccionados
   - Marcar como correcto/incorrecto
   - Retornar resultado

3. Implementar `CompleteOrder()`:
   - Marcar pedido como completado
   - Actualizar estado del cliente

### Paso 3: Implementar AngerSystem

1. Implementar `IncreaseAnger()`:
   - Obtener AngerComponent del jugador
   - Aumentar CurrentAnger
   - Verificar si alcanzó MaxAnger
   - Marcar HasExploded si es necesario

2. Implementar `DecreaseAnger()`:
   - Obtener AngerComponent
   - Reducir CurrentAnger
   - Asegurar que no sea negativo

3. Implementar `GetAngerWarning()`:
   - Calcular porcentaje
   - Retornar mensaje según nivel

### Paso 4: Implementar EconomySystem

1. Implementar `AddMoney()`:
   - Obtener MoneyComponent
   - Aumentar CurrentMoney y DailyIncome

2. Implementar `ProcessDailyExpenses()`:
   - Restar DailyExpenses de CurrentMoney
   - Añadir resto a TotalSavings
   - Resetear DailyIncome

3. Implementar `HasReachedGoal()`:
   - Comparar TotalSavings con SavingsGoal

### Paso 5: Integrar con Managers

1. Conectar sistemas con GameManager
2. Conectar sistemas con UIManager
3. Conectar sistemas con DialogueManager

---

## Notas de Desarrollo

### Estado Actual

- ✅ Estructura ECS completa
- ✅ Componentes definidos
- ✅ Sistemas con esqueleto
- ⚠️ Lógica de sistemas sin implementar (TODO)
- ⚠️ Integración con Managers pendiente

### Próximos Pasos

1. Implementar lógica de CustomerSystem
2. Implementar lógica de OrderSystem
3. Implementar lógica de AngerSystem
4. Implementar lógica de EconomySystem
5. Integrar con GameManager y UIManager
6. Testing y balance

---

## Referencias

- **GDD MVP**: `Assets/Documetation/GDD_MVP_1Semana.txt`
- **GDD Completo**: `Assets/Documetation/GDD.txt`
- **Código ECS**: `Assets/ECS/`

---

**Última actualización**: Estructura inicial para MVP de 1 semana


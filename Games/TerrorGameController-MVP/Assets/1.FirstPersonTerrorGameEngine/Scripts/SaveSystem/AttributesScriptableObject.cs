using UnityEngine;

[CreateAssetMenu(fileName = "Attributes", menuName = "ScriptableObjects/AttributesScriptableObject", order = 1)]
public class AttributesScriptableObject : ScriptableObject
{
    // Atributo que representa la vitalidad del jugador.
    public int vitality;

    // Atributo que representa la fuerza del jugador.
    public int strength;

    // Atributo que representa el intelecto del jugador.
    public int intellect;

    // Atributo que representa la resistencia del jugador.
    public int endurance;
}

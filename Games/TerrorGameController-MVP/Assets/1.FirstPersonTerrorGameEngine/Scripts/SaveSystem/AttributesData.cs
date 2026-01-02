using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // Permite que esta clase sea serializable por Unity.
public class AttributesData
{
    public int vitality; // Representa la vitalidad del jugador.
    public int strength; // Representa la fuerza del jugador.
    public int intellect; // Representa el intelecto del jugador.
    public int endurance; // Representa la resistencia del jugador.

    // Constructor que inicializa los atributos con valores predeterminados.
    public AttributesData() 
    {
        this.vitality = 1; // Valor inicial de vitalidad.
        this.strength = 1; // Valor inicial de fuerza.
        this.intellect = 1; // Valor inicial de intelecto.
        this.endurance = 1; // Valor inicial de resistencia.
    }
}

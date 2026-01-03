using UnityEngine;

namespace HorrorEngine
{
    // Asegúrate de que esta línea esté presente para acceder a los eventos
    public class LightSwitch : MonoBehaviour, Iinteract
{
     public bool useCounterEvent = false; // Para elegir qué evento usar

     public void Oninteract()
     {
         Debug.Log("Light switch interacted!");
         if (useCounterEvent) {
             CGameEvents.OnCounLightSwitch.Publish(); // Publica el evento contador
         } else {
             CGameEvents.OnLightSwitch.Publish(); // Publica el evento toggle
         }
         // Aquí podrías añadir lógica visual para el interruptor
     }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RetroFPS
{
    /// <summary>
    /// Interface de interacción - MANTENIDA POR COMPATIBILIDAD.
    /// Esta interface se mantiene para compatibilidad con código existente.
    /// Para nuevos desarrollos, se recomienda implementar ICommand directamente.
    ///
    /// INTEGRACIÓN: Los objetos que implementan Iinteract automáticamente
    /// pueden crear comandos cuando se les solicita.
    /// </summary>
    public interface Iinteract
    {
        /// <summary>
        /// Método de interacción legacy - mantener por compatibilidad
        /// </summary>
        void Oninteract();
    }

    /// <summary>
    /// Extensión para integrar Iinteract con el patrón Command
    /// </summary>
    public static class InteractExtensions
    {
        /// <summary>
        /// Crea un comando de interacción genérico para objetos Iinteract
        /// </summary>
        public static RetroFPS.ICommand CreateInteractCommand(this Iinteract interactable, GameObject targetObject)
        {
            return new RetroFPS.GenericInteractCommand(targetObject, interactable);
        }

        /// <summary>
        /// Verifica si el objeto interactuable puede crear comandos
        /// </summary>
        public static bool SupportsCommandPattern(this Iinteract interactable)
        {
            // Por defecto, todos los Iinteract soportan comandos
            // Las implementaciones específicas pueden sobrescribir esto
            return true;
        }
    }

}
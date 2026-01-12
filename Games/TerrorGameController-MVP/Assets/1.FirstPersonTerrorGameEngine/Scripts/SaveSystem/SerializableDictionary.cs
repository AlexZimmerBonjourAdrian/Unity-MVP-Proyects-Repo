using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorEngine
{
    [System.Serializable] // Permite que esta clase sea serializable por Unity.
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    // Listas serializables para almacenar las claves y valores del diccionario.
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    // Este método se llama antes de que el objeto sea serializado.
    // Convierte el diccionario en dos listas (keys y values) para que Unity pueda serializarlo.
    public void OnBeforeSerialize()
    {
        keys.Clear(); // Limpia la lista de claves.
        values.Clear(); // Limpia la lista de valores.
        foreach (KeyValuePair<TKey, TValue> pair in this) 
        {
            keys.Add(pair.Key); // Agrega cada clave a la lista de claves.
            values.Add(pair.Value); // Agrega cada valor a la lista de valores.
        }
    }

    // Este método se llama después de que el objeto ha sido deserializado.
    // Reconstruye el diccionario a partir de las listas serializadas.
    public void OnAfterDeserialize()
    {
        this.Clear(); // Limpia el diccionario actual.

        // Verifica si las listas de claves y valores tienen el mismo tamaño.
        if (keys.Count != values.Count) 
        {
            Debug.LogError("Tried to deserialize a SerializableDictionary, but the amount of keys ("
                + keys.Count + ") does not match the number of values (" + values.Count 
                + ") which indicates que algo salió mal");
        }

        // Rellena el diccionario con las claves y valores deserializados.
        for (int i = 0; i < keys.Count; i++) 
        {
            this.Add(keys[i], values[i]);
        }
    }
    }
}

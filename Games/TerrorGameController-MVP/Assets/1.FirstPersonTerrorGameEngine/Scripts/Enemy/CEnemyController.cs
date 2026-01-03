using UnityEngine;

namespace HorrorEngine
{
    public class CEnemyController : MonoBehaviour
{
    // Array para almacenar las referencias a todos los CBodyPart encontrados en los hijos
    private CBodyPart[] bodyParts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Busca todos los componentes CBodyPart en este GameObject y en todos sus hijos.
        // Si solo quieres los de los hijos estrictamente (excluyendo el padre),
        // necesitarías un enfoque ligeramente diferente, pero GetComponentsInChildren es lo más común.
        bodyParts = GetComponentsInChildren<CBodyPart>();

        // Opcional: Imprimir cuántos se encontraron para depuración
        if (bodyParts != null && bodyParts.Length > 0)
        {
            //Debug.Log($"Found {bodyParts.Length} CBodyPart components in children of {gameObject.name}.");
            // Puedes iterar sobre ellos si necesitas hacer algo con cada uno al inicio
            // foreach (CBodyPart part in bodyParts)
            // {
            //     Debug.Log($" - Found body part on: {part.gameObject.name}");
            // }
            //Debug.Log($"Found {bodyParts.Length} CBodyPart components in children of {gameObject.name}.");

        }
        else
        {
            Debug.LogWarning($"No CBodyPart components found in children of {gameObject.name}.");
        }
    }

    

    // Opcional: Método para obtener las partes del cuerpo si otro script las necesita
    public CBodyPart[] GetBodyParts()
    {
        return bodyParts;
    }
    }
}


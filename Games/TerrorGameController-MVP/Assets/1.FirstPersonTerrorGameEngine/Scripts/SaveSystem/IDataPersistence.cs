using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// La interfaz IDataPersistence define los métodos que deben implementarse
    /// para cargar y guardar datos del juego. Esto permite que diferentes clases
    /// implementen su propia lógica de persistencia, como guardar en archivos,
    /// bases de datos, o servicios en la nube.
    /// </summary>
    public interface IDataPersistence
{
    /// <summary>
    /// Método para cargar datos en el objeto GameData.
    /// Este método será implementado por clases que necesiten leer datos
    /// desde una fuente de almacenamiento.
    /// </summary>
    /// <param name="data">El objeto GameData donde se cargarán los datos.</param>
    void LoadData(GameData data);

    /// <summary>
    /// Método para guardar datos desde el objeto GameData.
    /// Este método será implementado por clases que necesiten escribir datos
    /// a una fuente de almacenamiento.
    /// </summary>
    /// <param name="data">El objeto GameData que contiene los datos a guardar.</param>
    void SaveData(GameData data);
    }
}

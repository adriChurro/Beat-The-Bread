using UnityEngine;
using System.Collections.Generic;
using System.IO;

//Resumen rápido. Primero vamos nosotros manualmente al ritmo de la musica ha hacer 
// lo que queramos que haga el jugador, y lo vamos a guardar en un archivo json. 
// despues lo leemos y lo comparamos con lo que ha hecho el jugador.

[System.Serializable]
public class DatosClic
{
    public float posicionX;
    public float posicionY;
    public double tiempoCancion;
    public double beat;
}


[System.Serializable]
public class ContenedorClics
{
    public List<DatosClic> listaClics = new List<DatosClic>();
}



public class GrabadorRaton : MonoBehaviour
{
    public ContenedorClics registro = new ContenedorClics();
    
    [Tooltip("El archivo se guardará en la carpeta Assets de tu proyecto")]
    public string nombreArchivo = "NivelGrabado.json";

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RegistrarClic();
        }

        // Si pulsas la tecla G, confirmamos y guardamos el archivo
        if (Input.GetKeyDown(KeyCode.G))
        {
            GuardarEnJson();
        }
    }

    void RegistrarClic()
    {
        // Cogemos la posición del ratón en la pantalla (píxeles)
        Vector3 posRaton = Input.mousePosition; 
        
        DatosClic nuevoClic = new DatosClic();
        nuevoClic.posicionX = posRaton.x;
        nuevoClic.posicionY = posRaton.y;

        // Si el Conductor está sonando, guardamos los datos rítmicos
        if (Conductor.Instance != null)
        {
            nuevoClic.tiempoCancion = Conductor.Instance.TiempoCancion;
            nuevoClic.beat = Conductor.Instance.BeatActual;
        }

        registro.listaClics.Add(nuevoClic);
        Debug.Log($"Clic guardado. Total en memoria: {registro.listaClics.Count}");
    }

    void GuardarEnJson()
    {
        // El 'true' final formatea el JSON con saltos de línea para que sea legible por humanos
        string contenidoJson = JsonUtility.ToJson(registro, true);

        // Application.dataPath es tu carpeta "Assets" del proyecto.
        // Es el mejor sitio durante una Jam para encontrar el archivo rápido.
        string ruta = Path.Combine(Application.dataPath, nombreArchivo);

        File.WriteAllText(ruta, contenidoJson);
        Debug.Log($"<color=green>¡ARCHIVO GUARDADO EN:</color> {ruta}");
    }
}
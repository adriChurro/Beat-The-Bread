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

    void Update()
    {

        if (AdministradorJuego.Instance == null || AdministradorJuego.Instance.estadoActual != AdministradorJuego.EstadoJuego.Admin) {return;}

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
        string contenidoJson = JsonUtility.ToJson(registro, true);

        string rutaCarpeta = Path.Combine(Application.dataPath, "Niveles");
        string[] archivos = Directory.GetFiles(rutaCarpeta, "*.json");
        int numeroDeArchivos = archivos.Length;
        string nombreArchivo = $"NivelGrabado {numeroDeArchivos + 1}.json";

        string ruta = Path.Combine(Application.dataPath, "Niveles" ,nombreArchivo);

        if (!Directory.Exists(rutaCarpeta))
        {
            Directory.CreateDirectory(rutaCarpeta);
        }

        File.WriteAllText(ruta, contenidoJson);
        Debug.Log($"<color=green>¡ARCHIVO GUARDADO EN:</color> {ruta}");
    }
}
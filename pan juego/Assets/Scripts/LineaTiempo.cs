using UnityEngine;
using System.Collections.Generic;
using System.IO;

// Esta clase es la que se encarga de decidir si el click del jugador ha dado el el clavo.

public class LineaTiempo : MonoBehaviour
{
    [Header("Datos Cargados")]
    public List<DatosClic> clicsDelNivel = new List<DatosClic>();

    [Header("Configuración")]
	//POR DEFECTO EL UNO. ESTO HABRA QUE CAMBIARLO.
    public string nombreArchivoACargar = "NivelGrabado 1.json";


	// Explicación. Ponemos por defecto que no escuche los clicks (Desactivado),
	// cuando se hacerque el click que ha leido del nivel, cambiamos el estdo a mal, 
	// porque significa que le ha dado pronto. Despues cuando este más cerca del momento exacto,
	// lo ponemos a bien, y ya en el momento justo, o con MUY poco margen, perfecto.
	// Lo mismo pero ahora por el otro lado, alejandose del click.

	public enum estado {
		Desactivado,
		Mal,
		Bien,
		Perfecto
	}

	public estado estadoActual = estado.Desactivado;
	public bool clicado = false;
	public float tiempoParaSiguienteCambio;

	public float margenMal = 0.5f;
	public float margenBien = 0.2f;
	public float margenPerfecto = 0.05f;

	public int indiceClick = 0;


	// Leemos todos los datos del nivel para poder trabajar con ellos.
    public void CargarNivel(string nombreArchivo)
    {

        string rutaArchivo = Path.Combine(Application.dataPath, "Niveles", nombreArchivo);

        // Comprobamos que el archivo existe antes de intentar leerlo
        if (!File.Exists(rutaArchivo))
        {
            Debug.LogError($"No se encontró el archivo de nivel en: {rutaArchivo}");
            return;
        }

        // 3. Leemos todo el texto del archivo JSON
        string contenidoJson = File.ReadAllText(rutaArchivo);

        // 4. Convertimos el texto de vuelta a nuestro objeto ContenedorClics
        ContenedorClics datosCargados = JsonUtility.FromJson<ContenedorClics>(contenidoJson);

        // 5. Guardamos la lista en nuestra variable local
        if (datosCargados != null)
        {
            clicsDelNivel = datosCargados.listaClics;
            Debug.Log($"<color=cyan>Nivel cargado correctamente.</color> Total de clics: {clicsDelNivel.Count}");

			if (clicsDelNivel.Count > 0) {
				tiempoParaSiguienteCambio = clicsDelNivel[indice].tiempoCancion - margenMal;
			}
        }
    }

	public void setLevel(String nombreArchivo)
	{
		nombreArchivoACargar = nombreArchivo;
	}

	public void cambiarEstado {

	}

	public void Update() {

	}

	
}

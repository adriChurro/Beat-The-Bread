
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

// Esta clase es la que se encarga de decidir si el click del jugador ha dado el el clavo.

public class LineaTiempo : MonoBehaviour
{

	public int indiceClick = 0;
	public bool clicado = false;
	public double tiempoParaSiguienteCambio;

    [Header("Datos Cargados")]
    public List<DatosClic> clicsDelNivel = new List<DatosClic>();

    [Header("Configuración")]
	//POR DEFECTO EL UNO. ESTO HABRA QUE CAMBIARLO.
    public string nombreArchivoACargar = "NivelGrabado 2.json";


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

	public double margenMal = 0.5;
	public double margenBien = 0.2;
	public double margenPerfecto = 0.05;

	[Header("Partida")]
	public int puntuacion = 0;


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
				tiempoParaSiguienteCambio = clicsDelNivel[indiceClick].tiempoCancion - margenMal;
			}
        }
    }

	public void setLevel(string nombreArchivo)
	{
		nombreArchivoACargar = nombreArchivo;
	}

	
	public void ActualizarEstado(double tiempoActual)
{
    // Si ya no quedan más clics en el nivel
    if (indiceClick >= clicsDelNivel.Count) 
    {
        estadoActual = estado.Desactivado;
		Camera.main.backgroundColor = Color.black;
        return;
    }

    // --- LA SOLUCIÓN: BUSCAR EL BEAT MÁS CERCANO ---
    // Mientras haya un siguiente beat en la lista...
    while (indiceClick < clicsDelNivel.Count - 1)
    {
        double distanciaAlActual = Math.Abs(clicsDelNivel[indiceClick].tiempoCancion - tiempoActual);
        double distanciaAlSiguiente = Math.Abs(clicsDelNivel[indiceClick + 1].tiempoCancion - tiempoActual);

        // Si el siguiente beat ya está más cerca de nosotros que el que estábamos mirando,
        // significa que hemos cruzado el punto medio entre ambos. ¡Avanzamos al siguiente!
        if (distanciaAlSiguiente < distanciaAlActual)
        {
            indiceClick++;
			clicado = false;
        }
        else
        {
            // El beat actual sigue siendo el más cercano, paramos de comprobar.
            break; 
        }
    }

    // --- LA EVALUACIÓN ---

	if (clicado)
    {
        estadoActual = estado.Desactivado; 
        return;
    }
    double tiempoObjetivo = clicsDelNivel[indiceClick].tiempoCancion;
    double distancia = Math.Abs(tiempoObjetivo - tiempoActual);
	

    // Evaluamos los márgenes sobre el beat correcto
    if (distancia <= margenPerfecto)
    {
        estadoActual = estado.Perfecto;
		Camera.main.backgroundColor = Color.gold;
    }
    else if (distancia <= margenBien)
    {
        estadoActual = estado.Bien;
		Camera.main.backgroundColor = Color.green;
    }
    else if (distancia <= margenMal)
    {
        estadoActual = estado.Mal;
		Camera.main.backgroundColor = Color.brown;
    }
    else
    {
        estadoActual = estado.Desactivado;
		Camera.main.backgroundColor = Color.black;
        
        // Si el beat más cercano ya se nos ha pasado del todo por no hacer clic (Miss / Fallo)
        if (tiempoActual > tiempoObjetivo + margenMal)
        {
            indiceClick++; 
			clicado = false;
        }
    }
}

	void Update()
{
    if (Conductor.Instance != null)
    {
        ActualizarEstado(Conductor.Instance.TiempoCancion);
    }

    // Solo procesamos el input si la nota actual NO ha sido clicada aún
    if (Input.GetMouseButtonDown(0))
    {

		if (!clicado) {

			if (estadoActual == estado.Perfecto || estadoActual == estado.Bien || estadoActual == estado.Mal)
			{
				// Bloqueamos la nota para el resto de su duración temporal
				clicado = true; 
				
				// Evaluamos la puntuación
				if (estadoActual == estado.Perfecto) {Debug.Log("Perfecto!"); puntuacion += 10;}
				else if (estadoActual == estado.Bien) {Debug.Log("Bien!"); puntuacion += 2;}
				else if (estadoActual == estado.Mal) {Debug.Log("Mal..."); puntuacion -= 4;}
			}
			else if (estadoActual == estado.Desactivado)
			{
				Debug.Log("Spam / Fuera de ritmo");
			}

		}
		else {
			puntuacion -= 4;
		}

        
    }
}

	
}
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class LineaTiempo : MonoBehaviour
{
    [Header("Datos Cargados")]
    public List<DatosClic> clicsDelNivel = new List<DatosClic>();

    [Header("Configuración")]
    public string nombreArchivoACargar = "NivelGrabado 3.json";

    public enum estado { Desactivado, Mal, Bien, Perfecto }

    public double margenMal = 0.2;
    public double margenBien = 0.1;
    public double margenPerfecto = 0.05;

    [Header("Partida")]
    public int puntuacion = 0;

    public void CargarNivel(string nombreArchivo)
    {
        string ruta = Path.Combine(Application.dataPath, "Niveles", nombreArchivo);

        if (!File.Exists(ruta))
        {
            Debug.LogError($"No se encontró el archivo de nivel en: {ruta}");
            return;
        }

        ContenedorClics datos = JsonUtility.FromJson<ContenedorClics>(File.ReadAllText(ruta));
        if (datos != null)
        {
            clicsDelNivel = datos.listaClics;
            Debug.Log($"Nivel cargado. Total de clics: {clicsDelNivel.Count}");
        }
    }

    public void setLevel(string nombreArchivo) => nombreArchivoACargar = nombreArchivo;

    public estado EvaluarTiempo(double tiempoObjetivo, double tiempoActual)
    {
        double distancia = Math.Abs(tiempoObjetivo - tiempoActual);

        if (distancia <= margenPerfecto) return estado.Perfecto;
        if (distancia <= margenBien)     return estado.Bien;
        if (distancia <= margenMal)      return estado.Mal;
        return estado.Desactivado;
    }

    // Único sitio donde se suman puntos. Lo llama el pan.
    public void RegistrarResultado(estado resultado)
    {
        switch (resultado)
        {
            case estado.Perfecto: puntuacion += 10; Debug.Log("Perfecto!"); break;
            case estado.Bien:     puntuacion += 2;  Debug.Log("Bien!");     break;
            case estado.Mal:      puntuacion -= 4;  Debug.Log("Mal...");    break;
            case estado.Desactivado: puntuacion -= 4; Debug.Log("Fallo (pan sin cortar)"); break;
        }
    }
}
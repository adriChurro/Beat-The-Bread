using UnityEngine;
using System.Collections.Generic;

public class GeneradorPan : MonoBehaviour
{
    [Header("Referencias")]
    public LineaTiempo lineaTiempo;
    public GameObject panPrefab;

    [Header("Configuración")]
    public TipoPan[] tipos;
    public float velocidadPan = 4f;
    public float distanciaSpawn = 8f;

    int siguiente; //El click por el que vamos
    bool activo = true;

    public void Iniciar()
    {
        siguiente = 0;
        activo = true;
    }

    public void Detener() => activo = false;

    void Update()
    {
        if(!activo) return;

        List<DatosClic> clics = lineaTiempo.clicsDelNivel;
        double tiempoViaje = distanciaSpawn/velocidadPan; // el pan debe de empezar a moverse antes de que llegue el click

        //siempre que estemos dentro de la lista clicks y y el tiempo de la cancion sea mayor que tal... que pereza de verdad lees esto?
        if (siguiente < clics.Count && Conductor.Instance.TiempoCancion >= clics[siguiente].tiempoCancion - tiempoViaje)
        {
            Generar(clics[siguiente]);
            siguiente++;
        }
    }

    void Generar(DatosClic clic)
    {
        Vector2 posInicial = new Vector2(0, -3.35f) + Vector2.left * distanciaSpawn; // cambiar y algo mas general
        GameObject pan = Instantiate(panPrefab, posInicial, Quaternion.identity);

        pan.GetComponent<Pan>().Configurar(clic.tiempoCancion, velocidadPan);
    }
}

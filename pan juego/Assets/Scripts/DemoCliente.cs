using UnityEngine;
using System.Collections.Generic;

public class DemoCliente : MonoBehaviour
{
    [Header("Referencias")]
    public LineaTiempo lineaTiempo;
    public GameObject panPrefab;
    public GeneradorPan generador;

    [Header("Configuración")]
    public float adelantoBeats = 4f;
    public float tiempoHastaCorte = 0f;
    public bool remplazarAnterior = true;
    public float offsetY = 4f;

    Vector3 posBase;

    int siguiente;
    bool activo = true;
    GameObject actual;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posBase = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!activo) return;
        List<DatosClic> clics = lineaTiempo.clicsDelNivel;
        double t = Conductor.Instance.TiempoCancion;
        double adelanto = adelantoBeats * Conductor.Instance.SegundosPorBeat;

        if (siguiente < clics.Count && t >= clics[siguiente].tiempoCancion - adelanto - tiempoHastaCorte) 
        {
            Lanzar(clics[siguiente]);
            siguiente++;
        }
    }

    void Lanzar (DatosClic clic)
    {
        TipoPan tipo = generador.porDireccion[clic.direccion];

        if (remplazarAnterior && actual != null) Destroy(actual);

        actual = Instantiate(tipo.panPrefab, posBase + Vector3.up * offsetY, Quaternion.identity);
        
        actual.GetComponent<Animator>().SetTrigger("Cut");
        actual.transform.localScale = new Vector3(1f, 1f, 1f);
        actual.GetComponent<Pan>().demostracion = true;
        actual.GetComponent<IdleBeat>().activo = false;
    }
}

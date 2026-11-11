using UnityEngine;
using System.Collections.Generic;

public class DemoCliente : MonoBehaviour
{
    [Header("Referencias")]
    public LineaTiempo lineaTiempo;
    public TipoPan[] tipos;
    public GameObject panPrefab;
    public float offsetY = 4f;

    [Header("Configuración")]
    public float adelantoBeats = 4f;
    public float tiempoHastaCorte = 0f;
    public bool remplazarAnterior = true;

    int siguiente;
    bool activo = true;
    GameObject actual;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        if (remplazarAnterior && actual != null) Destroy(actual);

        actual = Instantiate(panPrefab, transform.position + Vector3.up * offsetY, Quaternion.identity);
        
        actual.GetComponent<Animator>().SetTrigger("Cut");
        actual.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        actual.GetComponent<Pan>().demostracion = true;
    }
}

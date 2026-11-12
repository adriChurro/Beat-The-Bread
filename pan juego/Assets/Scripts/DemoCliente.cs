using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemoCliente : MonoBehaviour
{
    [Header("Referencias")]
    public LineaTiempo lineaTiempo;
    public GameObject panPrefab;
    public GeneradorPan generador;
    public Sprite[] clientes;

    [Header("Configuración")]
    public float adelantoBeats = 4f;
    public float tiempoHastaCorte = 0f;
    public bool remplazarAnterior = true;
    public float offsetY = 4f;

    [Header("Ritmo")]
    public int beatsPorCambio = 8;         // 2 compases de 4/4
    public int beatsCuentaInicial = 4;

    [Header("Movimiento")]
    public float distanciaFuera = 12f;     // cuánto se aleja hacia la derecha (fuera de cámara)
    public float duracionSalida = 0.12f;
    public float duracionEntrada = 0.12f;
    public bool entraPorIzquierda = false; // false: vuelve desde la derecha, true: aparece desde la izquierda

    SpriteRenderer spriteCliente;

    Vector3 posBase;
    GameObject actual;

    int siguiente;
    bool activo = true;
    int indiceSprite;
    bool cambiando;
    bool nivelTerminado;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posBase = transform.position;
        Conductor.Instance.AlCambiarBeat += OnBeat;
        spriteCliente = GetComponent<SpriteRenderer>();
        Conductor.Instance.AlEmpezarCancion += EmpezarCambio;
    }

    void EmpezarCambio()
    {
        StartCoroutine(Cambiar());
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

        if (Conductor.Instance.TiempoCancion > TiempoUltimoRitmo() + lineaTiempo.margenMal)
            StartCoroutine(Despedir());
    }

    void Lanzar (DatosClic clic)
    {
        TipoPan tipo = generador.porDireccion[clic.direccion];

        if (remplazarAnterior && actual != null) Destroy(actual);

        actual = Instantiate(tipo.panPrefab, posBase + Vector3.up * offsetY +  Vector3.right * 5f, Quaternion.identity);
        
        actual.GetComponent<Animator>().SetTrigger("Cut");
        actual.transform.localScale = new Vector3(1f, 1f, 1f);
        actual.GetComponent<Pan>().demostracion = true;
        actual.GetComponent<IdleBeat>().activo = false;
    }

    void OnBeat(int beat)
    {
        if (nivelTerminado || cambiando) return;

        // Pasado el último ritmo, no entra ningún cliente nuevo
        if (Conductor.Instance.TiempoCancion > TiempoUltimoRitmo()) return;

        if (beat > beatsCuentaInicial &&
            (beat - beatsCuentaInicial) % beatsPorCambio == 0 &&
            !cambiando)
        {
            StartCoroutine(Cambiar());
        }
    }

    IEnumerator Despedir()
    {
        nivelTerminado = true;
        cambiando = true;

        // Sale hacia la derecha igual que en un cambio, pero ya no vuelve
        yield return Mover(posBase, posBase + Vector3.right * distanciaFuera, duracionSalida, true);
        spriteCliente.enabled = false;
    }

    IEnumerator Cambiar()
    {
        cambiando = true;
        Vector3 derecha = posBase + Vector3.right * distanciaFuera;
        Vector3 izquierda = posBase + Vector3.left * distanciaFuera;

        // 1. Sale rápido hacia la derecha (aceleración)
        yield return Mover(posBase, derecha, duracionSalida, true);

        // 2. Cambia el sprite fuera de cámara
        if (clientes.Length > 0)
        {
            indiceSprite = (indiceSprite + 1) % clientes.Length;
            spriteCliente.sprite = clientes[indiceSprite];
        }

        // 3. Vuelve al centro rápido (frenada)
        Vector3 origen = entraPorIzquierda ? izquierda : derecha;
        yield return Mover(origen, posBase, duracionEntrada, false);

        transform.position = posBase;
        cambiando = false;
    }

    IEnumerator Mover(Vector3 desde, Vector3 hasta, float duracion, bool acelerar)
    {
        float t = 0f;
        while (t < duracion)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duracion);
            k = acelerar ? k * k : 1f - (1f - k) * (1f - k); // ease-in al salir, ease-out al entrar
            transform.position = Vector3.LerpUnclamped(desde, hasta, k);
            yield return null;
        }
        transform.position = hasta;
    }

    double TiempoUltimoRitmo()
    {
        var clics = lineaTiempo.clicsDelNivel;
        return clics.Count == 0 ? 0 : clics[clics.Count - 1].tiempoCancion + lineaTiempo.margenMal;
    }
}

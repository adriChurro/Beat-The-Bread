using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.InputSystem;

[System.Serializable]
public class DatosClic
{
    public Tipo tipo;
    public Vector2 posicion;
    public Vector2 trazo;
    public DireccionCorte direccion;
    public double tiempoCancion;
    public double tiempoCancionFinal;
    public double beat;
    public double beatFinal;
}

public enum Tipo { Click, Corte, Arrastre }

[System.Serializable]
public class ContenedorClics
{
    public List<DatosClic> listaClics = new List<DatosClic>();
}

public class GrabadorRaton : MonoBehaviour
{
    [Tooltip("Distancia mínima (en unidades de mundo) para considerar que es un corte y no un click.")]
    public float distanciaMinima = 0.5f;

    [Tooltip("Margen de ángulo (en grados) alrededor de cada dirección. 22.5 = 4 zonas iguales.")]
    [Range(10f, 30f)]
    public float tolerancia = 22.5f;

    public ContenedorClics registro = new ContenedorClics();
    Vector2 inicio;
    Vector2 fin;
    Vector2 trazo;
    double tiempoCancionInicio;
    double tiempoCancionFinal;  
    double beatInicio;
    double beatFinal;
    bool arrastrando;
    
    void Update()
    {
        if (AdministradorJuego.Instance == null || AdministradorJuego.Instance.estadoActual != AdministradorJuego.EstadoJuego.Admin) {return;}

        if (Input.GetKeyDown(KeyCode.G))
        {
            GuardarEnJson();
        }

        if (PulsadoEsteFrame())
        {
            inicio = PosicionMundo();
            arrastrando = true;
            tiempoCancionInicio = Conductor.Instance.TiempoCancion;
            beatInicio = Conductor.Instance.BeatActual;
        }

        // Si no estamos soltando el ratón en este frame, salimos del Update (no pasa nada con la G porque ya la comprobamos arriba)
        if (!(arrastrando && SoltadoEsteFrame())) return;

        arrastrando = false;
        fin = PosicionMundo();

        trazo = fin - inicio; 
        
        if (trazo.magnitude < distanciaMinima) 
        {
            RegistrarClic();
        }
        else
        {
            tiempoCancionFinal = Conductor.Instance.TiempoCancion;
            beatFinal = Conductor.Instance.BeatActual;
            RegistrarCorte();
        }
    }

    void RegistrarClic()
    {
        Vector2 posRaton = Input.mousePosition; 
        
        DatosClic nuevoClic = new DatosClic();
        nuevoClic.tipo = Tipo.Click;
        nuevoClic.posicion = posRaton;

        if (Conductor.Instance != null)
        {
            nuevoClic.tiempoCancion = Conductor.Instance.TiempoCancion;
            nuevoClic.beat = Conductor.Instance.BeatActual;
        }

        registro.listaClics.Add(nuevoClic);
        Debug.Log($"Clic guardado. Total en memoria: {registro.listaClics.Count}");
    }

    void RegistrarCorte()
    {
        DatosClic nuevoCorte = new DatosClic();
        nuevoCorte.tipo = Tipo.Corte;
        nuevoCorte.posicion = inicio;
        nuevoCorte.trazo = trazo;
        nuevoCorte.direccion = Clasificar(trazo);
        nuevoCorte.tiempoCancion = tiempoCancionInicio;
        nuevoCorte.tiempoCancionFinal = tiempoCancionFinal;
        nuevoCorte.beat = beatInicio;
        nuevoCorte.beatFinal = beatFinal;

        registro.listaClics.Add(nuevoCorte);
        Debug.Log($"Corte guardado. Total en memoria: {registro.listaClics.Count}");
    }

    void GuardarEnJson()
    {
        string rutaCarpeta = Path.Combine(Application.dataPath, "Niveles");
        
        if (!Directory.Exists(rutaCarpeta))
        {
            Directory.CreateDirectory(rutaCarpeta);
        }

        string[] archivos = Directory.GetFiles(rutaCarpeta, "*.json");
        int numeroDeArchivos = archivos.Length;
        
        string nombreArchivo = $"NivelGrabado_{numeroDeArchivos + 1}.json";
        string ruta = Path.Combine(rutaCarpeta, nombreArchivo);

        string contenidoJson = JsonUtility.ToJson(registro, true);
        File.WriteAllText(ruta, contenidoJson);
        
        Debug.Log($"<color=green>¡ARCHIVO GUARDADO EN:</color> {ruta}");
    }

    bool PulsadoEsteFrame()
    {
        return Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
    }

    bool SoltadoEsteFrame()
    {
        return Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame;
    }

    Vector2 PosicionMundo()
    {
        Vector2 pantalla = Mouse.current.position.ReadValue();
        return Camera.main.ScreenToWorldPoint(pantalla);
    }

    public DireccionCorte Clasificar(Vector2 trazo)
    {
        float ang = AnguloNormalizado(trazo);

        if (ang < tolerancia || ang >= 180f - tolerancia) return DireccionCorte.Horizontal;
        if (Mathf.Abs(ang - 90f) < tolerancia) return DireccionCorte.Vertical;
        return ang < 90f ? DireccionCorte.DiagonalA : DireccionCorte.DiagonalB;
    }

    float AnguloNormalizado(Vector2 d)
    {
        float ang = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;
        if (ang < 0f) ang += 180f; 
        if (ang >= 180f) ang -= 180f; 
        return ang;
    }
}
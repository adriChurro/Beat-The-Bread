using System;
using UnityEngine;
 
public class Conductor : MonoBehaviour
{
    public static Conductor Instance { get; private set; }
 
    [Header("Música")]
    public AudioSource musica;
    public float bpm = 120f;
 
    [Tooltip("Segundos desde el inicio del audio hasta el primer beat real.")]
    public float offsetPrimerBeat = 0.81f;
 
    [Tooltip("Margen en segundos antes de que empiece a sonar la música.")]
    public float retrasoInicio = 0f;
 
    [Header("Depuración")]
    public bool mostrarTiempo = true;
    public bool clicEnCadaBeat = true;
    public AudioSource sonidoClic;
    public AudioClip clic;
 
    // Se dispara una vez por cada beat entero (0, 1, 2...)
    public event Action<int> AlCambiarBeat;
 
    double inicioDsp;
    bool iniciado;
    int ultimoBeat = -1;
 
    // ---------- Lo que leerán los demás scripts ----------
 
    public double SegundosPorBeat => 60.0 / bpm;
 
    // Segundos de canción. Es negativo antes de que empiece a sonar.
    public double TiempoCancion => iniciado ? AudioSettings.dspTime - inicioDsp : -retrasoInicio;
 
    // Beat actual con decimales (2.5 = mitad entre el beat 2 y el 3).
    public double BeatActual => TiempoABeat(TiempoCancion);
 
    public double BeatATiempo(double beat) => offsetPrimerBeat + beat * SegundosPorBeat;
    public double TiempoABeat(double tiempo) => (tiempo - offsetPrimerBeat) / SegundosPorBeat;
 
    // ---------- Ciclo de vida ----------
 
    void Awake()
    {
        Instance = this;
    }
 
    void Start()
    {
        Iniciar();
    }
 
    public void Iniciar()
    {
        inicioDsp = AudioSettings.dspTime + retrasoInicio;
        musica.PlayScheduled(inicioDsp);
        ultimoBeat = -1;
        iniciado = true;
    }
 
    void Update()
    {
        if (!iniciado) return;
 
        int beatEntero = Mathf.FloorToInt((float)BeatActual);
 
        // El while evita saltarse un beat si un frame tarda más de la cuenta
        while (ultimoBeat < beatEntero)
        {
            ultimoBeat++;
            AlCambiarBeat?.Invoke(ultimoBeat);
 
            if (clicEnCadaBeat && sonidoClic != null && clic != null)
                sonidoClic.PlayOneShot(clic);
        }
    }
 
    void OnGUI()
    {
        if (!mostrarTiempo) return;
 
        var estilo = new GUIStyle(GUI.skin.label) { fontSize = 24 };
        GUI.Label(new Rect(10, 10, 500, 40), $"Tiempo: {TiempoCancion:F3} s", estilo);
        GUI.Label(new Rect(10, 45, 500, 40), $"Beat: {BeatActual:F2}", estilo);
    }
}
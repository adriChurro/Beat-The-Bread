using UnityEngine;

public class EscalaPorVolumen : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("El AudioSource que está reproduciendo la música")]
    public AudioSource fuenteAudio;

    [Header("Configuración de Ritmo")]
    [Tooltip("Volumen mínimo necesario para que empiece a crecer")]
    public float umbralVolumen = 0.05f; 
    [Tooltip("Cuánto va a crecer el objeto por cada punto de volumen extra")]
    public float multiplicadorEscala = 10f; 

    [Header("Escala y Suavizado")]
    [Tooltip("El tamaño normal del objeto cuando no hay música o es muy baja")]
    public Vector3 escalaBase = Vector3.one; 
    [Tooltip("Velocidad a la que crece y vuelve a su tamaño normal (mayor = más rápido)")]
    public float velocidadSuavizado = 15f;

    // Array para guardar los datos de audio. 256 es un buen equilibrio entre precisión y rendimiento.
    private float[] muestras = new float[256];

    void Update()
    {
        if (fuenteAudio == null || !fuenteAudio.isPlaying) return;

        fuenteAudio.GetOutputData(muestras, 0);

        float suma = 0;
        for (int i = 0; i < muestras.Length; i++)
        {
            suma += muestras[i] * muestras[i];
        }
        float volumenRMS = Mathf.Sqrt(suma / muestras.Length);

        Vector3 escalaObjetivo = escalaBase;

        if (volumenRMS > umbralVolumen)
        {
            // Solo escalamos la parte del volumen que supera nuestro umbral
            float extra = (volumenRMS - umbralVolumen) * multiplicadorEscala;
            escalaObjetivo = escalaBase + new Vector3(extra, extra, extra);
        }

        transform.localScale = Vector3.Lerp(transform.localScale, escalaObjetivo, Time.deltaTime * velocidadSuavizado);
    }
}
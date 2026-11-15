using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ParpadeoSuave : MonoBehaviour
{
    [Tooltip("Velocidad a la que la imagen aparece y desaparece")]
    public float velocidad = 2f;
    
    [Tooltip("Transparencia mínima (0 = invisible, 1 = opaco)")]
    [Range(0f, 1f)] public float alfaMinimo = 0.2f;
    
    [Tooltip("Transparencia máxima (0 = invisible, 1 = opaco)")]
    [Range(0f, 1f)] public float alfaMaximo = 1f;

    private Image imagen;
    private Color colorOriginal;

    void Start()
    {
        imagen = GetComponent<Image>();
        colorOriginal = imagen.color;
    }

    void Update()
    {
        // Mathf.Sin crea una onda suave que sube y baja con el tiempo.
        // Lo ajustamos para que oscile entre el alfa mínimo y máximo.
        float onda = (Mathf.Sin(Time.time * velocidad) + 1f) / 2f; 
        float alfaActual = Mathf.Lerp(alfaMinimo, alfaMaximo, onda);

        // Aplicamos la nueva transparencia manteniendo los colores originales
        imagen.color = new Color(colorOriginal.r, colorOriginal.g, colorOriginal.b, alfaActual);
    }
}
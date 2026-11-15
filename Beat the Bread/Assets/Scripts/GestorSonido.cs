using UnityEngine;

public class GestorSonido : MonoBehaviour
{
    public static GestorSonido Instance { get; private set; }

    [Header("Fuente (distinta de la música)")]
    public AudioSource fuente;

    [Header("Clips")]
    public AudioClip pop;
    public AudioClip win;
    public AudioClip loose;

    [Header("Variación")]
    [Range(0f, 0.2f)] public float variacionPitch = 0.05f;

    void Awake() => Instance = this;

    public void Reproducir(AudioClip clip, float volumen = 1f)
    {
        if (clip == null || fuente == null) return;

        fuente.pitch = 1f + Random.Range(-variacionPitch, variacionPitch);
        fuente.PlayOneShot(clip, volumen);
    }
}
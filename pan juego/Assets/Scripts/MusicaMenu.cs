using UnityEngine;
using System.Collections;

public class MusicaMenu : MonoBehaviour
{
    public AudioSource fuente;
    public float duracionFundido = 0.5f;

    float volumenOriginal;
    Coroutine fundido;

    void Awake()
    {
        volumenOriginal = fuente.volume;
    }

    public void Sonar()
    {
        if (fundido != null) StopCoroutine(fundido);
        fuente.volume = volumenOriginal;
        if (!fuente.isPlaying) fuente.Play();
    }

    public void Parar()
    {
        if (fundido != null) StopCoroutine(fundido);
        fundido = StartCoroutine(FundidoYParar());
    }

    IEnumerator FundidoYParar()
    {
        float inicio = fuente.volume;
        for (float t = 0; t < duracionFundido; t += Time.deltaTime)
        {
            fuente.volume = Mathf.Lerp(inicio, 0f, t / duracionFundido);
            yield return null;
        }
        fuente.Stop();
        fuente.volume = volumenOriginal;
    }
}
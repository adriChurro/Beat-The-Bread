using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControladorEscenas : MonoBehaviour
{
    [Header("Configuración de Transición")]
    [Tooltip("El AudioSource que reproduce la música del menú")]
    public AudioSource musicaMenu;
    [Tooltip("Segundos que tardará la música en apagarse y cambiar de escena")]
    public float tiempoDesvanecimiento = 2f;

    public void CambiarA(string nombreDeLaEscena)
    {
        StartCoroutine(BajarVolumenYCargar(nombreDeLaEscena));
    }

    private IEnumerator BajarVolumenYCargar(string nombreEscena)
    {
        if (musicaMenu != null)
        {
            float volumenInicial = musicaMenu.volume;
            float tiempoPasado = 0f;

            while (tiempoPasado < tiempoDesvanecimiento)
            {
                tiempoPasado += Time.deltaTime;
                musicaMenu.volume = Mathf.Lerp(volumenInicial, 0f, tiempoPasado / tiempoDesvanecimiento);
                yield return null;
            }
            
            musicaMenu.volume = 0f;
        }
        else
        {
            yield return new WaitForSeconds(tiempoDesvanecimiento);
        }

        SceneManager.LoadScene(nombreEscena);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Obligatorio para poder usar Corrutinas

public class ControladorEscenas : MonoBehaviour
{
    [Header("Configuración de Transición")]
    [Tooltip("El AudioSource que reproduce la música del menú")]
    public AudioSource musicaMenu;
    [Tooltip("Segundos que tardará la música en apagarse y cambiar de escena")]
    public float tiempoDesvanecimiento = 2f;

    // Esta es la función que llama tu botón
    public void CambiarA(string nombreDeLaEscena)
    {
        // En lugar de cargar la escena de golpe, iniciamos la secuencia progresiva
        StartCoroutine(BajarVolumenYCargar(nombreDeLaEscena));
    }

    private IEnumerator BajarVolumenYCargar(string nombreEscena)
    {
        // Si hemos asignado música, la bajamos poco a poco
        if (musicaMenu != null)
        {
            float volumenInicial = musicaMenu.volume;
            float tiempoPasado = 0f;

            while (tiempoPasado < tiempoDesvanecimiento)
            {
                tiempoPasado += Time.deltaTime;
                // Lerp calcula el valor intermedio entre el volumen inicial y 0
                musicaMenu.volume = Mathf.Lerp(volumenInicial, 0f, tiempoPasado / tiempoDesvanecimiento);
                yield return null; // Espera al siguiente frame antes de continuar el bucle
            }
            
            musicaMenu.volume = 0f; // Aseguramos que quede a 0 al final
        }
        else
        {
            // Si te olvidaste de asignar la música, simplemente espera el tiempo antes de cambiar
            yield return new WaitForSeconds(tiempoDesvanecimiento);
        }

        // Una vez terminada la música y el tiempo, cargamos la escena
        SceneManager.LoadScene(nombreEscena);
    }
}
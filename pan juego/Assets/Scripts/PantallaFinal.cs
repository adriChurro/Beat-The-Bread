using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PantallaFinal : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject lineaTiempo;
    public TMP_Text textoVeredicto;
    public GameObject siguiente;
    public GameObject reiniciar;

    [Header("Juicio")]
    [Range(0f, 1f)] public float porcentajeParaAprobar = 0.5f;  // 50% de la puntuación máxima
    public string textoApto = "¡APTO! Estás contratado.";
    public string textoNoApto = "NO APTO. Vuelve cuando sepas cortar pan.";

    bool apto;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Conductor.Instance.AlTerminarCancion += OnFinCancion;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnFinCancion ()
    {
        int puntos = lineaTiempo.GetComponent<LineaTiempo>().puntuacion;
        int maximo = lineaTiempo.GetComponent<LineaTiempo>().clicsDelNivel.Count * 10;          // todo Perfecto
        float porcentaje = maximo > 0 ? (float)puntos / maximo : 0f;

        apto = porcentaje >= porcentajeParaAprobar;

        textoVeredicto.text = apto ? textoApto : textoNoApto;

        

        GetComponent<Animator>().SetTrigger("Fin");
    }

    void mostrarBotones()
    {
        if (apto) siguiente.SetActive(true);

        reiniciar.SetActive(true);
    }

    public void reiniciarNivel () 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SonarPop()
    {
        GestorSonido.Instance.Reproducir(GestorSonido.Instance.pop);
    }
}

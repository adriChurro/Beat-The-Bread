using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dialogo : MonoBehaviour
{
    [SerializeField] private TMP_Text textoDialogo;
    [SerializeField] private GameObject imagenRaton;
    public GameObject demoCliente;

    private string[] frases;
    private int indice = 0;
    bool empezado;

    void Start()
    {
        string nivel = "";
        switch(SceneManager.GetActiveScene().name)
        {
            case "Nivel1":
                nivel = "Nivel1";
                break;
            case "Nivel2":
                nivel = "Nivel2";
                break;
            case "Nivel3":
                nivel = "Nivel3";
                break;
        }
        TextAsset archivo = Resources.Load<TextAsset>(nivel);

        frases = archivo.text.Split('\n');

        MostrarFrase();
    }

    void Update()
    {
        if (empezado) return;

        // Se cambia Input.GetKeyDown(KeyCode.Space) por Input.GetMouseButtonDown(0)
        // El 0 representa el clic izquierdo del ratón.
        if (Input.GetMouseButtonDown(0))
        {
            if (indice >= frases.Length)
            {
                AdministradorJuego.Instance.CambiarEstado(AdministradorJuego.EstadoJuego.Jugando);
                textoDialogo.gameObject.SetActive(false);
                imagenRaton.SetActive(false);
                empezado = true;
            }
            MostrarFrase();
        }
    }

    public void MostrarFrase()
    {
        if (indice < frases.Length)
        {
            if (frases[indice].Trim().StartsWith("/"))
            {
                textoDialogo.text = "";

                DatosClic clic = new DatosClic();

                switch (frases[indice].Trim())
                {
                    case "/PanBimbo":
                        clic.direccion = DireccionCorte.Click; // Ejemplo
                        break;
                    case "/PanBarra":
                        clic.direccion = DireccionCorte.Horizontal; // Ejemplo
                        break;
                    case "/PanMolde":
                        clic.direccion = DireccionCorte.Vertical; // Ejemplo
                        break;
                }

                demoCliente.GetComponent<DemoCliente>().Lanzar(clic);
            }
            else
            {
                textoDialogo.text = frases[indice].Trim();
            }
            indice++;
        }
    }
}
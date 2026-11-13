using UnityEngine;
using TMPro;

public class Dialogo : MonoBehaviour
{
    [SerializeField] private TMP_Text textoDialogo;
    [SerializeField] private TMP_Text pulsarContinuar;
    public GameObject demoCliente;

    private string[] frases;
    private int indice = 0;
    bool empezado;

    void Start()
    {
        TextAsset archivo = Resources.Load<TextAsset>("Nivel1");

        frases = archivo.text.Split('\n');

        MostrarFrase();
    }

    void Update()
    {
        if (empezado) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (indice >= frases.Length)
            {
                AdministradorJuego.Instance.CambiarEstado(AdministradorJuego.EstadoJuego.Jugando);
                textoDialogo.gameObject.SetActive(false);
                pulsarContinuar.gameObject.SetActive(false);
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

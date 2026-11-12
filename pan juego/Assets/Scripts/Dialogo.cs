using UnityEngine;
using TMPro;

public class Dialogo : MonoBehaviour
{
    [SerializeField] private TMP_Text textoDialogo;
    [SerializeField] private TMP_Text pulsarContinuar;

    private string[] frases;
    private int indice = 0;

    void Start()
    {
        TextAsset archivo = Resources.Load<TextAsset>("Nivel1");

        frases = archivo.text.Split('\n');

        MostrarFrase();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (indice >= frases.Length)
            {
                AdministradorJuego.Instance.CambiarEstado(AdministradorJuego.EstadoJuego.Jugando);
                textoDialogo.gameObject.SetActive(false);
                pulsarContinuar.gameObject.SetActive(false);
            }
            MostrarFrase();
        }
    }

    public void MostrarFrase()
    {
        if (indice < frases.Length)
        {
            textoDialogo.text = frases[indice].Trim();
            indice++;
        }
    }
}

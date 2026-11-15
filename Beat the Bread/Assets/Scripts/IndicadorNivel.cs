using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IndicadorNivel : MonoBehaviour
{
    TMP_Text textoIndicador;

    void Start()
    {
        textoIndicador = GetComponent<TMP_Text>();

        string dia = "Dia " + SceneManager.GetActiveScene().name[^1];

        textoIndicador.text = dia;
    }
}

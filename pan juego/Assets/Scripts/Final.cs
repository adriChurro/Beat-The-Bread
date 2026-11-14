using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Final : MonoBehaviour
{
    bool menu; //pal menu
    bool siguiente;

    void Start()
    {
        
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!siguiente) return;

            if (!menu)
            {
                GetComponent<Animator>().SetTrigger("Siguiente");
                menu = true;
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }

            siguiente = false;
        }
    }

    void sePuedePasarAlSiguiente()
    {
        siguiente = true;
    }
}

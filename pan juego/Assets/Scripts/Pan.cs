using UnityEngine;

public class Pan : MonoBehaviour
{
    private Animator animator; //Componente animador

    public float velocidad = 2f;

    double tiempoObjetivo;

    bool cortado;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public void Configurar (double tiempo, float vel)
    {
        tiempoObjetivo = tiempo;
        velocidad = vel;
    }

    // Update is called once per frame
    void Update()
    {
        double t = Conductor.Instance.TiempoCancion;

        // Posición según el reloj de la música (llega a 0 justo en tiempoObjetivo)
        float x = 0f - (float)((tiempoObjetivo - t) * velocidad);

        // Si se pasa de 0, se queda en 0 y no esta cortado
        if (!cortado)  x = Mathf.Min(x, 0f);
    
        transform.position = new Vector3(x, -2.46f, transform.position.z);

        if (t > tiempoObjetivo + 0.2f && !cortado) Destroy(gameObject);
    }

    //si me clicas :)
    void OnMouseDown()
    {
        if (cortado) return;

        double t = Conductor.Instance.TiempoCancion;
        LineaTiempo.estado resultado = AdministradorJuego.Instance.lineaTiempo.EvaluarTiempo(tiempoObjetivo, t);

        if (resultado == LineaTiempo.estado.Perfecto || resultado == LineaTiempo.estado.Bien)
        {
            cortado = true;
            animator.SetTrigger("Cut");
        }
    }

    public void AlTerminarAnimacionCorte() //self explainatory
    {
        Destroy(gameObject);
    }
}

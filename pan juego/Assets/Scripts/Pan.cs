using UnityEngine;

public class Pan : MonoBehaviour
{
    private Animator animator; //Componente animador

    public float velocidad = 2f;
    double tiempoObjetivo;

    bool cortado;
    bool resuelto;
    public bool demostracion;

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
        if(demostracion == true || Conductor.Instance == null) return;

        double t = Conductor.Instance.TiempoCancion;

        // Posición según el reloj de la música (llega a 0 justo en tiempoObjetivo)
        float x = 0f - (float)((tiempoObjetivo - t) * velocidad);

        // Si se pasa de 0, se queda en 0 y no esta cortado
        if (!cortado)  x = Mathf.Min(x, 0f);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);

        // Pasado el margen de "Mal" sin cortar: fallo y desaparece
        LineaTiempo lt = AdministradorJuego.Instance.lineaTiempo;
        if (!cortado && t > tiempoObjetivo + lt.margenMal)
        {
            if (!resuelto)
            {
                resuelto = true;
                lt.RegistrarResultado(LineaTiempo.estado.Desactivado);
            }
            Destroy(gameObject);
        }
    }

    //si me clicas :)
    void OnMouseDown()
    {
        //Vamos a it filtrando casos
        if (resuelto || demostracion) return; // si ya esta cortado no entra

        LineaTiempo lt = AdministradorJuego.Instance.lineaTiempo;
        LineaTiempo.estado resultado = lt.EvaluarTiempo(tiempoObjetivo, Conductor.Instance.TiempoCancion);

        if (resultado == LineaTiempo.estado.Desactivado) return; // demasiado pronto: se ignora
  
        resuelto = true;
        lt.RegistrarResultado(resultado);

        if (resultado == LineaTiempo.estado.Mal) return; // penaliza y no se corta

        cortado = true;
        animator.SetTrigger("Cut");
        gameObject.GetComponent<IdleBeat>().activo = false;
    }

    public void AlTerminarAnimacionCorte() =>  Destroy(gameObject); //self explainatory
}

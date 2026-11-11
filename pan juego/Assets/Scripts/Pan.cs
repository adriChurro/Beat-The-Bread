using UnityEngine;

public class Pan : MonoBehaviour
{
    private Animator animator; //Componente animador

    public float velocidad = 2f;
    double tiempoObjetivo;

    bool cortado;
    bool resuelto;
    public bool demostracion;

    DireccionCorte direccionCorrecta;
    public DireccionCorte DireccionCorrecta => direccionCorrecta;
    public double TiempoObjetivo => tiempoObjetivo;
    public bool Resuelto => resuelto;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public void Configurar (DireccionCorte dir, double tiempo, float vel)
    {
        direccionCorrecta = dir;
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

    // El detector ya puntuó; aquí solo se reacciona
    public void Cortar()
    {
        resuelto = true;
        cortado = true;
        animator.SetTrigger("Cut");

        IdleBeat idle = GetComponent<IdleBeat>();
        if (idle != null) idle.activo = false;
    }

    public void Fallar()
    {
        resuelto = true; // ya puntuado; seguirá hasta desaparecer pasado margenMal
    }

    public void AlTerminarAnimacionCorte() => Destroy(gameObject);
}
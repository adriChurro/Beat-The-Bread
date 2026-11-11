using UnityEngine;
using UnityEngine.InputSystem;

public enum DireccionCorte
{
    Click,
    Vertical,
    Horizontal,
    DiagonalA, // "/"
    DiagonalB  // "\"
}

public class CortadorDireccion : MonoBehaviour
{
    [Tooltip("Distancia mínima (en unidades de mundo) para considerar que es un corte y no un click.")]
    public float distanciaMinima = 0.5f; //TODO ajustar parametro

    [Tooltip("Margen de ángulo (en grados) alrededor de cada dirección. 22.5 = 4 zonas iguales.")]
    [Range(10f, 30f)]
    public float tolerancia = 22.5f; //Número de grados que nos podemos permitir fallar
    //TODO probarlo un poco para ajustar su valor

    Vector2 inicio;
    Vector2 fin;
    bool arrastrando;

    //El update se ejecuta cada frame
    void Update()
    {

        //Comprobamos si empieza a pulsar justo en este frame
        if (PulsadoEsteFrame())
        {
            inicio = PosicionMundo(); //si es asi me guardo su posicion inicial
            arrastrando = true; // calramente estoy arrastrando :P
        }

        //si estaba arrastrando y justo suelto elclick en este frame
        if (arrastrando && SoltadoEsteFrame()) 
        {
            arrastrando = false; //dejo de arrastrar, obvio
            fin = PosicionMundo(); //guardo la posicion final del raton
            
            //detectamos si de verdad es un corte
            Vector2 trazo = fin - inicio;
            if (trazo.magnitude < distanciaMinima) return;

            DireccionCorte dir = Clasificar(trazo);
            Debug.Log($"Corte: {dir}  (ángulo {AnguloNormalizado(trazo):F1}°)");
            Debug.DrawLine(inicio, fin, Color.red, 1f); // dibuja trayectoria 

            // Comprobamos si ha cortado algo 
            RaycastHit2D hit = Physics2D.Linecast(inicio, fin, LayerMask.GetMask("Pan"));

            //no?? pos na macho otra vez sera
            if (hit.collider == null)
            {
                Debug.Log("No has tocado ningún pan");
                return;
            }

            //si estas aqui es que si
            //Pan pan = hit.collider.GetComponent<Pan>();
            /*//Lo has cortao por donde era??
            if (pan.tipo.direccionCorrecta == dir)
                Debug.Log($"ACIERTO con {pan.tipo.nombre}");
            else
                Debug.Log($"FALLO: {pan.tipo.nombre} pedía {pan.tipo.direccionCorrecta} y cortaste {dir}");*/
        }
    }

    // Devuelve un ángulo entre 0 y 180, ignorando el sentido del trazo.
    // Cortar de izquierda a derecha o de derecha a izquierda da el mismo resultado.
    // Claudeada de manual ni idea de como se hacia esto la verdad.
    float AnguloNormalizado(Vector2 d)
    {
        float ang = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg; //Al parecer Atan2 devuelve vel angulo de dos puntos en radianes y Mathf los pasa a grados, Un grande.
        if (ang < 0f) ang += 180f; //Normalizando...
        if (ang >= 180f) ang -= 180f; //Mas normalizacion...
        return ang;
    }

    public DireccionCorte Clasificar(Vector2 trazo)
    {
        float ang = AnguloNormalizado(trazo);

        // Horizontal: cerca de 0° o de 180°
        if (ang < tolerancia || ang >= 180f - tolerancia) return DireccionCorte.Horizontal;
        // Vertical: cerca de 90°
        if (Mathf.Abs(ang - 90f) < tolerancia) return DireccionCorte.Vertical;
        // Diagonales: lo que queda
        return ang < 90f ? DireccionCorte.DiagonalA : DireccionCorte.DiagonalB;
    }

    //Inputs

    bool PulsadoEsteFrame()
    {
        return Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
    }

    bool SoltadoEsteFrame()
    {
        return Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame;
    }

    Vector2 PosicionMundo()
    {
        Vector2 pantalla = Mouse.current.position.ReadValue();
        return Camera.main.ScreenToWorldPoint(pantalla);
    }
}
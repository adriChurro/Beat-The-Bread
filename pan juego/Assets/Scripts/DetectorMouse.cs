using UnityEngine;
using UnityEngine.InputSystem;

public class DetectorMouse : MonoBehaviour
{
    [Header("Configuración")]
    public float distanciaMinima = 0.5f;   // por debajo es un clic, por encima un corte
    [Range(10f, 30f)] public float tolerancia = 22.5f;

    Vector2 inicio, fin;
    double tiempoInicio;
    bool arrastrando;

    void Update()
    {
        if (AdministradorJuego.Instance == null ||
            AdministradorJuego.Instance.estadoActual != AdministradorJuego.EstadoJuego.Jugando)
        {
            arrastrando = false;
            return;
        }
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            inicio = PosicionMundo();
            tiempoInicio = Conductor.Instance.TiempoCancion;
            arrastrando = true;
        }

        if (arrastrando && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            arrastrando = false;
            fin = PosicionMundo();
            Resolver();
        }
    }

    void Resolver()
    {
        LineaTiempo lt = AdministradorJuego.Instance.lineaTiempo;

        // 1. ¿Clic o corte?
        Vector2 trazo = fin - inicio;
        DireccionCorte accion = trazo.magnitude < distanciaMinima
            ? DireccionCorte.Click
            : Clasificar(trazo);

        Debug.DrawLine(inicio, fin, Color.red, 1f);

        // 2. ¿Qué pan hay bajo el gesto?
        Pan pan = BuscarPan(accion);
        if (pan == null)
        {
            lt.RegistrarResultado(LineaTiempo.estado.Desactivado); // al vacío
            return;
        }

        // 3. ¿A tiempo?
        var timing = lt.EvaluarTiempo(pan.TiempoObjetivo, tiempoInicio);
        if (timing == LineaTiempo.estado.Desactivado)
        {
            lt.RegistrarResultado(LineaTiempo.estado.Desactivado); // demasiado pronto, el pan sigue
            return;
        }

        // 4. ¿Interacción correcta?
        if (accion != pan.DireccionCorrecta)
        {
            Debug.Log($"Interacción incorrecta: pedía {pan.DireccionCorrecta}, hiciste {accion}");
            lt.RegistrarResultado(LineaTiempo.estado.Mal);
            pan.Fallar();
            return;
        }

        // 5. Acierto (o Mal por timing)
        lt.RegistrarResultado(timing);
        if (timing == LineaTiempo.estado.Mal) pan.Fallar();
        else pan.Cortar();
    }

    // Entre los panes tocados, el más cercano en el tiempo y aún sin resolver
    Pan BuscarPan(DireccionCorte accion)
    {
        int mascara = LayerMask.GetMask("Pan");
        Collider2D[] golpes;

        if (accion == DireccionCorte.Click)
            golpes = Physics2D.OverlapPointAll(inicio, mascara);
        else
        {
            RaycastHit2D[] hits = Physics2D.LinecastAll(inicio, fin, mascara);
            golpes = new Collider2D[hits.Length];
            for (int i = 0; i < hits.Length; i++) golpes[i] = hits[i].collider;
        }

        Pan mejor = null;
        double mejorDist = double.MaxValue;
        foreach (Collider2D c in golpes)
        {
            if (!c.TryGetComponent(out Pan p) || p.Resuelto || p.demostracion) continue;
            double d = System.Math.Abs(p.TiempoObjetivo - tiempoInicio);
            if (d < mejorDist) { mejorDist = d; mejor = p; }
        }
        return mejor;
    }

    DireccionCorte Clasificar(Vector2 trazo)
    {
        float ang = Mathf.Atan2(trazo.y, trazo.x) * Mathf.Rad2Deg;
        if (ang < 0f) ang += 180f;
        if (ang >= 180f) ang -= 180f;

        if (ang < tolerancia || ang >= 180f - tolerancia) return DireccionCorte.Horizontal;
        if (Mathf.Abs(ang - 90f) < tolerancia) return DireccionCorte.Vertical;
        return ang < 90f ? DireccionCorte.DiagonalA : DireccionCorte.DiagonalB;
    }

    Vector2 PosicionMundo() =>
        Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
}

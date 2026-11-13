using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TajoVisual : MonoBehaviour
{
    public float duracion = 0.18f;
    public float anchoInicial = 0.25f;

    LineRenderer lr;
    float t;
    Color color;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.useWorldSpace = true;
    }

    public void Mostrar(Vector2 inicio, Vector2 fin, Color c)
    {
        color = c;
        lr.SetPosition(0, inicio);
        lr.SetPosition(1, fin);
    }

    void Update()
    {
        t += Time.deltaTime;
        float k = Mathf.Clamp01(t / duracion);

        lr.widthMultiplier = anchoInicial * (1f - k);            // se estrecha
        Color c = color; c.a = 1f - k;                           // se desvanece
        lr.startColor = lr.endColor = c;

        if (k >= 1f) Destroy(gameObject);
    }
}
using UnityEngine;

public class IdleBeat : MonoBehaviour
{
    [Header("Movimiento")]
    public float amplitud = 0.08f;
    public float beatsPorCiclo = 1f;

    Vector3 posBase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posBase = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float fase = 0f;

        if (Conductor.Instance.Sonando)
        {
            fase = (float) (Conductor.Instance.BeatActual / beatsPorCiclo);
        }
        else
        {
            fase = Time.time * 2 / beatsPorCiclo;
        }

        float onda = Mathf.Abs(Mathf.Sin(fase * Mathf.PI)); // bote: toca "suelo" en cada beat

        transform.localPosition = new Vector3(transform.localPosition.x, posBase.y, transform.localPosition.z) + Vector3.up * onda * amplitud;
    }
}

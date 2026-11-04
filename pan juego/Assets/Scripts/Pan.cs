using UnityEngine;

public class Pan : MonoBehaviour
{
    public TipoPan tipo;
    public float velocidad = 2f;

    void Start()
    {
        if (tipo != null) Init(tipo);
    }

    void Update()
    {
        transform.Translate(Vector2.right * velocidad * Time.deltaTime, Space.World); // mueve el pan
    }

    public void Init (TipoPan nuevoTipo) {
        tipo = nuevoTipo;
        GetComponent<SpriteRenderer>().sprite = tipo.sprite;
    }
}

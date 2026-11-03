using UnityEngine;

public class Pan : MonoBehaviour
{
    public TipoPan tipo;
    public float velocidad = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (tipo != null) Init(tipo);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * velocidad * Time.deltaTime, Space.World); // mueve el pan
    }

    public void Init (TipoPan nuevoTipo) {
        tipo = nuevoTipo;
        GetComponent<SpriteRenderer>().sprite = tipo.sprite;
    }
}

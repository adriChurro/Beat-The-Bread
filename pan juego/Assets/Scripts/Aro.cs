using UnityEngine;

public class Aro : MonoBehaviour
{
    public float velocidadEncogimiento = 0.5f;
    public float tamañoMinimo = 0.1f;

    
    void Update()
    {
		transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, velocidadEncogimiento * Time.deltaTime);
		
        if (transform.localScale.x <= tamañoMinimo)
        {
            Destroy(gameObject);
        }
    }
}
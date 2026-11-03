using UnityEngine;

public class GeneradorAros : MonoBehaviour
{
    [Header("Configuración")]
    
    public GameObject Aro; 
    
    public Vector2 spawnAreaSize = new Vector2(10f, 10f);
    
    private GameObject currentRing;

    void Update()
    {
        if (currentRing == null)
        {
            SpawnRing();
        }
    }

    void SpawnRing()
    {
        float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float randomY = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);

        Vector3 randomPosition = transform.position + new Vector3(randomX, randomY, 0);

        currentRing = Instantiate(Aro, randomPosition, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaSize.x, spawnAreaSize.y, 0));
    }
}
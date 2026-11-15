using UnityEngine;

[CreateAssetMenu(fileName = "TipoPan", menuName = "Scriptable Objects/TipoPan")]
public class TipoPan : ScriptableObject
{
    public string nombre;
    public GameObject panPrefab;
    public DireccionCorte direccionCorrecta;
}

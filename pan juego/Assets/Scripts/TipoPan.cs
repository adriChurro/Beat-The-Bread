using UnityEngine;

[CreateAssetMenu(fileName = "TipoPan", menuName = "Scriptable Objects/TipoPan")]
public class TipoPan : ScriptableObject
{
    public string nombre;
    public Sprite sprite;
    public DireccionCorte direccionCorrecta;
    public int cortes;
}

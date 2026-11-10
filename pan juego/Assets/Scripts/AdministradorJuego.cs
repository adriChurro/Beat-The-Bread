using UnityEngine;

// Resumen rápido. Un administrador de juego, que proporciona distintos estados,
// y los scripst reaccionan y se activan en función de ellos.
// por ejemplo, si el juego está en pausa, la musica no debe de sonar.

public class AdministradorJuego : MonoBehaviour
{

	public static AdministradorJuego Instance { get; private set; }

    // Los posibles estados en los que puede estar el juego
	public enum EstadoJuego
	{
		Esperando,
		Jugando,
		Admin
	}

	[Header("Estado Actual")]
	public EstadoJuego estadoActual = EstadoJuego.Esperando;

	// PONER AQUI LOS SCRIPS QUE UTILICE EL JUEGO
	[Header("Referencias")]
	public LineaTiempo lineaTiempo;
	public Conductor conductor;
	public GrabadorRaton grabadorRaton;
	public GeneradorPan generadorPan; 


	void Start()
	{
		Debug.Log("ENTER para jugar. D para debug");
	}


	void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // Si por error pones dos GameManagers en la escena, destruye el sobrante
            Destroy(gameObject); 
        }
    }


	void Update()
	{	
		// Acciones según el estado del juego
		switch (estadoActual)
		{
			case EstadoJuego.Esperando:
				if (Input.GetKeyDown(KeyCode.Return))
				{
					CambiarEstado(EstadoJuego.Jugando);
				}
				else if (Input.GetKeyDown(KeyCode.D))
				{
					CambiarEstado(EstadoJuego.Admin);
				}
				break;


			case EstadoJuego.Jugando:
				// TODO
				break;
			

			case EstadoJuego.Admin:
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					CambiarEstado(EstadoJuego.Esperando);
				}
				break;
		}
	}

	void CambiarEstado(EstadoJuego nuevoEstado)
	{
		estadoActual = nuevoEstado;

		if (estadoActual == EstadoJuego.Jugando)
		{
			lineaTiempo.CargarNivel(lineaTiempo.nombreArchivoACargar);
			conductor.Iniciar();
			Debug.Log("Estas Jugando");
		}
		else if (estadoActual == EstadoJuego.Admin)
		{
			conductor.Iniciar();
			Debug.Log("Estas en modo administrador");
		}
		else if (estadoActual == EstadoJuego.Esperando)
		{
			Debug.Log("Estas esperando");
		}
	}
}

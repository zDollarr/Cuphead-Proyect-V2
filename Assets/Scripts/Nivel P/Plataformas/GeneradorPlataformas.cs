using UnityEngine;

public class GeneradorPlataformas : MonoBehaviour
{
    [Header("Configuración Generación")]
    public GameObject[] prefabsPlataformas;
    public Transform jugador;
    public float distanciaMinima = 2f;
    public float distanciaMaxima = 5f;
    public float alturaMinima = 0.5f;
    public float alturaMaxima = 3f;
    public int plataformasIniciales = 5;

    [Header("Apilamiento")]
    [Range(0, 100)] public int probabilidadApilamiento = 20; // 20% de chance
    public float offsetVerticalApilamiento = 1f; // Distancia vertical entre plataformas apiladas

    private Vector2 ultimaPosicion;

    void Start()
    {
        ultimaPosicion = transform.position;
        GenerarPlataformasIniciales();
    }

    void GenerarPlataformasIniciales()
    {
        for (int i = 0; i < plataformasIniciales; i++)
        {
            GenerarPlataforma();
        }
    }

    void GenerarPlataforma()
    {
        float nuevaX = ultimaPosicion.x + Random.Range(distanciaMinima, distanciaMaxima);
        float nuevaY = Random.Range(alturaMinima, alturaMaxima);
        Vector2 nuevaPosicion = new Vector2(nuevaX, nuevaY);

        // Generar la plataforma base
        GenerarPlataformaEnPosicion(nuevaPosicion);

        // Verificar si debe apilarse otra plataforma encima (según probabilidad)
        if (Random.Range(0, 100) < probabilidadApilamiento)
        {
            Vector2 posicionApilada = new Vector2(nuevaPosicion.x, nuevaPosicion.y + offsetVerticalApilamiento);
            GenerarPlataformaEnPosicion(posicionApilada);
        }

        ultimaPosicion = nuevaPosicion;
    }

    void GenerarPlataformaEnPosicion(Vector2 posicion)
    {
        Collider2D colisionador = Physics2D.OverlapBox(posicion, new Vector2(1f, 1f), 0f);
        if (colisionador == null)
        {
            GameObject nuevaPlataforma = Instantiate(
                prefabsPlataformas[Random.Range(0, prefabsPlataformas.Length)],
                posicion,
                Quaternion.identity
            );

            Rigidbody2D rb = nuevaPlataforma.GetComponent<Rigidbody2D>();
            if (rb == null) rb = nuevaPlataforma.AddComponent<Rigidbody2D>();

            rb.linearDamping = 2f;
            rb.gravityScale = 1f;
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        if (jugador != null && jugador.position.x > ultimaPosicion.x - 10f)
        {
            GenerarPlataforma();
        }
    }
}
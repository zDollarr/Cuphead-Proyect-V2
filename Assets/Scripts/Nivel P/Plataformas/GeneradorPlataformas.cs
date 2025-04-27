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

    [Header("Plataformas Fijas")]
    [Range(0, 100)] public int probabilidadFija = 50; // 50% de chance de ser fija

    [Header("Generación hacia atrás")]
    public bool generarIzquierda = true; // Permitir generación hacia la izquierda
    public float distanciaGeneracionIzquierda = 10f; // Distancia para generar hacia la izquierda

    private Vector2 ultimaPosicionDerecha; // Última posición generada hacia la derecha
    private Vector2 ultimaPosicionIzquierda; // Última posición generada hacia la izquierda

    void Start()
    {
        ultimaPosicionDerecha = transform.position;
        ultimaPosicionIzquierda = transform.position;

        GenerarPlataformasIniciales();
    }

    void GenerarPlataformasIniciales()
    {
        // Generar plataformas iniciales hacia adelante
        for (int i = 0; i < plataformasIniciales; i++)
        {
            GenerarPlataformaDerecha();
        }

        // Generar plataformas iniciales hacia atrás si está habilitado
        if (generarIzquierda)
        {
            for (int i = 0; i < plataformasIniciales; i++)
            {
                GenerarPlataformaIzquierda();
            }
        }
    }

    void Update()
    {
        if (jugador == null) return;

        // Generar plataformas hacia adelante (derecha)
        if (jugador.position.x + 10f > ultimaPosicionDerecha.x)
        {
            GenerarPlataformaDerecha();
        }

        // Generar plataformas hacia atrás (izquierda)
        if (generarIzquierda && jugador.position.x - distanciaGeneracionIzquierda < ultimaPosicionIzquierda.x)
        {
            GenerarPlataformaIzquierda();
        }
    }

    void GenerarPlataformaDerecha()
    {
        float nuevaX = ultimaPosicionDerecha.x + Random.Range(distanciaMinima, distanciaMaxima);
        float nuevaY = Random.Range(alturaMinima, alturaMaxima);
        Vector2 nuevaPosicion = new Vector2(nuevaX, nuevaY);
        
        GenerarPlataformaEnPosicion(nuevaPosicion);
        ultimaPosicionDerecha = nuevaPosicion;
    }

    void GenerarPlataformaIzquierda()
    {
        float nuevaX = ultimaPosicionIzquierda.x - Random.Range(distanciaMinima, distanciaMaxima);
        float nuevaY = Random.Range(alturaMinima, alturaMaxima);
        Vector2 nuevaPosicion = new Vector2(nuevaX, nuevaY);

        GenerarPlataformaEnPosicion(nuevaPosicion);
        ultimaPosicionIzquierda = nuevaPosicion;
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

            // Decidir si la plataforma será fija o tendrá físicas activas
            if (Random.Range(0, 100) < probabilidadFija)
            {
                // Plataforma fija (sin físicas)
                Rigidbody2D rb = nuevaPlataforma.GetComponent<Rigidbody2D>();
                if (rb != null) Destroy(rb); // Eliminar Rigidbody2D si existe
            }
            else
            {
                // Plataforma con físicas activas
                Rigidbody2D rb = nuevaPlataforma.GetComponent<Rigidbody2D>();
                if (rb == null) rb = nuevaPlataforma.AddComponent<Rigidbody2D>();

                rb.linearDamping = 2f;
                rb.gravityScale = 1f;
                rb.freezeRotation = true;
            }}
    }
}


using UnityEngine;

public class CallesGenerator : MonoBehaviour{
    public GameObject[] prefabsCalles;
    public Transform jugador;

    [Header("Configuración de Generación")]
    public float distanciaMinimaX = 15f;  // Distancia mínima horizontal entre calles
    public float distanciaMaximaX = 22f;  // Distancia máxima horizontal entre calles
    public float variacionAltura = 1.2f;  // Máxima variación en altura
    public float alturaMaximaRelativa = 2f; // Altura máxima permitida relativa a la anterior
    public float probabilidadCallePegada = 0.15f; // 15% de chance de calles totalmente pegadas
    public float distanciaGeneracion = 10f;  // Cuando generar más
    public int callesIniciales = 5;          // Calles hacia derecha
    public bool generarAtras = true;         // Si genera hacia la izquierda
    public float distanciaEntreCalles = 7f;  // Separación de calles hacia atrás

    private Vector2 ultimaPosicionDerecha;
    private Vector2 ultimaPosicionIzquierda;
    private Vector2 posicionBase;
    private int _callesGeneradas = 0;

    void Start()
    {
        // Encontrar CalleBase en la escena
        GameObject calleBase = GameObject.Find("CalleBase");
        if (calleBase == null)
        {
            Debug.LogError("No se encontró CalleBase en la escena.");
            return;
        }

        posicionBase = calleBase.transform.position;
        ultimaPosicionDerecha = posicionBase;
        ultimaPosicionIzquierda = posicionBase;

        // Generar calles iniciales hacia la derecha
        for (int i = 0; i < callesIniciales; i++)
        {
            GenerarCalleDerecha();
        }

        // Generar calles iniciales hacia la izquierda si está activado
        if (generarAtras)
        {
            for (int i = 0; i < callesIniciales; i++)
            {
                GenerarCalleIzquierda();
            }
        }
    }

    void Update()
    {
        if (jugador == null) return;

        // Generar hacia derecha
        if (jugador.position.x + distanciaGeneracion > ultimaPosicionDerecha.x)
        {
            GenerarCalleDerecha();
        }

        // Generar hacia izquierda
        if (generarAtras && jugador.position.x - distanciaGeneracion < ultimaPosicionIzquierda.x)
        {
            GenerarCalleIzquierda();
        }
    }

    void GenerarCalleDerecha()
    {
        if (prefabsCalles.Length == 0) return;

        float nuevaX = ultimaPosicionDerecha.x + Random.Range(distanciaMinimaX, distanciaMaximaX);
        float nuevaY = CalcularNuevaAltura(ultimaPosicionDerecha.y);

        Vector2 nuevaPosicion = new Vector2(nuevaX, nuevaY);
        InstanciarCalle(nuevaPosicion);

        ultimaPosicionDerecha = nuevaPosicion;
        _callesGeneradas++;
    }

    void GenerarCalleIzquierda()
    {
        if (prefabsCalles.Length == 0) return;

        float nuevaX = ultimaPosicionIzquierda.x - Random.Range(distanciaMinimaX, distanciaMaximaX);
        float nuevaY = CalcularNuevaAltura(ultimaPosicionIzquierda.y);

        Vector2 nuevaPosicion = new Vector2(nuevaX, nuevaY);
        InstanciarCalle(nuevaPosicion);

        ultimaPosicionIzquierda = nuevaPosicion;
        _callesGeneradas++;
    }

    float CalcularNuevaAltura(float alturaReferencia)
    {
        float nuevaY = alturaReferencia;

        if (Random.value > probabilidadCallePegada)
        {
            float variacion = Random.Range(-variacionAltura, variacionAltura);

            // Si la variación es muy pequeña, forzarla para que no se superpongan
            if (Mathf.Abs(variacion) < 0.5f)
            {
                variacion = (variacion > 0 ? 1 : -1) * 0.5f;
            }

            nuevaY += variacion;

            // Limitar que la diferencia de altura no sea exagerada
            nuevaY = Mathf.Clamp(nuevaY, alturaReferencia - alturaMaximaRelativa, alturaReferencia + alturaMaximaRelativa);
        }

        // Evitar que esté muy cerca de la altura base
        if (Mathf.Abs(nuevaY - posicionBase.y) < 0.1f)
        {
            nuevaY += variacionAltura;
        }

        return nuevaY;
    }

    void InstanciarCalle(Vector2 posicion)
    {
        int indiceAleatorio = Random.Range(0, prefabsCalles.Length);
        Instantiate(prefabsCalles[indiceAleatorio], posicion, Quaternion.identity);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(ultimaPosicionDerecha, 0.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(ultimaPosicionIzquierda, 0.5f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(posicionBase, 0.5f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(posicionBase, ultimaPosicionDerecha);

        if (generarAtras)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(posicionBase, ultimaPosicionIzquierda);
        }
    }

}

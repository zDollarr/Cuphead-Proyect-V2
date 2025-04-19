using UnityEngine;

public class CallesGenerator : MonoBehaviour
{
    // Array de prefabs de calles (asignado desde el Inspector)
    public GameObject[] prefabsCalles;
    public Transform jugador;

    // Distancias y posiciones
    public float distanciaEntreCalles = 3f;
    public float distanciaGeneracion = 10f; // ¡Faltaba esta variable!
    public Vector2 posicionInicial = new Vector2(0, 0);

    // Control de generación
    public int callesIniciales = 5;
    private int _callesGeneradas = 0; // Contador interno

    void Start()
    {
        GenerarCallesIniciales();
    }

    void GenerarCallesIniciales()
    {
        Vector2 posicionActual = posicionInicial;

        for (int i = 0; i < callesIniciales; i++)
        {
            GenerarCalle(posicionActual);
            posicionActual += new Vector2(distanciaEntreCalles, 0);
            _callesGeneradas++;
        }
    }

    void GenerarCalle(Vector2 posicion)
    {
        if (prefabsCalles.Length == 0) return; // Evita errores si no hay prefabs

        int indiceAleatorio = Random.Range(0, prefabsCalles.Length);
        Instantiate(prefabsCalles[indiceAleatorio], posicion, Quaternion.identity);
    }

    void Update()
    {
        if (jugador == null) return; // Seguridad si no hay jugador asignado

        float puntoGeneracion = jugador.position.x + distanciaGeneracion;
        float ultimaCalleX = posicionInicial.x + (_callesGeneradas * distanciaEntreCalles);

        if (puntoGeneracion > ultimaCalleX)
        {
            Vector2 nuevaPosicion = new Vector2(ultimaCalleX, posicionInicial.y);
            GenerarCalle(nuevaPosicion);
            _callesGeneradas++;
        }
    }
}
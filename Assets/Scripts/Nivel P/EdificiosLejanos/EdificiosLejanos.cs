using System.Collections.Generic;
using UnityEngine;

public class GeneradorEdificiosLejanos : MonoBehaviour
{
    public GameObject[] prefabsEdificios; // Arrastra los prefabs aquí
    public int cantidadMaxima = 5; // Cantidad máxima de edificios visibles
    public float distanciaMinimaEntreEdificios = 4f; // Evita amontonamiento
    public float distanciaParaGenerar = 30f; // Distancia para generar nuevos edificios
    public float opacidadMinima = 0.7f; // Opacidad mínima de los edificios
    public float alturaBase = -3.09f; // Altura fija para alinear los edificios con la base del nivel

    private Dictionary<Vector2, GameObject> edificiosGenerados = new Dictionary<Vector2, GameObject>();
    private Vector2 ultimaPosicionJugador;

    void Start()
    {
        // Generar edificios iniciales
        GenerarEdificiosIniciales();
    }

    void Update()
    {
        // Generar nuevos edificios si el jugador avanza más allá de la distancia definida
        if (Vector2.Distance(Vector2.zero, ultimaPosicionJugador) >= distanciaParaGenerar)
        {
            GenerarEdificiosEnRango();
            ultimaPosicionJugador = Vector2.zero;
        }
    }

    void GenerarEdificiosIniciales()
    {
        // Generar edificios en un rango inicial
        GenerarEdificiosEnRango();
    }

    void GenerarEdificiosEnRango()
    {
        // Calcular el rango visible en el eje X basado en la cámara
        float rangoVisibleX = Camera.main.orthographicSize * Camera.main.aspect;

        int edificiosGeneradosEnEstaIteracion = 0;

        for (float x = -rangoVisibleX; x <= rangoVisibleX; x += distanciaMinimaEntreEdificios)
        {
            // Detener la generación si ya alcanzamos el límite de cantidad máxima
            if (edificiosGenerados.Count + edificiosGeneradosEnEstaIteracion >= cantidadMaxima)
                break;

            Vector2 posicion = new Vector2(x, alturaBase); // Altura fija para alinear con la base del nivel

            // Verificar si ya existe un edificio en esta posición
            if (!edificiosGenerados.ContainsKey(posicion))
            {
                // Generar un nuevo edificio
                int indiceAleatorio = Random.Range(0, prefabsEdificios.Length);
                GameObject edificio = Instantiate(
                    prefabsEdificios[indiceAleatorio],
                    posicion,
                    Quaternion.identity,
                    transform
                );

                // Configurar opacidad
                SpriteRenderer renderer = edificio.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    Color color = renderer.color;
                    color.a = Random.Range(opacidadMinima, 1f);
                    renderer.color = color;
                }

                // Registrar el edificio generado
                edificiosGenerados[posicion] = edificio;
                edificiosGeneradosEnEstaIteracion++;
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class GeneradorEdificiosLejanos : MonoBehaviour
{
    public GameObject[] prefabsEdificios; // Prefabs de edificios
    public Transform jugador; // Referencia al jugador

    [Header("Configuración de Generación")]
    public float distanciaMinimaX = 5f; // Distancia mínima entre edificios
    public float distanciaMaximaX = 10f; // Distancia máxima entre edificios
    public float alturaMinima = -2f; // Altura mínima de los edificios
    public float alturaMaxima = 2f; // Altura máxima de los edificios
    public int edificiosIniciales = 2; // Cantidad inicial de edificios
    public float distanciaGeneracion = 15f; // Distancia para generar nuevos edificios

    private Vector2 ultimaPosicionDerecha; // Última posición generada hacia la derecha
    private Vector2 ultimaPosicionIzquierda; // Última posición generada hacia la izquierda
    private HashSet<Vector2> posicionesGeneradas = new HashSet<Vector2>(); // Evitar duplicados

    void Start()
    {
        ultimaPosicionDerecha = jugador.position;
        ultimaPosicionIzquierda = jugador.position;

        GenerarEdificiosIniciales();
    }

    void Update()
    {
        if (jugador == null) return;

        // Generar edificios hacia la derecha
        if (jugador.position.x + distanciaGeneracion > ultimaPosicionDerecha.x)
        {
            GenerarEdificioDerecha();
        }

        // Generar edificios hacia la izquierda
        if (jugador.position.x - distanciaGeneracion < ultimaPosicionIzquierda.x)
        {
            GenerarEdificioIzquierda();
        }
    }

    void GenerarEdificiosIniciales()
    {
        // Generar edificios iniciales hacia adelante
        for (int i = 0; i < edificiosIniciales; i++)
        {
            GenerarEdificioDerecha();
        }

        // Generar edificios iniciales hacia atrás
        for (int i = 0; i < edificiosIniciales; i++)
        {
            GenerarEdificioIzquierda();
        }
    }

    void GenerarEdificioDerecha()
    {
        float nuevaX = ultimaPosicionDerecha.x + Random.Range(distanciaMinimaX, distanciaMaximaX);
        float nuevaY = Random.Range(alturaMinima, alturaMaxima);
        Vector2 nuevaPosicion = new Vector2(nuevaX, nuevaY);

        if (!posicionesGeneradas.Contains(nuevaPosicion))
        {
            GenerarEdificioEnPosicion(nuevaPosicion);
            ultimaPosicionDerecha = nuevaPosicion;
            posicionesGeneradas.Add(nuevaPosicion);
        }
    }

    void GenerarEdificioIzquierda()
    {
        float nuevaX = ultimaPosicionIzquierda.x - Random.Range(distanciaMinimaX, distanciaMaximaX);
        float nuevaY = Random.Range(alturaMinima, alturaMaxima);
        Vector2 nuevaPosicion = new Vector2(nuevaX, nuevaY);

        if (!posicionesGeneradas.Contains(nuevaPosicion))
        {
            GenerarEdificioEnPosicion(nuevaPosicion);
            ultimaPosicionIzquierda = nuevaPosicion;
            posicionesGeneradas.Add(nuevaPosicion);
        }
    }

    void GenerarEdificioEnPosicion(Vector2 posicion)
    {
        // Instanciar un edificio en la posición especificada
        int indiceAleatorio = Random.Range(0, prefabsEdificios.Length);
        Instantiate(prefabsEdificios[indiceAleatorio], posicion, Quaternion.identity, transform);
    }
}


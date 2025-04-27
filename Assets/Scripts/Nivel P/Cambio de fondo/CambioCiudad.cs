using System.Collections.Generic;
using UnityEngine;

public class CambioCiudad : MonoBehaviour
{
    public Sprite[] fondosCiudades; // Arrastra tus sprites aquí
    public SpriteRenderer renderizadorFondo;
    public Transform jugador; // Asigna el personaje
    public float distanciaCambio = 20f; // Distancia para cambiar de fondo
    public float duracionFade = 1f; // Duración del desvanecido (0 = sin fade)
    public float toleranciaRegreso = 1f; // Tolerancia para detectar si el jugador regresa al inicio

    private Dictionary<int, int> fondosGenerados = new Dictionary<int, int>(); // Almacena los fondos generados por posición
    private Vector2 posicionInicialJugador; // Posición inicial del jugador
    private Vector2 ultimaPosicionJugador;
    private int indiceFondoActual;
    private int indiceFondoInicial; // Índice del fondo inicial

    void Start()
    {
        // Validar referencias
        if (fondosCiudades == null || fondosCiudades.Length == 0)
        {
            Debug.LogError("No se han asignado fondos en 'fondosCiudades'.");
            enabled = false;
            return;
        }

        if (renderizadorFondo == null)
        {
            Debug.LogError("No se ha asignado un SpriteRenderer en 'renderizadorFondo'.");
            enabled = false;
            return;
        }

        if (jugador == null)
        {
            Debug.LogError("No se ha asignado un Transform en 'jugador'.");
            enabled = false;
            return;
        }

        // Inicializar el fondo
        indiceFondoInicial = Random.Range(0, fondosCiudades.Length); // Fondo inicial
        indiceFondoActual = indiceFondoInicial;
        renderizadorFondo.sprite = fondosCiudades[indiceFondoInicial];
        AjustarFondo();

        // Guardar la posición inicial del jugador
        posicionInicialJugador = jugador.position;
        ultimaPosicionJugador = jugador.position;

        // Registrar el fondo inicial
        fondosGenerados[CalcularClavePosicion(jugador.position)] = indiceFondoInicial;
    }

    void Update()
    {
        int clavePosicion = CalcularClavePosicion(jugador.position);

        // Detectar si el jugador regresa a una posición anterior
        if (fondosGenerados.ContainsKey(clavePosicion))
        {
            CambiarFondoExistente(fondosGenerados[clavePosicion]);
        }
        else if (Vector2.SqrMagnitude((Vector2)jugador.position - ultimaPosicionJugador) >= distanciaCambio * distanciaCambio)
        {
            CambiarFondoNuevo(clavePosicion);
            ultimaPosicionJugador = jugador.position;
        }
    }

    void CambiarFondoExistente(int indiceFondo)
    {
        if (indiceFondoActual != indiceFondo)
        {
            indiceFondoActual = indiceFondo;
            renderizadorFondo.sprite = fondosCiudades[indiceFondo];
            AjustarFondo();
        }
    }

    void CambiarFondoNuevo(int clavePosicion)
    {
        int nuevoIndice;
        do
        {
            nuevoIndice = Random.Range(0, fondosCiudades.Length);
        } while (nuevoIndice == indiceFondoActual); // Asegurarse de que el fondo sea diferente

        indiceFondoActual = nuevoIndice;
        renderizadorFondo.sprite = fondosCiudades[indiceFondoActual];
        AjustarFondo();

        // Registrar el nuevo fondo
        fondosGenerados[clavePosicion] = nuevoIndice;
    }

    int CalcularClavePosicion(Vector2 posicion)
    {
        return Mathf.FloorToInt(posicion.x / distanciaCambio);
    }

    void AjustarFondo()
    {
        // Escalar el sprite para cubrir la pantalla
        Sprite sprite = renderizadorFondo.sprite;
        float alturaPantalla = Camera.main.orthographicSize * 2;
        float anchoPantalla = alturaPantalla * Camera.main.aspect;

        renderizadorFondo.transform.localScale = new Vector3(
            anchoPantalla / sprite.bounds.size.x,
            alturaPantalla / sprite.bounds.size.y,
            1
        );

        // Asegurar que el fondo esté centrado en la pantalla
        renderizadorFondo.transform.position = Camera.main.transform.position;
        renderizadorFondo.transform.position = new Vector3(
            renderizadorFondo.transform.position.x,
            renderizadorFondo.transform.position.y,
            0 // Aseguramos que esté en el plano Z correcto
        );
    }}
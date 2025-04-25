using UnityEngine;

public class CambioCiudad : MonoBehaviour
{
    public Sprite[] fondosCiudades; // Arrastra tus sprites aquí
    public SpriteRenderer renderizadorFondo;
    public Transform jugador; // Asigna el personaje
    public float distanciaCambio = 20f; // Distancia para cambiar de fondo
    public float duracionFade = 1f; // Duración del desvanecido (0 = sin fade)
    public float toleranciaRegreso = 1f; // Tolerancia para detectar si el jugador regresa al inicio

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
    }

    void Update()
    {
        // Detectar si el jugador regresa a la posición inicial
        if (Vector2.Distance(jugador.position, posicionInicialJugador) <= toleranciaRegreso)
        {
            CambiarFondoInicial();
            return;
        }

        // Cambia el fondo si el jugador avanza la distancia especificada
        if (Vector2.SqrMagnitude((Vector2)jugador.position - ultimaPosicionJugador) >= distanciaCambio * distanciaCambio)
        {
            CambiarFondo();
            ultimaPosicionJugador = jugador.position;
        }
    }

    void CambiarFondo()
    {
        if (duracionFade > 0)
            StartCoroutine(FadeYCambiar());
        else
            CambiarFondoInstantaneo();
    }

    void CambiarFondoInicial()
    {
        if (indiceFondoActual != indiceFondoInicial) // Solo cambiar si no es el fondo inicial
        {
            indiceFondoActual = indiceFondoInicial;
            renderizadorFondo.sprite = fondosCiudades[indiceFondoInicial];
            AjustarFondo();
        }
    }

    void CambiarFondoInstantaneo()
    {
        int nuevoIndice;
        do
        {
            nuevoIndice = Random.Range(0, fondosCiudades.Length);
        } while (nuevoIndice == indiceFondoActual); // Asegurarse de que el fondo sea diferente

        indiceFondoActual = nuevoIndice;
        renderizadorFondo.sprite = fondosCiudades[indiceFondoActual];
        AjustarFondo();
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
    }

    System.Collections.IEnumerator FadeYCambiar()
    {
        // Validar que el material soporte transparencia
        if (!renderizadorFondo.material.HasProperty("_Color"))
        {
            Debug.LogError("El material del SpriteRenderer no soporta transparencia.");
            yield break;
        }

        // Fade Out
        for (float t = 0; t < duracionFade; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1, 0, t / duracionFade);
            renderizadorFondo.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        CambiarFondoInstantaneo();

        // Fade In
        for (float t = 0; t < duracionFade; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0, 1, t / duracionFade);
            renderizadorFondo.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }
}

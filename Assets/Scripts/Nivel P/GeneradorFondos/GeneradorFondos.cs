using UnityEngine;

public class GeneradorFondos : MonoBehaviour
{
    public Sprite[] fondosDisponibles; // Arrastra aquí tus sprites de fondo
    public SpriteRenderer renderizadorFondo;
    public Transform jugador; // Asigna el Transform del jugador
    public float tiempoCambio = 30f; // Cambia cada 30 segundos

    private float temporizador;
    private int indiceActual = 0; // Variable para rastrear el índice actual del fondo

    void Start()
    {
        if (fondosDisponibles.Length > 0 && renderizadorFondo != null)
        {
            CambiarFondo(); // Cambia el fondo inicial
        }
    }

    void Update()
    {
        // Hacer que el fondo siga al jugador
        SeguirJugador();

        // Cambiar el fondo automáticamente cada cierto tiempo
        temporizador += Time.deltaTime;
        if (temporizador >= tiempoCambio)
        {
            CambiarFondo();
            temporizador = 0f; // Reinicia correctamente el temporizador
        }
    }

    void SeguirJugador()
    {
        if (Camera.main != null)
        {
            // Actualizar la posición del fondo para que siga a la cámara
            renderizadorFondo.transform.position = new Vector3(
                Camera.main.transform.position.x,
                Camera.main.transform.position.y,
                renderizadorFondo.transform.position.z // Mantener el plano Z
            );
        }
    }


    void CambiarFondo()
    {
        renderizadorFondo.sprite = fondosDisponibles[indiceActual];

        // Escalar el sprite para cubrir la pantalla
        Sprite sprite = renderizadorFondo.sprite;
        float alturaPantalla = Camera.main.orthographicSize * 2;
        float anchoPantalla = alturaPantalla * Camera.main.aspect;

        renderizadorFondo.transform.localScale = new Vector3(
            anchoPantalla / sprite.bounds.size.x,
            alturaPantalla / sprite.bounds.size.y,
            1
        );

        indiceActual = (indiceActual + 1) % fondosDisponibles.Length;
    }
}

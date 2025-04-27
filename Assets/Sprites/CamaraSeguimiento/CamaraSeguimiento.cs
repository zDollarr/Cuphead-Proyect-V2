using UnityEngine;

public class CamaraSeguimiento : MonoBehaviour
{
    public Transform jugador;
    public float suavizado = 5f;
    public Vector2 offset = new Vector2(2f, 1f); // Ajusta según necesidad
    public float posicionFijaY = 0f; // Altura fija de la cámara

    void LateUpdate()
    {
        // Calcula la posición objetivo (sigue al jugador en X, pero mantiene Y fija)
        Vector3 posicionObjetivo = new Vector3(
            jugador.position.x + offset.x,
            posicionFijaY, // Mantén la posición fija en Y
            transform.position.z
        );

        // Movimiento suavizado
        transform.position = Vector3.Lerp(
            transform.position,
            posicionObjetivo,
            suavizado * Time.deltaTime
        );
    }
}

using UnityEngine;

public class CalleBase : MonoBehaviour
{
    // Este script es solo un marcador para identificar la calle base
    void OnDrawGizmos()
    {
        // Dibuja un gizmo en el editor para identificar la calle base
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(5f, 1f, 0f)); // Ajusta el tamaño según tu calle base
    }
}

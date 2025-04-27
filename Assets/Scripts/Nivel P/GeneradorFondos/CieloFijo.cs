using UnityEngine;

public class CieloSeguidorCamara : MonoBehaviour
{
    private Transform camara;

    void Start()
    {
        camara = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Sincroniza la posición X/Z con la cámara, pero mantén la Y fija
        transform.position = new Vector3(
            camara.position.x,
            transform.position.y, // Mantén la Y original
            transform.position.z
        );
    }
}

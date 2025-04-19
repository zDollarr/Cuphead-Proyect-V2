using UnityEngine;

public class GeneradorFondos : MonoBehaviour
{
    public Sprite[] fondosDisponibles; // Arrastra aquí tus sprites de fondo
    public SpriteRenderer renderizadorFondo;
    public float tiempoCambio = 30f; // Cambia cada 10 segundos

    private float temporizador;

    void Start()
    {
        if (fondosDisponibles.Length > 0 && renderizadorFondo != null)
        {
            CambiarFondoAleatorio();
        }
    }

    void Update()
    {
        temporizador += Time.deltaTime;
        if (temporizador >= tiempoCambio)
        {
            CambiarFondoAleatorio();
            temporizador = 0f; // Reinicia correctamente el temporizador
        }
    }

    void CambiarFondoAleatorio()
    {
        int indiceAleatorio = Random.Range(0, fondosDisponibles.Length);
        renderizadorFondo.sprite = fondosDisponibles[indiceAleatorio];
    }
}

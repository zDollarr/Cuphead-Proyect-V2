using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator anim;
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravityScale = 3f; // A mayor valor, más rápida será la caída
    public float normalGravityScale = 1f; // Gravedad normal al estar en el suelo

    private bool isGrounded = true;
    private bool isCrouching = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        rb2d.gravityScale = normalGravityScale; // Configuración inicial de gravedad normal
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleCrouch();
        HandleShoot(); // Se maneja la animación de disparo
    }

    private void HandleMovement()
    {
        if (isCrouching)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y); // No se mueve mientras se agacha
            anim.SetBool("run", false);
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");

        rb2d.velocity = new Vector2(h * moveSpeed, rb2d.velocity.y);
        anim.SetBool("run", h != 0 && isGrounded); // Solo correr si está en el suelo

        if (h > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (h < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            anim.SetBool("jump", true);
            isGrounded = false;
            rb2d.gravityScale = gravityScale; // Aplicar gravedad aumentada tan pronto como salte
        }
    }

    private void HandleCrouch()
    {
        isCrouching = Input.GetKey(KeyCode.S);
        anim.SetBool("down", isCrouching);
    }

    private void HandleShoot()
    {
        bool isShooting = Input.GetMouseButton(0);
        anim.SetBool("shoot", isShooting);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("jump", false);
            rb2d.gravityScale = normalGravityScale; // Volver a la gravedad normal cuando toque el suelo
        }
    }
}
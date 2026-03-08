using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class jugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool mirandoDerecha = true;

    [Header("Salto")]
    public int saltosMax = 2;
    private int saltosActuales;

    [Header("Vida")]
    public int vidaMax = 5;
    public int vidaActual;

    [Header("Vidas")]
    public int vidas = 3;
    public TMP_Text textoVidas;

    [Header("Invulnerabilidad")]
    public float tiempoInvulnerable = 1.5f;
    public bool invulnerable = false;
    public bool dañado = false;
    private float timerInvulnerabilidad = 0f;

    [Header("UI")]
    public Slider barraVida;
    public GameObject canvasGameOver;

    private bool muerto = false;
    private Vector3 puntoRespawn;

    [Header("Disparo")]
    public GameObject balaPrefab;
    public Transform puntoDisparo;
    public float velocidadBala = 7f;
    public float tiempoEntreDisparos = 0.5f;
    private bool puedeDisparar = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        vidaActual = vidaMax;
        puntoRespawn = transform.position;
        saltosActuales = saltosMax;

        if (barraVida != null)
        {
            barraVida.maxValue = vidaMax;
            barraVida.value = vidaActual;
        }

        if (textoVidas != null)
            textoVidas.text = "Vidas: " + vidas;

        if (canvasGameOver != null)
            canvasGameOver.SetActive(false);
    }

    void Update()
    {
        if (muerto)
        {
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        // Voltear sprite
        if (move > 0 && !mirandoDerecha)
            Voltear();
        else if (move < 0 && mirandoDerecha)
            Voltear();

        // SALTO NORMAL + DOBLE SALTO
        if (Input.GetKeyDown(KeyCode.Space) && saltosActuales > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            saltosActuales--;
        }

        // Disparo
        if (Input.GetKey(KeyCode.Mouse0) && puedeDisparar)
        {
            Disparar();
        }

        // Invulnerabilidad
        if (invulnerable)
        {
            timerInvulnerabilidad -= Time.deltaTime;
            if (timerInvulnerabilidad <= 0)
            {
                invulnerable = false;
                timerInvulnerabilidad = 0;
            }
        }

        if (dañado && !invulnerable)
        {
            AplicarDaño(1);
            invulnerable = true;
            timerInvulnerabilidad = tiempoInvulnerable;
        }
    }

    void Voltear()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    void Disparar()
    {
        puedeDisparar = false;

        GameObject balaObj = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
        balaJugador b = balaObj.GetComponent<balaJugador>();

        if (b != null)
        {
            Vector2 dir = mirandoDerecha ? Vector2.right : Vector2.left;
            b.SetDireccion(dir);
            b.velocidad = velocidadBala;
        }

        Invoke(nameof(ResetDisparo), tiempoEntreDisparos);
    }

    void ResetDisparo()
    {
        puedeDisparar = true;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            saltosActuales = saltosMax; // resetear saltos
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spawn"))
        {
            float baseY = other.bounds.min.y;
            puntoRespawn = new Vector3(other.transform.position.x, baseY, transform.position.z);
        }
    }

    public void RecibirDaño(int daño)
    {
        if (invulnerable || muerto) return;

        AplicarDaño(daño);
        invulnerable = true;
        timerInvulnerabilidad = tiempoInvulnerable;
    }

    void AplicarDaño(int daño)
    {
        vidaActual -= daño;

        if (barraVida != null)
            barraVida.value = vidaActual;

        if (vidaActual <= 0)
            PerderVida();
    }

    void PerderVida()
    {
        vidas--;

        if (textoVidas != null)
            textoVidas.text = "Vidas: " + vidas;

        if (vidas <= 0)
            Morir();
        else
            Respawn();
    }

    void Respawn()
    {
        vidaActual = vidaMax;

        if (barraVida != null)
            barraVida.value = vidaActual;

        transform.position = puntoRespawn;
        rb.velocity = Vector2.zero;
    }

    void Morir()
    {
        muerto = true;

        if (canvasGameOver != null)
            canvasGameOver.SetActive(true);

        rb.velocity = Vector2.zero;
    }
}
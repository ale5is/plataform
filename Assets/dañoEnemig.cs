using UnityEngine;

public class dañoEnemigo : MonoBehaviour
{
    public GameObject objetoADestruir;
    public float fuerzaRebote = 8f;
    public int puntosAlDestruir = 10; // puntos que dará al jugador al destruir

    [Header("Referencia a UI")]
    public interfaz ui; // arrastrar el objeto que tenga el script 'interfaz' desde el Inspector

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("balaJ"))
        {
            // Dar puntos al jugador vía interfaz
            if (ui != null)
            {
                ui.SumarPuntos(puntosAlDestruir);
            }

            // Destruir objeto extra
            if (objetoADestruir != null)
            {
                Destroy(objetoADestruir);
            }

            // Destruir enemigo
            Destroy(gameObject);
        }
            if (other.CompareTag("Pdaño"))
            {
            // Rebote del jugador
            Rigidbody2D rb = other.GetComponentInParent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(rb.velocity.x, fuerzaRebote);
            }

            // Dar puntos al jugador vía interfaz
            if (ui != null)
            {
                ui.SumarPuntos(puntosAlDestruir);
            }

            // Destruir objeto extra
            if (objetoADestruir != null)
            {
                Destroy(objetoADestruir);
            }

            // Destruir enemigo
            Destroy(gameObject);
        }
    }
}
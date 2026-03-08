using UnityEngine;

public class bala : MonoBehaviour
{
    public float velocidad = 5f;
    public int daño;

    private Vector2 direccion;

    // La dirección solo será horizontal: -1 = izquierda, 1 = derecha
    public void SetDireccion(GameObject jugador)
    {
        if (jugador.transform.position.x >= transform.position.x)
            direccion = Vector2.right;  // disparar a la derecha
        else
            direccion = Vector2.left;   // disparar a la izquierda

        // Ajustar rotación visual si quieres que la bala mire a la dirección
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angulo);
    }

    void Update()
    {
        // Movimiento solo horizontal
        transform.position += (Vector3)direccion * velocidad * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugador j = other.GetComponent<jugador>();
            if (j != null)
            {
                j.RecibirDaño(daño);
            }
            Destroy(gameObject);
        }
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
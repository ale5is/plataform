using UnityEngine;

public class balaJugador : MonoBehaviour
{
    public float velocidad = 5f;
    public int daño = 1;

    private Vector2 direccion;

    public void SetDireccion(Vector2 dir)
    {
        direccion = dir.normalized;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angulo);
    }

    void Update()
    {
        transform.position += (Vector3)direccion * velocidad * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Daño a enemigos
        if (other.CompareTag("pDaene"))
        {   
            Destroy(gameObject);
        }
        
    }
}
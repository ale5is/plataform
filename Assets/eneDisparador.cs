using UnityEngine;
using System.Collections;

public class eneDisparador : MonoBehaviour
{
    [Header("Stats")]
    public float rangoDeteccion = 10f;
    public float tiempoAntesDeDisparar = 1.5f;
    public GameObject balaPrefab;
    public Transform puntoDisparo;
    public int dañoBala = 1;

    [Header("Disparo")]
    public float tiempoEntreDisparos = 1f;   // cooldown entre ráfagas
    public int cantidadBalas = 1;            // cuántas balas dispara a la vez
    public float velocidadBala = 5f;         // velocidad de las balas
    public float tiempoEntreBalas = 0.2f;    // tiempo entre cada bala de la ráfaga

    private bool puedeDisparar = true;

    void Update()
    {
        DetectarJugador();
    }

    void DetectarJugador()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador == null) return;

        float toleranciaY = 0.5f;
        if (Mathf.Abs(jugador.transform.position.y - transform.position.y) <= toleranciaY)
        {
            float distancia = Vector2.Distance(jugador.transform.position, transform.position);
            if (distancia <= rangoDeteccion && puedeDisparar)
            {
                puedeDisparar = false;
                Invoke(nameof(Disparar), tiempoAntesDeDisparar);
            }
        }
    }

    void Disparar()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador == null) return;

        // Iniciar ráfaga de balas con delay
        StartCoroutine(DispararRafaga(jugador));

        // Reactivar disparo después de cooldown
        Invoke(nameof(ReactivarDisparo), tiempoEntreDisparos);
    }

    IEnumerator DispararRafaga(GameObject jugador)
    {
        for (int i = 0; i < cantidadBalas; i++)
        {
            GameObject balaObj = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
            bala b = balaObj.GetComponent<bala>();
            if (b != null)
            {
                b.SetDireccion(jugador);
                b.daño = dañoBala;
                b.velocidad = velocidadBala;
            }

            yield return new WaitForSeconds(tiempoEntreBalas); // espera antes de crear la siguiente bala
        }
    }

    void ReactivarDisparo()
    {
        puedeDisparar = true;
    }
}
using UnityEngine;
using TMPro;

public class objetivo : MonoBehaviour
{
    public TMP_Text texto;     // Texto a desactivar
    public float tiempo = 3f;  // Segundos antes de desaparecer

    void Start()
    {
        Invoke("Desactivar", tiempo);
    }

    void Desactivar()
    {
        if (texto != null)
        {
            texto.gameObject.SetActive(false);
        }
    }
}
using UnityEngine;

public class Escapar : MonoBehaviour
{
    // Define el tag del objeto que activa la destrucción
    private const string TAG_PUERTA = "Player";

    // Usar OnCollisionEnter si quieres una colisión física
    private void OnCollisionEnter(Collision collision)
    {
        // Comprobar si el objeto con el que colisionamos tiene el tag "puerta"
        if (collision.gameObject.CompareTag(TAG_PUERTA))
        {
            Debug.Log(gameObject.name + " ha tocado la puerta! Escapando...");
            
            // Destruir este objeto (el personaje)
            Destroy(gameObject);
        }
    }
    
    
    // OPCIONAL: Si la "puerta" tiene marcado 'Is Trigger' en su Collider, usa esta función:
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TAG_PUERTA))
        {
            Debug.Log(gameObject.name + " ha atravesado la puerta! Escapando...");
            Destroy(gameObject);
            Time.timeScale = 0f;
            if (FindObjectOfType<ControladorUI>() != null)
            {
                FindObjectOfType<ControladorUI>().MostrarCanvasGanar();
            }
        }
    }
    
}
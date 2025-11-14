using UnityEngine;

public class ItemCuracion : MonoBehaviour
{
    // Esta es la variable pública que verás en el Inspector
    [Header("Configuración de Curación")]
    [Tooltip("Cantidad de puntos de vida que se restaurarán al jugador.")]
    public int cantidadDeCuracion = 25; 

    private const string PlayerTag = "Player";


    // Se llama cuando el Collider (marcado como Trigger) de este objeto es atravesado por otro Collider.
    private void OnTriggerEnter(Collider other)
    {
        // 1. Verifica si el objeto que atravesó tiene el Tag "Player"
        if (other.gameObject.CompareTag(PlayerTag))
        {
            // 2. Intenta obtener el script AtributosPersonaje del objeto Player
            AtributosPersonaje atributosJugador = other.gameObject.GetComponent<AtributosPersonaje>();

            if (atributosJugador != null)
            {
                // 3. Aplica la curación
                AplicarCuracion(atributosJugador);
            }
            else
            {
                Debug.LogWarning("El Player colisionó con la curación pero NO tiene el componente AtributosPersonaje.");
            }
        }
    }

    private void AplicarCuracion(AtributosPersonaje target)
    {
        // Llama al método público en el script AtributosPersonaje
        target.RecibirCuracion(cantidadDeCuracion);
        
        Debug.Log($"El jugador se curó en {cantidadDeCuracion} puntos.");

        // 4. Desaparecer el objeto de curación
        Destroy(gameObject);
    }
}
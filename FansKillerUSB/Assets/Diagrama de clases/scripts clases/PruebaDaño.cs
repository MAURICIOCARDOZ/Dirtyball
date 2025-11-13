using UnityEngine;

public class PruebaDaño : MonoBehaviour
{
    public int damage = 10; // Establece un valor predeterminado de daño.
    public GameObject canvasAMostrar;

    // No necesitamos la referencia pública 'Player' si la obtenemos al colisionar.

    private void OnCollisionEnter(Collision other)
    {
        // 1. Intentar obtener el componente AtributosPersonaje del objeto colisionado
        AtributosPersonaje targetAttributes = other.gameObject.GetComponent<AtributosPersonaje>();

        // 2. Comprobar si el objeto colisionado tiene el componente de vida
        if (targetAttributes != null)
        {
            // Hemos chocado con algo que tiene atributos de personaje (Player o Enemigo)

            // 3. Llamar al método RecibirDano del script encontrado
            targetAttributes.RecibirDano(damage);
            if (targetAttributes.VidaActual <= 0)
            {
                canvasAMostrar.SetActive(true);
            }
            Debug.Log("Impacto detectado en un objetivo con vida: " + other.gameObject.name);

            // Opcional: Destruir el objeto que causa el daño (ej. una bala)
            // Destroy(gameObject); 
        }
        else
        {
            // Opcional: Mensaje si el objeto no tenía el script de vida
            // Debug.Log(other.gameObject.name + " no tiene el componente AtributosPersonaje.");
        }
    }
}



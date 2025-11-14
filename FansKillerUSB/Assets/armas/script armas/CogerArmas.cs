using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CogerArmas : MonoBehaviour
{
    public GameObject[] armas;
    // Referencia al script de atributos de tu personaje
    public AtributosPersonaje atributosPersonaje;
    private ArmasClase atributosArma;


    // CogerArmas.cs
public void ActivarArma(int numero)
{
    // ... (Tu código de activación/desactivación de GameObjects)
    
    armas[numero].SetActive(true);

    Collider armaCollider = armas[numero].GetComponent<Collider>();
    atributosArma = armas[numero].GetComponent<ArmasClase>();

    // 1. **COMPROBACIÓN CRÍTICA**: Verificar la referencia del personaje
    if (atributosPersonaje == null)
    {
        // Esto evita el NullReferenceException si no asignaste nada en el Inspector.
        Debug.LogError("ERROR en CogerArmas: La variable 'atributosPersonaje' no está asignada en el Inspector de Unity.");
        return; // Detiene la ejecución para evitar el error.
    }

    // 2. Comprobar la referencia del arma (como ya lo estabas haciendo)
    if (atributosArma != null)
    {
        // Settea los atributos solo si AMBAS referencias son válidas.
        atributosPersonaje.SetearAtributosDeArma(
            atributosArma.daño,
            atributosArma.cooldown,
            atributosArma.rango
        );
        atributosPersonaje.Arma = numero; // Opcional: setear el índice del arma
    }
    else
    {
        Debug.LogError($"ERROR: El GameObject del arma con índice {numero} no tiene el componente 'ArmasClase' adjunto.");
    }
    
    // ... (El resto de tu código)
    if (armaCollider != null)
    {
        armaCollider.enabled = false;
    }
}

public void DesactivarArma(int numero)
{
    // Verificamos que el índice sea válido y que el GameObject exista en el array 'armas'.
    if (numero >= 0 && numero < armas.Length && armas[numero] != null)
    {
        // --- 1. Desactivar el GameObject (Ocultar el arma de la mano) ---
        // Esto oculta el modelo visual y detiene cualquier script adjunto al arma.
        armas[numero].SetActive(false);

        // --- 2. Desactivar el Collider (aunque SetActive(false) ya lo hace, es una buena práctica) ---
        Collider armaCollider = armas[numero].GetComponent<Collider>();
        if (armaCollider != null)
        {
            armaCollider.enabled = false;
        }

        // --- 3. Restablecer Atributos del Personaje (CRÍTICO para quitar los bonus del arma) ---
        if (atributosPersonaje != null)
        {
            // Llama a una función que restablece los atributos del personaje a sus valores base (sin arma).
            // ¡DEBES IMPLEMENTAR atributosPersonaje.ResetearAtributosDeArma() en AtributosPersonaje.cs!
            
            // Seteamos el ID del arma del jugador a 0 (representando 'sin arma').
            atributosPersonaje.Arma = 0; 
            
            Debug.Log($"Arma ID {numero} desactivada. El personaje está ahora desarmado.");
        }
        else
        {
            Debug.LogError("ERROR: La variable 'atributosPersonaje' es nula. No se pudieron restablecer los atributos.");
        }
    }
    else
    {
        Debug.LogError($"ERROR: El índice {numero} no es válido o el GameObject no existe en la lista 'armas'.");
    }
}
}

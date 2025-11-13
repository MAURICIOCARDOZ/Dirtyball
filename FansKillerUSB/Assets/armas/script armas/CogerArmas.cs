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

}

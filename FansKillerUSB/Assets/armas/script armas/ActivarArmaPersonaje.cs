using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq; // Necesario para usar .FirstOrDefault()

public class ActivarArmaPersonaje : MonoBehaviour
{
    public CogerArmas cogerArmas;
    [Tooltip("ID del arma que representa este objeto en el suelo.")]
    public int numeroArma;
    
    // Referencia al componente de texto TextMeshPro
    [Header("UI de Interacción")]
    public TextMeshPro textoInteraccion; 
    
    // Esta lista ya no se usa para spawn.
    [Header("Configuración de Spawn (No usada)")]
    public GameObject[] weaponPrefabsList;
    
    private bool jugadorEnRango = false;
    private AtributosPersonaje atributosJugador; 
    private Collider myCollider; 
    private Renderer[] allRenderers; 
    
    // Lista estática para que todos los scripts de arma se registren (Gerente Implícito).
    private static List<ActivarArmaPersonaje> allWeaponScripts = new List<ActivarArmaPersonaje>();

    void Awake()
    {
        // Registrar este script en la lista estática.
        if (!allWeaponScripts.Contains(this))
        {
            allWeaponScripts.Add(this);
        }
    }

    void OnDestroy()
    {
        // Limpiar la lista si el objeto es destruido (por si acaso).
        allWeaponScripts.Remove(this);
    }
    
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            cogerArmas = player.GetComponent<CogerArmas>();
            atributosJugador = player.GetComponent<AtributosPersonaje>(); 
        }
        
        if (cogerArmas == null || atributosJugador == null)
        {
            Debug.LogError("Faltan componentes CogerArmas o AtributosPersonaje en el Player.");
        }

        myCollider = GetComponent<Collider>();
        // Obtener renderers para poder ocultar/mostrar la malla (Importante: incluye hijos inactivos)
        allRenderers = GetComponentsInChildren<Renderer>(true);
        
        if (textoInteraccion != null)
        {
            textoInteraccion.enabled = false;
        }

        // Asegurarse de que el objeto esté activo si fue colocado en la escena visiblemente
        if (gameObject.activeSelf)
        {
            SetObjectVisibility(true); 
        }
    }
    
    /// <summary>
    /// Oculta o muestra el GameObject completo (modelos, colliders y detiene el Update).
    /// </summary>
    private void SetObjectVisibility(bool isVisible)
    {
        // Oculta/Muestra los renderers y el collider
        foreach (Renderer r in allRenderers)
        {
            r.enabled = isVisible;
        }
        
        if (myCollider != null)
        {
            myCollider.enabled = isVisible;
        }
        
        if (textoInteraccion != null)
        {
             textoInteraccion.enabled = isVisible && jugadorEnRango;
        }
        
        // Desactiva el GameObject completo, lo cual detiene todo, incluyendo el script Update().
        // Esto es esencial para que solo el Gerente pueda reactivarlo.
        gameObject.SetActive(isVisible);
        
        jugadorEnRango = false; 
    }

    /// <summary>
    /// **Gerente Implícito:** Se llama cuando un jugador suelta un arma. Hace aparecer el objeto original.
    /// </summary>
    public static void SpawnOldWeapon(int weaponId, Vector3 position, Quaternion rotation)
    {
        // 1. Buscamos el objeto ORIGINAL con ese ID que actualmente esté INACTIVO (oculto)
        ActivarArmaPersonaje oldWeapon = allWeaponScripts
            .FirstOrDefault(w => w.numeroArma == weaponId && !w.gameObject.activeSelf);

        if (oldWeapon != null)
        {
            // 2. Si lo encuentra, lo teletransporta a la posición de drop
            oldWeapon.transform.position = position;
            oldWeapon.transform.rotation = rotation;
            
            // 3. Lo hace visible (SetActve(true) en el GameObject)
            oldWeapon.SetObjectVisibility(true);
            Debug.Log($"[Manager] Objeto ORIGINAL ID {weaponId} reactivado.");
        }
        else
        {
            // Fallback: Si no hay objetos originales disponibles para reciclar, muestra un error.
            Debug.LogWarning($"[Manager] No se encontró el objeto ORIGINAL ID {weaponId} para reciclar. Necesitas colocar más copias de esta arma en la escena.");
        }
    }

    void Update()
    {
        // Solo se ejecuta si el GameObject está activo
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.Q))
        {
            int oldWeaponId = atributosJugador.Arma; 
            int newWeaponId = this.numeroArma;      
            
            // FUSIÓN
            if (oldWeaponId == newWeaponId)
            {
                SetObjectVisibility(false); // Ocultar el objeto recogido
            }
            else // SWAP / REEMPLAZO
            {
                Debug.Log($"Intercambio. Dropeando: {oldWeaponId}. Recogiendo: {newWeaponId}.");
                
                // 1. Lógica de Drop (Hacer aparecer el objeto ORIGINAL antiguo)
                if (oldWeaponId > 0) 
                {
                    // Llama al Gerente para que encuentre el objeto ORIGINAL (sword1) y lo active en la posición actual
                    SpawnOldWeapon(oldWeaponId, transform.position, transform.rotation);
                }
                
                // 2. Equipar el arma nueva (la que estaba en el suelo)
                cogerArmas.DesactivarArma(oldWeaponId); 
                cogerArmas.ActivarArma(newWeaponId); 
                
                // 3. Ocultar el objeto que acabamos de recoger (sword2)
                SetObjectVisibility(false); 
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Comprobación implícita: si el GameObject está activo, el script se está ejecutando
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            if (textoInteraccion != null)
            {
                textoInteraccion.enabled = true;
                textoInteraccion.text = "[Q]";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
            if (textoInteraccion != null)
            {
                textoInteraccion.enabled = false;
            }
        }
    }
}
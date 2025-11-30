using UnityEngine;
using System.Collections;

// NOTA: Asegúrate de que este archivo C# se llame StairsTeleporter.cs
public class DoorsTeleporter : MonoBehaviour
{
    [Header("1. Destino y Referencias")]
    public Vector3 targetPosition = new Vector3(0, 5, 0); 
    public ScreenFader screenFader; 
    public string playerTag = "Player";

    [Header("2. Control de Movimiento")]
    public Rigidbody playerRigidbody; 
    
    [Header("3. Interfaz de Usuario")]
    [Tooltip("Si este campo está vacío (None), el teletransporte es AUTOMÁTICO (Escalera).")]
    public GameObject interactionPrompt; // <-- Interruptor de modo
    
    // Variables de estado y modo
    private bool isTeleporting = false;
    private bool requiresInteractionKey = false; // TRUE si necesita 'Q', FALSE si es automático.
    private Transform playerToTeleport;         // Solo usado si requiresInteractionKey es TRUE

    // UNITY LLAMA A ESTA FUNCIÓN AL INICIO
    private void Start() 
    {
        // El modo de interacción se basa en si hay un prompt asignado
        requiresInteractionKey = (interactionPrompt != null);

        if (screenFader == null)
        {
            screenFader = FindFirstObjectByType<ScreenFader>();
            if (screenFader == null)
            {
                Debug.LogError("No se encontró ScreenFader.");
            }
        }
        
        if (screenFader != null) 
        {
            CanvasGroup cg = screenFader.GetComponent<CanvasGroup>();
            if (cg != null) cg.alpha = 0f;
        }
        
        // OCULTA EL PROMPT AL INICIO DEL JUEGO
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
        
        Debug.Log("Teleporter iniciado en MODO: " + (requiresInteractionKey ? "PUERTA (Presionar 'Q')" : "ESCALERA (Automático)"));
    }

    private void Update()
    {
        // 🟢 LÓGICA SOLO PARA MODO PUERTA ('PRESIONAR Q')
        if (requiresInteractionKey && playerToTeleport != null && !isTeleporting && Input.GetKeyDown(KeyCode.Q))
        {
            // Oculta el prompt y comienza la secuencia
            if (interactionPrompt != null) interactionPrompt.SetActive(false);
            
            StartCoroutine(StairsTeleportWithFade(playerToTeleport));
        }
    }

    // UNITY LLAMA A ESTA FUNCIÓN AL ENTRAR AL TRIGGER
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !isTeleporting)
        {
            if (playerRigidbody == null)
            {
                playerRigidbody = other.GetComponent<Rigidbody>();
            }
            
            if (requiresInteractionKey) // MODO PUERTA (Presiona Q)
            {
                // Guarda la referencia y muestra el prompt
                playerToTeleport = other.transform;
                if (interactionPrompt != null) interactionPrompt.SetActive(true);
            }
            else // MODO ESCALERA (Automático)
            {
                // Teletransporte inmediato
                StartCoroutine(StairsTeleportWithFade(other.transform));
            }
        }
    }
    
    // UNITY LLAMA A ESTA FUNCIÓN AL SALIR DEL TRIGGER
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // Limpia la referencia y oculta el prompt solo si estaba en modo Puerta
            if (requiresInteractionKey)
            {
                playerToTeleport = null;
                if (interactionPrompt != null) interactionPrompt.SetActive(false);
            }
        }
    }

    // Corutina con nombre único para evitar conflictos
    private IEnumerator StairsTeleportWithFade(Transform playerTransform)
    {
        isTeleporting = true;
        
        // Limpiamos referencias y prompts al iniciar la secuencia
        if (requiresInteractionKey)
        {
            playerToTeleport = null; 
        }
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
        
        // 0. DETENER MOVIMIENTO
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero; 
            playerRigidbody.angularVelocity = Vector3.zero; 
            playerRigidbody.isKinematic = true; 
        }

        // 1. FUNDIDO A NEGRO (Fade Out) - Gradual
        if (screenFader != null)
        {
            yield return StartCoroutine(screenFader.Fade(1f)); 
        }

        // 2. TELETRANSPORTE Y GIRO
        playerTransform.position = targetPosition;
        Quaternion newRotation = playerTransform.rotation * Quaternion.Euler(0, 180f, 0);
        playerTransform.rotation = newRotation;
        
        // PAUSA INTERMEDIA (Hold Duration)
        if (screenFader != null && screenFader.holdDuration > 0)
        {
            yield return new WaitForSeconds(screenFader.holdDuration); 
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
        }

        // 3. FUNDIDO A LA ESCENA (Fade In) - Gradual
        if (screenFader != null)
        {
            yield return StartCoroutine(screenFader.Fade(0f)); 
        }

        // 4. RESTAURAR MOVIMIENTO
        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = false; 
        }
        
        isTeleporting = false;
    }
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour
{
    [Header("1. Destino y Referencias")]
    [Tooltip("Las coordenadas de destino (X, Y, Z).")]
    public Vector3 targetPosition = new Vector3(0, 5, 0);

    [Tooltip("Arrastra aquí el objeto con el script ScreenFader.")]
    public ScreenFader screenFader;

    [Tooltip("Etiqueta (Tag) del objeto que puede usar el teletransportador.")]
    public string playerTag = "Player";

    // --- NUEVO ---
    private bool isTeleporting = false;
    // -------------

    // Asegurarse de tener la referencia al fader.
    private void Start()
    {
        if (screenFader == null)
        {
            // Busca el fader si no se asignó en el Inspector.
            screenFader = FindObjectOfType<ScreenFader>();
            if (screenFader == null)
            {
                Debug.LogError("No se encontró ScreenFader en la escena. El teletransporte no tendrá efecto de fundido.");
            }
        }
        // El fader debe empezar transparente.
        if (screenFader != null)
        {
            screenFader.GetComponent<CanvasGroup>().alpha = 0f;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !isTeleporting)
        {
            // Inicia la corrutina de teletransporte con fundido
            StartCoroutine(TeleportWithFade(other.transform));
        }
    }

    // Corrutina para manejar la secuencia de fundido, teletransporte y vuelta al juego.
    private IEnumerator TeleportWithFade(Transform playerTransform)
    {
        isTeleporting = true;

        // 1. FUNDIDO A NEGRO (Fade Out)
        if (screenFader != null)
        {
            // Pone la pantalla en negro (targetAlpha = 1)
            yield return StartCoroutine(screenFader.Fade(1f));
        }

        // 2. TELETRANSPORTE (Ocurre instantáneamente mientras la pantalla está negra)
        playerTransform.position = targetPosition;
        Debug.Log("Teletransporte completado a " + targetPosition);

        // 3. FUNDIDO A LA ESCENA (Fade In)
        if (screenFader != null)
        {
            // Vuelve a hacer visible la escena (targetAlpha = 0)
            yield return StartCoroutine(screenFader.Fade(0f));
        }

        isTeleporting = false;
    }
}

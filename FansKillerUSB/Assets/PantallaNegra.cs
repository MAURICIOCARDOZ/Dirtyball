using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    // Canvas Group para controlar la transparencia de la imagen
    private CanvasGroup fadeCanvasGroup;
    private Image fadeImage;

    [Tooltip("Tiempo que tarda la pantalla en ponerse negra o volver a la normalidad.")]
    public float fadeDuration = 0.5f; 
    
    // Tiempo que la pantalla se mantiene completamente oscura (alpha = 1).
    [Tooltip("Tiempo que la pantalla se mantiene completamente oscura (alpha = 1).")]
    public float holdDuration = 1.0f; // Puedes aumentar este valor en el Inspector para la pausa.

    private void Awake()
    {
        // Obtiene las referencias necesarias
        fadeCanvasGroup = GetComponent<CanvasGroup>();
        fadeImage = GetComponentInChildren<Image>();

        if (fadeCanvasGroup == null || fadeImage == null)
        {
            Debug.LogError("ScreenFader requiere un CanvasGroup y una Image hija. ¡Asegúrate de configurar el Canvas!");
        }
    }

    /// <summary>
    /// Inicia el proceso de fundido. Utiliza una estructura basada en Time.time más robusta.
    /// </summary>
    /// <param name="targetAlpha">0f para transparente (desvanecer) o 1f para opaco (ponerse negro).</param>
    public IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float startTime = Time.time;
        float endTime = startTime + fadeDuration;

        // Bucle que se ejecuta hasta que el tiempo actual supera el tiempo final
        while (Time.time < endTime)
        {
            // Calcula la proporción de tiempo transcurrido (entre 0 y 1)
            float t = (Time.time - startTime) / fadeDuration;
            
            // La transparencia se mueve gradualmente.
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            
            yield return null; // Espera un frame
        }

        // Asegura el valor final (0f para desvanecer, 1f para negro)
        fadeCanvasGroup.alpha = targetAlpha;
    }

    // ----------------------------------------------------------------------------------
    // --- MÉTODOS PÚBLICOS DE ORQUESTACIÓN (CONTROLAN LA PAUSA) ---
    // ----------------------------------------------------------------------------------

    /// <summary>
    /// Inicia el desvanecimiento de la pantalla negra (Fade-In: alpha -> 0).
    /// </summary>
    public IEnumerator FadeToClear()
    {
        yield return StartCoroutine(Fade(0f));
    }
    
    /// <summary>
    /// Inicia el fundido a negro (Fade-Out: alpha -> 1).
    /// </summary>
    public IEnumerator FadeToBlack()
    {
        yield return StartCoroutine(Fade(1f));
    }
    
    /// <summary>
    /// Realiza la secuencia completa: Fundido a negro, Pausa (holdDuration), y Desvanecimiento.
    /// **DEBES LLAMAR A ESTE MÉTODO PARA QUE LA PAUSA FUNCIONE.**
    /// </summary>
    public IEnumerator FadeAndHold()
    {
        // 1. FADE OUT (Se pone negro de a poco)
        yield return StartCoroutine(FadeToBlack()); 
        
        Debug.Log("Pantalla completamente negra. Iniciando pausa de " + holdDuration + " segundos.");

        // 2. PAUSA/HOLD (NUEVA LÓGICA FORZADA)
        // Usamos un bucle while para garantizar que la pausa se respete frame a frame.
        float pausaEndTime = Time.time + holdDuration;

        yield return new WaitForSeconds(2.5f);

        // 3. FADE IN (Vuelve a ser visible de a poco)
        yield return StartCoroutine(FadeToClear());

        Debug.Log("Transición completada.");
    }
}
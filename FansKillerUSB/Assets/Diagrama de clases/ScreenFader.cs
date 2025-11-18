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

    private void Awake()
    {
        // Asegúrate de que este objeto tenga un Canvas Group y una Image (debe ser de color negro).
        fadeCanvasGroup = GetComponent<CanvasGroup>();
        fadeImage = GetComponentInChildren<Image>();

        if (fadeCanvasGroup == null || fadeImage == null)
        {
            Debug.LogError("ScreenFader requiere un CanvasGroup y una Image hija. ¡Asegúrate de configurar el Canvas!");
        }
    }

    /// <summary>
    /// Inicia el proceso de fundido.
    /// </summary>
    /// <param name="targetAlpha">0f para fade-in (visible) o 1f para fade-out (negro).</param>
    /// <returns>IEnumerator para usar en StartCoroutine.</returns>
    public IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float time = 0;

        // Bucle que cambia la transparencia con el tiempo
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null; // Espera un frame
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }
}
using UnityEngine;
using System.Collections; 

public class ControladorUI : MonoBehaviour
{
    public GameObject canvasGanar;
    public GameObject canvasPerder;
    public GameObject canvasNivel;
    public AtributosPersonaje PersonajeJugador;

    private const float TIEMPO_MOSTRAR_NIVEL = 3f; 

    // **********************************************
    // MÉTODO START() AÑADIDO
    // **********************************************
    void Start()
    {
        // 1. Llama a la función de inicio que contiene la lógica de congelación/temporizador.
        MostrarCanvasInicio();
        
        // 2. Opcional: Asegúrate de que el canvas de perder esté inactivo al inicio.
        // if (canvasPerder != null) canvasPerder.SetActive(false); 
    }
    // **********************************************


    public void MostrarCanvasPerder()
    {
        Debug.Log("MOSTRANDO EL CANVAS DE GAME OVER");
        if (canvasPerder != null)
        {
            canvasPerder.SetActive(true);
        }
        else
        {
            Debug.Log("NO EXISTE EL CANVASSSSS.");
        }
    }
    public void MostrarCanvasGanar()
    {
        Debug.Log("MOSTRANDO EL CANVAS DE GANAR");
        if (canvasGanar != null)
        {
            canvasGanar.SetActive(true);
        }
        else
        {
            Debug.Log("NO EXISTE EL CANVASSSSS.");
        }
    }
    
    public void MostrarCanvasInicio()
    {
        Debug.Log("INICIANDO SECUENCIA DE NIVEL (Congelar, Mostrar, Esperar, Descongelar)");
        
        if (canvasNivel == null)
        {
            Debug.LogError("Canvas Nivel no asignado en el Inspector.");
            return;
        }

        // 1. Congelar el Tiempo
        Time.timeScale = 0f;

        // 2. Mostrar el Canvas
        canvasNivel.SetActive(true);
        
        // **Añade esta línea de debug:**
        Debug.Log("Canvas Nivel DEBERÍA estar ACTIVO ahora. Comprueba el Inspector.");

        // 3. Iniciar la Corutina.
        StartCoroutine(EsperaYReanudaJuego(TIEMPO_MOSTRAR_NIVEL)); 
    }
    
    private IEnumerator EsperaYReanudaJuego(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        // 4. Desactivar el Canvas
        canvasNivel.SetActive(false);

        // 5. Descongelar el Tiempo (El juego se reanuda)
        Time.timeScale = 1f;
        Debug.Log("Juego reanudado.");
    }

    void Update()
    {
        if (PersonajeJugador != null)
        {
            if (PersonajeJugador.VidaActual <= 0)
            {
                MostrarCanvasPerder();
            }
        }
    }
}
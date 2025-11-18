using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class ItemCuracion : MonoBehaviour
{
    // --- Configuración de Curación ---
    [Header("Configuración de Curación")]
    public int cantidadDeCuracion = 25; 

    private const string PlayerTag = "Player";
    private bool jugadorEstaDentro = false;
    private AtributosPersonaje atributosJugadorActual;

    // --- Variables de Lógica de Mejora (Buff) ---
    [Header("Lógica de Mejora (Buff)")]
    public float espera = 0f; 
    public bool disponible = false; 
    public float tiempoMaximoEspera = 30f;
    
    // --- VARIABLES DE DESBLOQUEO Y CUCHILLO ---
    [Header("Mecánica de Desbloqueo (Cuchillo)")]
    [Tooltip("El contador de veces que el cronómetro llegó a 0.")]
    public int contadorActivaciones = 0; 
    public const int ACTIVACIONES_REQUERIDAS = 5; 

    [Tooltip("El GameObject que debe activarse cuando el cuchillo está disponible (ej: el ícono del Power-up).")]
    public GameObject objetoCuchilloParaActivar; // Se usa su estado activo como el booleano 'cuchillo'

    // --- Configuración de UI ---
    [Header("Configuración de UI")]
    [Tooltip("El GameObject del mensaje de aviso regular de curación.")]
    public GameObject avisoDisponibleUI; 
    
    [Tooltip("El GameObject del mensaje de aviso cuando el CUCHILLO se desbloquea.")]
    public GameObject avisoCuchilloDesbloqueadoUI; 
    
    [Tooltip("El componente Image de la barra de carga (Tipo: Filled).")]
    public Image barraDeCarga; 

    private const float TIEMPO_VISIBILIDAD_AVISO = 3f; 

    
    void Start()
    {
        // Ocultar todos los elementos de UI y el objeto del cuchillo al inicio.
        if (avisoDisponibleUI != null) avisoDisponibleUI.SetActive(false);
        if (avisoCuchilloDesbloqueadoUI != null) avisoCuchilloDesbloqueadoUI.SetActive(false);
        
        // Desactivar el GameObject del cuchillo al inicio
        if (objetoCuchilloParaActivar != null) objetoCuchilloParaActivar.SetActive(false); 
        
        if (barraDeCarga != null)
        {
            barraDeCarga.fillAmount = 0f;
            barraDeCarga.gameObject.SetActive(false);
        }
    }


    // --- 1. Manejo del Temporizador y Lógica de UI (MODIFICADO) ---

    private void Update()
    {
        if (espera > 0f)
        {
            espera -= Time.deltaTime; 
            
            // Actualización de la barra de carga 
            if (barraDeCarga != null)
            {
                barraDeCarga.fillAmount = 1f - (espera / tiempoMaximoEspera);
                barraDeCarga.gameObject.SetActive(true);
            }

            if (espera <= 0f)
            {
                espera = 0f;
                
                if (!disponible) 
                {
                    disponible = true;
                    contadorActivaciones++; 
                    
                    GameObject avisoAMostrar = avisoDisponibleUI; 

                    // --- LÓGICA DE CONTADOR Y DESBLOQUEO ---
                    if (contadorActivaciones == ACTIVACIONES_REQUERIDAS)
                    {
                        avisoAMostrar = avisoCuchilloDesbloqueadoUI; 
                        
                        // ¡ACTIVACIÓN DEL GAMEOBJECT CUCHILLO!
                        if (objetoCuchilloParaActivar != null)
                        {
                            objetoCuchilloParaActivar.SetActive(true); // <-- ACTIVACIÓN
                        }

                        Debug.Log("¡CUCHILLO DESBLOQUEADO! Puedes usar el power-up.");
                        contadorActivaciones = 0; 
                    }
                    
                    Debug.Log($"Cronómetro completado. Contador: {contadorActivaciones}/{ACTIVACIONES_REQUERIDAS}");
                    // --- FIN LÓGICA NUEVA ---
                    
                    // La barra de carga se queda llena (100%)
                    if (barraDeCarga != null)
                    {
                        barraDeCarga.fillAmount = 1f;
                        barraDeCarga.gameObject.SetActive(true);
                    }

                    // Muestra el aviso seleccionado
                    if (avisoAMostrar != null)
                    {
                        StopAllCoroutines(); 
                        StartCoroutine(MostrarAvisoPorTiempo(avisoAMostrar, TIEMPO_VISIBILIDAD_AVISO)); 
                    }
                }
            }
        }
        // Ocultar la barra si espera es 0 y NO está disponible (listo para reiniciar)
        else if (barraDeCarga != null && !disponible && barraDeCarga.gameObject.activeSelf) 
        {
            barraDeCarga.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Q) && jugadorEstaDentro)
        {
            ActivarOUsarMejora();
        }
    }


    // --- 2. Lógica de Activación/Uso y Reinicio (MODIFICADO) ---

    private void ActivarOUsarMejora()
    {
        // 1. CONDICIÓN PRINCIPAL DE EVALUACIÓN (USO DEL CUCHILLO)
        // Se comprueba si el GameObject del cuchillo está activo.
        if (objetoCuchilloParaActivar != null && objetoCuchilloParaActivar.activeSelf)
        {
            // ¡DESACTIVACIÓN DEL GAMEOBJECT CUCHILLO AL USARLO!
            objetoCuchilloParaActivar.SetActive(false); // <-- DESACTIVACIÓN
            
            Debug.Log("Power-up 'Cuchillo' desactivado. (Usado)");
            
            // Ocultar avisos
            if (avisoDisponibleUI != null) avisoDisponibleUI.SetActive(false);
            if (avisoCuchilloDesbloqueadoUI != null) avisoCuchilloDesbloqueadoUI.SetActive(false);
            
            // Aquí iría el código de ataque o efecto del cuchillo
            return; 
        }

        // 2. RESTO DE LA LÓGICA (Curación y Reinicio)
        
        // 2.1. Estado de Bloqueo (Cronómetro activo)
        if (espera > 0f)
        {
            Debug.Log("¡ESPERAAAAA! El cronómetro sigue corriendo.");
            
            if (avisoDisponibleUI != null) avisoDisponibleUI.SetActive(false);
            if (avisoCuchilloDesbloqueadoUI != null) avisoCuchilloDesbloqueadoUI.SetActive(false);
            
            return;
        }

        // 2.2. Estado de Consumo (Mejora disponible)
        if (disponible)
        {
            AplicarCuracion(atributosJugadorActual); 
            disponible = false;
            
            // Ocultar AMBOS avisos
            if (avisoDisponibleUI != null) avisoDisponibleUI.SetActive(false);
            if (avisoCuchilloDesbloqueadoUI != null) avisoCuchilloDesbloqueadoUI.SetActive(false);
            
            // Ocultar y vaciar la barra al consumir
            if (barraDeCarga != null)
            {
                barraDeCarga.fillAmount = 0f;
                barraDeCarga.gameObject.SetActive(false);
            }
            
            Debug.Log("Mejora consumida. Presiona Q de nuevo para reiniciar el cronómetro.");
        }
        
        // 2.3. Estado de Reinicio/Activación (No disponible y espera = 0)
        else 
        {
            espera = tiempoMaximoEspera;
            disponible = false;
            
            // Mostrar la barra para iniciar la carga
            if (barraDeCarga != null)
            {
                barraDeCarga.fillAmount = 0f;
                barraDeCarga.gameObject.SetActive(true);
            }

            Debug.Log($"Cronómetro de mejora iniciado/reiniciado. {tiempoMaximoEspera} segundos para estar disponible.");
        }
    }

    // --- 3. Corutina para Mostrar/Ocultar el aviso ---
    
    IEnumerator MostrarAvisoPorTiempo(GameObject aviso, float duracion)
    {
        if (aviso != null)
        {
            aviso.SetActive(true);
            yield return new WaitForSeconds(duracion);
            
            // Solo desactiva si el objeto actual aún está disponible
            if (aviso != null && disponible)
            {
                aviso.SetActive(false);
            }
        }
    }

    // --- 4. Métodos de Colisión y Curación (SIN CAMBIOS) ---
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerTag))
        {
            atributosJugadorActual = other.gameObject.GetComponent<AtributosPersonaje>();
            if (atributosJugadorActual != null)
            {
                jugadorEstaDentro = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerTag))
        {
            jugadorEstaDentro = false;
            atributosJugadorActual = null;
        }
    }

    private void AplicarCuracion(AtributosPersonaje target)
    {
        if (target != null)
        {
            target.RecibirCuracion(cantidadDeCuracion);
        }
    }
}
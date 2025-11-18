using UnityEngine;
using UnityEngine.SceneManagement; // Necesario si quieres cambiar de escena

// Este script debe colocarse en un GameObject vacío en tu escena
public class ManejoBoss : MonoBehaviour
{
    // Hacemos que sea un Singleton para que otros scripts (los enemigos) puedan acceder fácilmente
    public static ManejoBoss Instance;

    [Header("Configuración del Boss")]
    // Prefab del jefe que se instanciará
    [Tooltip("Arrastra aquí el Prefab del GameObject del Jefe.")]
    public GameObject bossPrefab;

    // Punto donde aparecerá el jefe (se recomienda usar un transform en la escena)
    [Tooltip("Arrastra aquí el Transform que indica dónde aparecerá el Jefe.")]
    public Transform bossSpawnPoint;

    [Header("UI y Eventos")]
    // Canvas que se activará (e.g., "¡Jefe Aproximándose!")
    [Tooltip("Arrastra aquí el objeto Canvas que quieres activar.")]
    public GameObject bossCanvas;

    // Contador interno de enemigos activos en la escena
    private int activeEnemyCount = 0;
    private bool bossSpawned = false;

  


    private void Awake()
    {
        // Implementación del Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método que cada enemigo debe llamar al nacer (al instanciarse)
    public void RegistrarEnemigo()
    {
        activeEnemyCount++;
        Debug.Log("Enemigo registrado. Total de enemigos restantes: " + activeEnemyCount);
    }

    // Método que cada enemigo debe llamar al morir
    public void RestarEnemigoMuerto()
    {
        if (activeEnemyCount > 0)
        {
            activeEnemyCount--;
            Debug.Log("Un enemigo ha muerto. Enemigos restantes: " + activeEnemyCount);

            // Verificamos si es el momento de disparar el encuentro con el jefe
            if (activeEnemyCount <= 0 && !bossSpawned)
            {
                Debug.Log("ACTIVAR BOSSSSS");
                TriggerBossEncounter();
            }
        }
    }

    // Lógica principal para iniciar el encuentro
    private void TriggerBossEncounter()
    {
        bossSpawned = true;
        Debug.Log("¡Todos los enemigos han sido derrotados! Iniciando Encuentro con el Jefe.");

        // 1. Activar la Canvas de advertencia (si está asignada)
        if (bossCanvas != null)
        {
            bossCanvas.SetActive(true);
            // Puedes agregar un delay aquí si quieres que la canvas se quede un tiempo visible
            // Invoke("SpawnBoss", 3f); // Descomentar para esperar 3 segundos

            // Si no hay delay, simplemente spawnear de inmediato
            SpawnBoss();
        }
        else
        {
            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            // Opción 2: Instanciar el Jefe en la escena actual
            Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
            Debug.Log("Jefe instanciado en la escena actual.");
        }
        else
        {
            Debug.LogError("Error: No se asignó Prefab del Jefe, Punto de Spawn, o Nombre de Escena. El encuentro no puede iniciar.");
        }
    }
}

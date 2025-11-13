using System;
using UnityEngine;

public class AtributosEnemigoBase : MonoBehaviour
{
    public int VidaActual = 100;
    public int VidaMaxima = 100; // Es útil tener un máximo
    public int Velocidad = 50;
    public int Salto = 5;
    public int daño = 1;
    public float cooldown = 1f;
    public float rango = 10f;

    public Transform objetivo;
    public Animator animator;
    public GameObject[] armas;




    //Tomar la clase de Barra de vida
    void Start()
    {
        VidaActual = VidaMaxima;
        float sqrRango = this.rango * this.rango; // Pre-calcular esto en Start o Awake


        //Setear el indice del arma
        if (armas == null || armas.Length == 0)
        {
            Debug.LogWarning(gameObject.name + ": La lista de armas está vacía.");
            return;
        }
        else
        {
            // 2. Selecciona un índice aleatorio
            int indiceAleatorio = UnityEngine.Random.Range(0, armas.Length);
            setearArma(indiceAleatorio);
        }

    }
    public void RecibirDanoEnemigo(int dano)
    {
        VidaActual -= dano;
        // Limita la vida para que no baje de 0
        VidaActual = Mathf.Max(VidaActual, 0);

        Debug.Log(gameObject.name + " recibió " + dano + ". Vida restante: " + VidaActual);

        if (VidaActual <= 0)
        {
            Morir_enemigo();
        }
    }

    public void Morir_enemigo()
    {
        Debug.Log(gameObject.name + " ha muerto.");
        // Destruye el objeto al que está adjunto este script
        Destroy(gameObject);

    }
    void Update()
    {
        if (objetivo != null)
        {
            // Calcula la dirección hacia el objetivo
            Vector3 direccion = (objetivo.position - transform.position).normalized;
            
            // Calcula la magnitud al cuadrado (distancia al cuadrado)
            float sqrDistancia = ((objetivo.position - transform.position).sqrMagnitude) /100;

            if (sqrDistancia <= this.rango)
            {
                // Mueve la esfera en esa dirección
                transform.position += direccion * Velocidad * Time.deltaTime;

                // Actualiza la animación para reflejar el movimiento
                if (animator != null)
                {
                    animator.SetFloat("Velx", direccion.x);
                    animator.SetFloat("Vely", direccion.z);
                }
            }
        }
    }

    public void setearArma(int indiceArma)
    {
        GameObject armaPrefab = armas[indiceArma];

        // 4. Instancia el arma como hijo del enemigo
        // Opcional: Esto asume que las armas no están ya en la escena y se deben generar.
        GameObject nuevaArmaGO = Instantiate(armaPrefab, transform);

        // Configura la posición y rotación si es necesario (ej: nuevaArmaGO.transform.localPosition = Vector3.zero;)
  

        // 5. Obtiene el script AtributosArma de la nueva arma
        ArmasClase atributosArma = nuevaArmaGO.GetComponent<ArmasClase>();

        if (atributosArma != null)
        {
            Debug.Log("HUBI ARMA");
            Debug.Log(atributosArma.daño);
            // 6. Asigna el daño del arma a la variable del enemigo
            this.daño = atributosArma.daño; // Asigna el daño del arma a la variable 'daño' del enemigo
            nuevaArmaGO.SetActive(true);
            
        }
    }

}
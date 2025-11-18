using System;
using UnityEngine;
//MANEJA LOS ATRIBUTOS DEL ENEMIGO BASE Y REGISTRA EN EL MANEJO DE BOSS
public class AtributosEnemigoBase : MonoBehaviour
{
    public int VidaActual = 100;
    public int VidaMaxima = 100; // Es útil tener un máximo
    public int Salto = 5;
    public int daño = 1;
    public float cooldown = 1f;

    public Animator animator;
    public GameObject[] armas;
    
    public Vector3 PosicionLocalArma = new Vector3(0.5f, 1.0f, 0.2f);

    [Tooltip("El Empty/Socket en la mano donde se adjuntarán las armas.")]
    public Transform HandSocket;


    //Tomar la clase de Barra de vida
    void Start()
    {
        VidaActual = VidaMaxima;

        if (ManejoBoss.Instance != null)
        {
            ManejoBoss.Instance.RegistrarEnemigo();
        }


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
        ManejoBoss.Instance.RestarEnemigoMuerto();

    }

    public void setearArma(int indiceArma)
    {
        GameObject armaPrefab = armas[indiceArma];

        // 4. Instancia el arma como hijo del enemigo
        // Opcional: Esto asume que las armas no están ya en la escena y se deben generar.
        GameObject nuevaArmaGO = Instantiate(armaPrefab, HandSocket);

        // Configura la posición y rotación si es necesario (ej: nuevaArmaGO.transform.localPosition = Vector3.zero;)
        nuevaArmaGO.transform.localPosition = PosicionLocalArma;

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
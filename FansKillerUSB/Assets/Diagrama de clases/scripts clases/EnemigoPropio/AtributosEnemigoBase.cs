using UnityEngine;

public class AtributosEnemigoBase : MonoBehaviour
{
    public int VidaActual = 100;
    public int VidaMaxima = 100; // Es útil tener un máximo
    public int Velocidad = 50;
    public int Salto = 5;
    public int daño = 1;
    public float cooldown = 1f;
    public int rango = 1;



    //Tomar la clase de Barra de vida
    void Start()
    {
        VidaActual = VidaMaxima;
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

    
}
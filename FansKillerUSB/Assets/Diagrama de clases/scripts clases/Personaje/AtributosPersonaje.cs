using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtributosPersonaje : MonoBehaviour
{
    public int VidaActual = 100;
    public int VidaMaxima = 100; // Es útil tener un máximo
    public int Velocidad = 50;
    public int Salto = 5;
    public int daño = 1;
    public float cooldown = 1f;
    public int rango = 1;
    public int Arma = 0;
    public bool EstaAtacando = false;

    private bool estaMuerto = false;

    //Tomar la clase de Barra de vida
    public Image barraDeVida;

    public ControladorUI controladorUIScript; // Referencia al script de ControladorUI

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VidaActual = VidaMaxima;// Setea la vida actual al maximo al iniciar

        // ¡Asegúrate de inicializar la barra aquí!
        if (barraDeVida != null)
        {
            ActualizarBarra();
        }
        else
        {
            Debug.LogWarning("Barra de vida no asignada en el Inspector.");
        }
    }

    public void RecibirDano(int dano)
    {
        VidaActual -= dano;
        // Limita la vida para que no baje de 0
        VidaActual = Mathf.Max(VidaActual, 0);

        Debug.Log(gameObject.name + " recibió " + dano + ". Vida restante: " + VidaActual);

        //Actualizar la barra de vida
        if (barraDeVida != null)
        { // Llama a la función del otro script para sincronizar la UI
            ActualizarBarra();
            Debug.Log("SE ACTUALIZO LA BARRA DE VIDA");
        }

        if (VidaActual <= 0 && !estaMuerto) //Solo se llama una vezc a Morir()
        {
            estaMuerto = true;
            Morir();
        }
    }

    // Nuevo método para manejar la muerte
    public void Morir()
{
    Destroy(gameObject);
}


    public void ActualizarBarra()
    {
         if (barraDeVida != null)
        {
            // Usamos las propiedades de AtributosPersonaje para calcular el fillAmount
            barraDeVida.fillAmount = (float)VidaActual / VidaMaxima;
        }
    }


    //Cambiar atributos al recoger un arma
    // Nuevo método para actualizar los atributos con los del arma
    public void SetearAtributosDeArma(int nuevoDano, float nuevoCooldown, int nuevoRango)
    {
        this.daño = nuevoDano;
        this.cooldown = nuevoCooldown;
        this.rango = nuevoRango;

        Debug.Log($"Atributos del personaje actualizados: Daño={this.daño}, Cooldown={this.cooldown}, Rango={this.rango}");
    }

    public void RecibirCuracion(int cantidad)
    {
        // 1. Calcula la nueva vida, asegurando que no exceda la VidaMaxima
        VidaActual += cantidad;
        VidaActual = Mathf.Min(VidaActual, VidaMaxima); // Mathf.Min evita que la vida exceda el máximo

        Debug.Log(gameObject.name + " se curó " + cantidad + ". Vida actual: " + VidaActual);

        // 2. Actualiza la barra de vida
        if (barraDeVida != null)
        {
            ActualizarBarra();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BalaEnemigo"))
        {
            RecibirDano(50);
        }
    }
}

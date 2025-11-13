using UnityEngine;

//Este script maneja el ataque del personaje jugador
public class ManejoDeAtaque : AtributosPersonaje
{
    private const string EnemyTag = "Enemigo";
    public DamagePopUpGenerator damagePopUpGenerator; // Control del popup de daño

    // Referencia al script de atributos del JUGADOR para obtener el daño actual
    private AtributosPersonaje misAtributos;

    // Referencia al Collider de Ataque (para activarlo/desactivarlo)
    public Collider attackCollider;

    private float tiempoSiguienteAtaque = 0f;

    // Variable para controlar si podemos atacar (basado en el cooldown)


    void Start()
    {
        // Desactivar el collider de ataque al inicio
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }
    
    void Update()
    {
        // AÑADE LA VERIFICACIÓN DEL COOLDOWN AQUÍ
        if (Input.GetKeyDown(KeyCode.P)) //&& Time.time >= tiempoSiguienteAtaque) // <--- ¡Añadir esto!
        {
            Debug.Log("Intentando atacar..."); // Este mensaje es correcto, significa que el ataque se inicia.
            IniciarAtaque();
            // Establece el tiempo para el próximo ataque (Cooldown)
            tiempoSiguienteAtaque = Time.time + this.cooldown;
        }
    }

    private void IniciarAtaque()
    {
        this.EstaAtacando = true;

        if (attackCollider != null)
        {
            attackCollider.enabled = true;
        }

        // Programa una llamada para finalizar el ataque
        Invoke("FinalizarAtaque", 0.2f);
    }

    private void FinalizarAtaque()
    {
        // Usamos 'this.EstaAtacando'
        this.EstaAtacando = false;

    }

    private void OnCollisionEnter(Collision other)
    {
         // Verifica si el objeto colisionado tiene la etiqueta "Enemigo"
        if (other.gameObject.tag == EnemyTag && this.EstaAtacando)
        {
            Debug.Log("Colisión con enemigo detectada.");

            // Obtén el componente AtributosEnemigoBase del enemigo
            AtributosEnemigoBase atributosEnemigo = other.gameObject.GetComponent<AtributosEnemigoBase>();

            if (atributosEnemigo != null)
            {
                // Aplica el daño al enemigo
                atributosEnemigo.RecibirDanoEnemigo(this.daño);
                damagePopUpGenerator.CreatePopUp(this.transform.position, this.daño.ToString());
                Debug.Log("Enemigo golpeado por " + this.daño + " de daño.");
            }
            else
            {
                Debug.LogWarning("El objeto colisionado no tiene el componente AtributosEnemigoBase.");
            }
        }
    }
}

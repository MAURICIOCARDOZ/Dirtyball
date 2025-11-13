using UnityEngine;

public class PerseguirObjetivo : MonoBehaviour
{
    public Transform objetivo;
    public float velocidad = 5f;
    public Animator animator;

    void Update()
    {
        if (objetivo != null)
        {
            // Calcula la dirección hacia el objetivo
            Vector3 direccion = (objetivo.position - transform.position).normalized;
             float distancia = Vector3.Distance(this.transform.position, objetivo.position);

            // Mueve la esfera en esa dirección
            transform.position += direccion * velocidad * Time.deltaTime;

            // Actualiza la animación para reflejar el movimiento
            if (animator != null)
            {
                animator.SetFloat("Velx", direccion.x);
                animator.SetFloat("Vely", direccion.z);
            }
        }
    }
}
using UnityEngine;
using UnityEngine.AI;

public class EnemigoRango : MonoBehaviour
{
    public NavMeshAgent Enemigo;
    public float Velocidad;
    public bool Persiguiendo;
    public float Rango;
    float Distancia;
    public Transform Objetivo;

    //[Header("Animaciones")]
    public Animation Anim;
    public string NombreAnimacionCaminar;
    public string NombreAnimacionQuieto;

    public float DistanciaExtra = 2;
    private void Update()
    {
        Distancia = Vector3.Distance(Enemigo.transform.position, Objetivo.position);
        if (Distancia < Rango) //Activamos el perseguir si la distiancia es menor al rango que le damos
        {
            Persiguiendo = true;
        }
        else if (Distancia > Rango + DistanciaExtra)
        { //Rango se cambiara a lo que nosotros querramos  
            Persiguiendo = false;
        }

        if (Persiguiendo == false)
        {
            Enemigo.speed = 0;
            //Anim.CrossFade(NombreAnimacionQuieto); 
        }
        else if (Persiguiendo == true)
        {
            Enemigo.speed = Velocidad;
            //Anim.CrossFade(NombreAnimacionCaminar);
            Enemigo.SetDestination(Objetivo.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Enemigo.transform.position, Rango);   
    }
}   
 
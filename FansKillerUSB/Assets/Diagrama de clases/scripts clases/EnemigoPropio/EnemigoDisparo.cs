using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemigoDisparo : MonoBehaviour
{
    public GameObject BalaEnemigo;
    public Transform spawnBalaPoint;
    private Transform PosicionJugador;

    public float VelocidadBala = 20f;
    public float CantPredict = 5f; //Cuanto mas adelante de la distandcia a la que mira el jugador se disparara;
    public float Cadencia = 3f;

    private Animator Animator;

    void Start()
    {
        GameObject Jugador = GameObject.FindGameObjectWithTag("Player");
        if (Jugador != null)
        {
            PosicionJugador = Jugador.transform;
            Animator = GetComponent<Animator>();
       
            InvokeRepeating("WrapperJugadorDisparo", Cadencia,Cadencia);
        }    
    }

    void WrapperJugadorDisparo()
    {
        StartCoroutine(DisparoJugadorCorutina());
    }

    private IEnumerator DisparoJugadorCorutina()
    {
        if (PosicionJugador == null) yield break;

        Animator.SetTrigger("Lanzando");
        yield return new WaitForSeconds(0.7f);

        Vector3 DireccionPredecida = PosicionJugador.position + PosicionJugador.forward * CantPredict;
        Vector3 DireccionBala = DireccionPredecida - transform.position;

        GameObject BalaNueva;
        
        BalaNueva = Instantiate(BalaEnemigo,spawnBalaPoint.position, spawnBalaPoint.rotation);

        BalaNueva.GetComponent<Rigidbody>().AddForce(DireccionBala.normalized * VelocidadBala, ForceMode.VelocityChange);


    }
}

using UnityEngine;

public class EnemigoDisparo : MonoBehaviour
{
    public GameObject BalaEnemigo;
    public Transform spawnBalaPoint;
    private Transform PosicionJugador;

    public float VelocidadBala;

    void Start()
    {
        GameObject Jugador = GameObject.FindGameObjectWithTag("Player");
        if (Jugador != null)
        {
            PosicionJugador = Jugador.transform;
            Invoke("DisparoJugador",3);
        }    
    }

    void Update()
    {
    }

    void DisparoJugador()
    {
        Vector3 DireccionJugador = PosicionJugador.position - transform.position;

        GameObject BalaNueva;
        
        BalaNueva = Instantiate(BalaEnemigo,spawnBalaPoint.position, spawnBalaPoint.rotation);

        BalaNueva.GetComponent<Rigidbody>().AddForce(DireccionJugador * VelocidadBala, ForceMode.Force);


        Invoke("DisparoJugador", 3);
    }
}

using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    // Variables para el movimiento horizontal
    public float velocidad = 5.0f;
    
    // Variables para el salto
    public float fuerzaSalto = 10f;
    public Transform verificadorSuelo;
    public LayerMask capaSuelo;
    private bool estaEnSuelo;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Verifica si el personaje está en el suelo usando un "raycast"
        estaEnSuelo = Physics.CheckSphere(verificadorSuelo.position, 0.2f, capaSuelo);

        // Detecta la pulsación de la tecla Espacio y salta si está en el suelo
        if (Input.GetButtonDown("Jump") && estaEnSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");

        Vector3 movimiento = new Vector3(movimientoHorizontal, 0f, movimientoVertical);
        rb.linearVelocity = movimiento * velocidad + new Vector3(0, rb.linearVelocity.y, 0);
    }
}
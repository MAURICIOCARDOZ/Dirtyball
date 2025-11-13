using UnityEngine;

public class MovimientoEthan : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad del jugador

    void Update()
    {
        // Obtenemos el input del teclado (WASD o flechas)
        float moveX = Input.GetAxis("Horizontal"); // A/D o Izquierda/Derecha
        float moveZ = Input.GetAxis("Vertical");   // W/S o Arriba/Abajo

        // Creamos un vector de movimiento
        Vector3 move = new Vector3(moveX, 0, moveZ);

        // Movemos al personaje (Frame-rate independiente)
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
    }
}

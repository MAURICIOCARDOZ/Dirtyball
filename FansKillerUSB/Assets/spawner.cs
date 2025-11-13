using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// componente del gamneobject plane
public class spawner : MonoBehaviour
{
    public Transform respawn;
    public int penalidad = 5;

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.position = respawn.position;
        collision.gameObject.GetComponent<Control>().incremento(-penalidad);

    }
}

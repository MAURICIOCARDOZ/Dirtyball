using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RecogerObjeto : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Moneda"))
        {
            Destroy(other.gameObject);
        }
    }
}

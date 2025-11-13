using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ActivarArmaPersonaje : MonoBehaviour
{
    public CogerArmas cogerArmas;
    public int numeroArma;
    void Start()
    {
        cogerArmas = GameObject.FindGameObjectWithTag("Player").GetComponent<CogerArmas>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            cogerArmas.ActivarArma(numeroArma);
            Destroy(gameObject);
            
        }
    }
}

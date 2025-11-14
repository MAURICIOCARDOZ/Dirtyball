using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


public class NavMeshPrueba : MonoBehaviour
{
    public GameObject Objetivo;
    NavMeshAgent agent;
    Animator anim;

    public float rango = 1000;
    public float tiempoEspera = 10;
    public float quieto = 10;

    bool esperando = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distancia = Vector3.Distance(transform.position, Objetivo.transform.position);
        if (distancia < rango)
        {
            Vector3 direccion = (Objetivo.transform.position - transform.position).normalized;
            if (anim != null)
            {
                anim.SetFloat("Velx", direccion.x);
                anim.SetFloat("Vely", direccion.z);
            }
            agent.SetDestination(Objetivo.transform.position);
        }
    }
}

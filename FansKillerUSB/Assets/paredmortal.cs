using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class paredmortal : MonoBehaviour
{
    public float velocidad = 10.0f;
    private float mover = 1f;
    public bool subiendo = true;
    public int comienzo = -1;
    public int fin = 1;

    // Start is called before the first frame update
    void Start()
    {
        velocidad = 1 / velocidad;
    }

    // Update is called once per frame
    void Update()
    {

    transform.position = new Vector3((transform.position.x + mover * velocidad), transform.position.y, transform.position.z);
        if (subiendo == true)  {
            mover++;
            if (mover > fin) { subiendo = false; }
        }
        else if(subiendo == false){
             mover--;
            if (mover < comienzo) { subiendo = true; }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Control es un componente de el player
public class Control : MonoBehaviour
{
    public int puntajeJugador = 0;
    public float limiteDeTiempo = 10.0f;
    public float tiempo = 0.0f;

    // una función un valor de salida, un nombre y parametros de entrada
    public void incremento(int valor) {
        if (tiempo < limiteDeTiempo) puntajeJugador += valor;
    }


     // Update is called once per frame
    void Update()
    {
        tiempo += Time.deltaTime;
        Debug.Log(puntajeJugador);
    }

    private void OnGUI()
    {
        GUI.contentColor = Color.black;
        if (tiempo < limiteDeTiempo)
        {
            GUI.Label(new Rect(100, 10, 90, 40), "Puntaje: " + puntajeJugador);
            GUI.Label(new Rect(300, 10, 140, 40), "Tiempo: " + tiempo);
        }
        else
        {
            if (puntajeJugador < 5) { GUI.Label(new Rect(200, 10, 140, 40), "PERDISTE"); }
            else { GUI.Label(new Rect(200, 10, 140, 40), "GANASTE"); }
        }
    }
}

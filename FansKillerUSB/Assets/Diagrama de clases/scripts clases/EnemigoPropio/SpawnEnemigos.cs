using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemy; // Prefab del enemigo a spawnear
    private Vector3 coordinates = new Vector3(0f, 0.99f, 10); // Coordenadas donde se spawneará el enemigo

    public int enemiesToSpawn = 50;
    private int xzLimites = 45;
    void Start()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
        float xzRPosition = Random.Range(-xzLimites, xzLimites);
        Vector3 randomSpawn = new Vector3(xzRPosition, 3.99f, xzRPosition);
            Instantiate(enemy, randomSpawn, enemy.transform.rotation);
        }
    }

    void Update()
    {
        // Aquí podrías agregar lógica para spawnear enemigos en ciertos intervalos o condiciones
    }
}
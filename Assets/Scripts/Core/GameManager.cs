using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float spawnRate; 
    [SerializeField] GameObject enemyPrefab;

    public enum gameDifficulty {Easy, Medium, Hard};
    private gameDifficulty difficulty;

    private void Awake()
    {
        // difficulty = ... (PASAR DIFICULTAD ELEGIDA EN EL MAINMENU, PODRIA SER UN EVENTO) 
        
    }

    void Start()
    {
        StartGame(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemy()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnRate);
            Instantiate(enemyPrefab); 
        }
    }

    public void StartGame()
    {
        StartCoroutine(SpawnEnemy());
    }
}

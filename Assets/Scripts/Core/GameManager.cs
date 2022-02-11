using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float spawnRate; 
    [SerializeField] GameObject enemyPrefab; 
    // Start is called before the first frame update
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float enemySpawnRate; 
    [SerializeField] List<GameObject> enemyPrefabList;

    private int enemyListIndx;

    private int difficulty; // 0=easy, 1=medium, 2=hard 

    private void Awake()
    {
        difficulty = ProfileManager.sharedInstance.GetDifficulty();
    }

    void Start()
    {
        float level = (int)difficulty + 1.5f; // para no dividir por 0 
        enemySpawnRate /= level;

        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnEnemy()
    {
        while(true) // ver si reemplazar por while(!gameOver) 
        {
            yield return new WaitForSeconds(enemySpawnRate);
            enemyListIndx = Random.Range(0, enemyPrefabList.Count); 
            Instantiate(enemyPrefabList[enemyListIndx]); 
        }
    }
    
    public void StartGame()
    {
        StartCoroutine(SpawnEnemy());
    }
    
}

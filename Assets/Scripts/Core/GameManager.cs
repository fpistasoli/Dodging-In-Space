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
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnEnemy()
    {
        int level = (int)difficulty + 1; // para que no divida por 0

        while(true)
        {
            enemySpawnRate /= level; 

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

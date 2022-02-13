using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float enemySpawnRate; 
    [SerializeField] List<GameObject> enemyPrefabList;
    private int enemyListIndx; 

    public enum gameDifficulty {Easy, Medium, Hard};
    private gameDifficulty difficulty;

    private void Awake()
    {
        // difficulty = ... (PASAR DIFICULTAD ELEGIDA EN EL MAINMENU, PODRIA SER UN EVENTO) 
        
    }

    void Start()
    {
        StartGame(difficulty); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemy(gameDifficulty difficulty)
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

    public void StartGame(gameDifficulty difficulty)
    {
        StartCoroutine(SpawnEnemy(difficulty));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float enemySpawnRate; 
    [SerializeField] List<GameObject> enemyPrefabList;
    [SerializeField] private int winningBonus;

    private int enemyListIndx;
    private int difficulty; // 0=easy, 1=medium, 2=hard 
    private int score;
    private int highScore;
    private string highScoreUser;
    private string playerName;
    public bool inTheGame = true;

    public bool isGameOver = false;
    private bool isGameEnd = false;

    //Events
    public static event System.Action onNewHighScore;


    private void Awake()
    {
        difficulty = ProfileManager.sharedInstance.GetDifficulty();
        playerName = ProfileManager.sharedInstance.UserName;

        highScoreUser = ProfileManager.sharedInstance.HighScoreUser;
        highScore = ProfileManager.sharedInstance.HighScore;
    }

    void Start()
    {
        PlayerController.onGoalReached += onGoalHandler;

        float level = (int)difficulty + 1.5f; // para no dividir por 0 
        enemySpawnRate /= level;
        this.score = 0; 

        StartGame();
    }

    private void onGoalHandler()
    {
        isGameEnd = true;
        score += winningBonus;
    }

    // Update is called once per frame
    void Update()
    {
        SetHighScore();

        if (!isGameOver && !isGameEnd)
        {
            PauseButton();
        }

    }

    private void PauseButton()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (inTheGame)
            {
                Time.timeScale = 0;
                inTheGame = false;
            } 
            else
            {
                Time.timeScale = 1;
                inTheGame = true;
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        while(!isGameOver)
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
    
    public void AddPoints(int points)
    {
        this.score += points;
    }

    public int GetScore()
    {
        return this.score; 
    }

    public int GetHighScore()
    {
        return this.highScore; 
    }

    public string GetHighScoreUser()
    {
        return this.highScoreUser; 
    }

    private void SetHighScore()
    {
        if (this.highScore < this.score)
        {
            this.highScore = this.score;
            this.highScoreUser = this.playerName;

            ProfileManager.sharedInstance.HighScoreUser = this.highScoreUser.ToUpper().Trim();
            ProfileManager.sharedInstance.HighScore = this.highScore;

            onNewHighScore?.Invoke();
        }
    }

    private void OnDestroy()
    {
        PlayerController.onGoalReached -= onGoalHandler;
    }





}

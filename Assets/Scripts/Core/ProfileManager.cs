using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager sharedInstance;
    private int difficulty = 0; // 0=easy, 1=medium, 2=hard (0 po defecto)
    public string UserName { get; set; }
    public string HighScoreUser { get; set; }
    public int HighScore { get; set; }
    private int levelsQty;
    private string[] scoreUserArray; 

    private void Awake() //SINGLETON: esta clase no se destruye al cargar la escena del juego
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
            DontDestroyOnLoad(gameObject);

            LoadUserLevel(); 
        }
        else
        {
            Destroy(gameObject);
        }

    }
    
    public void SetDifficulty(int difficulty) 
    {
        this.difficulty = difficulty;
    }
    
    public int GetDifficulty()
    {
        return difficulty;
    }

    public void SetLevelsQty(int count)
    {
        this.levelsQty = count;
    }

    public string[] GetScoreUserArray()
    {
        return scoreUserArray; 
    }

    [System.Serializable]
    class SavedData
    {
        public string playerName;
        public int lastLevel;

        public string[] levelHighScoreUser; // Como no se puede usar Dictionary uso un arreglo
    }

    public void SaveUserLevel()
    {
        int indx = 3 * difficulty;

        SavedData data = new SavedData();
        data.playerName = UserName;
        data.lastLevel = difficulty;

        data.levelHighScoreUser = new string[3 * this.levelsQty];

        // [level, highScore_level, userHighScore_level, level1, highScore_level1, userHighScore_level1, ...]
        data.levelHighScoreUser[indx] = difficulty.ToString();
        data.levelHighScoreUser[indx + 1] = HighScore.ToString();
        data.levelHighScoreUser[indx + 2] = HighScoreUser; 

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);  
    }

    public void LoadUserLevel()
    {
        string path = Application.persistentDataPath + "/savefile.json"; 
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SavedData data = JsonUtility.FromJson<SavedData>(json);

            UserName = data.playerName;
            difficulty = data.lastLevel;
            scoreUserArray = data.levelHighScoreUser; 
        }
    }
 }


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

    private Dictionary<int, (int, string)> dicLevelHighScore = new Dictionary<int, (int, string)>(); 

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

    [System.Serializable]
    class SavedData
    {
        public string playerName;
        public int lastLevel;

        public string highScoreUser0;
        public int highScore0;
        public string highScoreUser1;
        public int highScore1;
        public string highScoreUser2;
        public int highScore2; 
    }

    public void SaveUserLevel()
    {
        int indx = 3 * difficulty;

        SavedData data = new SavedData();
        data.playerName = UserName;
        data.lastLevel = difficulty;

        switch (difficulty)
        {
            case 0:
                data.highScore0 = HighScore;
                data.highScoreUser0 = HighScoreUser; 
                break;
            case 1:
                data.highScore1 = HighScore;
                data.highScoreUser1 = HighScoreUser;
                break;
            case 2:
                data.highScore2 = HighScore;
                data.highScoreUser2 = HighScoreUser;
                break;
            default: 
                break; 
        }

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

            dicLevelHighScore = new Dictionary<int, (int, string)>();
            dicLevelHighScore.Add(0, (data.highScore0, data.highScoreUser0));
            dicLevelHighScore.Add(1, (data.highScore1, data.highScoreUser1));
            dicLevelHighScore.Add(2, (data.highScore2, data.highScoreUser2));
        }
    }

    public Dictionary<int, (int, string)> GetDicLevelHighScore()
    {
        return dicLevelHighScore; 
    }
 }


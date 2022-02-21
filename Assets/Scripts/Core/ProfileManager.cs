using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager sharedInstance;
    private int difficulty = 0; // 0=easy, 1=medium, 2=hard (0 po defecto)
    public string UserName { get; set; } 

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
    }

    public void SaveUserLevel()
    {
        SavedData data = new SavedData();
        data.playerName = UserName;
        data.lastLevel = difficulty;

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
        }
    }
 }


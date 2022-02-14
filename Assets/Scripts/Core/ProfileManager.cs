using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager sharedInstance;
    private int difficulty = 0; // 0=easy, 1=medium, 2=hard (0 po defecto)

    private void Awake() //SINGLETON: esta clase no se destruye al cargar la escena del juego
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
            DontDestroyOnLoad(gameObject);
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

 }


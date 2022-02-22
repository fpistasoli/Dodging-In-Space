using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; 

#if UNITY_EDITOR
using UnityEditor;
#endif
public class MainMenuController : MonoBehaviour
{

    [SerializeField] Dropdown difficultyDropdown;
    [SerializeField] TMP_InputField inputName;

    void Start()
    {
        inputName.text = ProfileManager.sharedInstance.UserName;
        difficultyDropdown.value = ProfileManager.sharedInstance.GetDifficulty();
   
        SetHighScoreUser();
    }

    void Update()
    {
        
    }

    public void SetDifficulty()
    {
        int selectedDifficulty = difficultyDropdown.value;
        ProfileManager.sharedInstance.SetDifficulty(selectedDifficulty);
    }

    // Es llamado en OnValueChange de inputName 
    public void SetName()
    {
        ProfileManager.sharedInstance.UserName = inputName.text.ToUpper().Trim(); 
    }

    private void SetHighScoreUser()
    {
        // Busco los datos de HighScore y tomo los datos de acuerdo a difficulty
        int difficulty = difficultyDropdown.value;
        Dictionary<int, (int, string)> dicDatos = ProfileManager.sharedInstance.GetDicLevelHighScore();

        if (dicDatos.Count > 0 && (ProfileManager.sharedInstance.HighScore == 0 || ProfileManager.sharedInstance.LastLevel != difficulty))
        {
            ProfileManager.sharedInstance.HighScore = dicDatos[difficulty].Item1;
            ProfileManager.sharedInstance.HighScoreUser = dicDatos[difficulty].Item2; 
        }
    }

    public void StartGame()
    {
        SetHighScoreUser();
        ProfileManager.sharedInstance.LastLevel = difficultyDropdown.value;

        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        ProfileManager.sharedInstance.SaveUserLevel(); 

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode(); 
#else
        Application.Quit(); 
#endif
    }
}

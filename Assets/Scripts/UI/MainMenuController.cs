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
        ProfileManager.sharedInstance.SetLevelsQty(difficultyDropdown.options.Count);

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
        ProfileManager.sharedInstance.UserName = inputName.text; 
    }

    private void SetHighScoreUser()
    {
        // Busco el arreglo con los datos de HighScore y tomo los datos de acuerdo a difficulty
        string[] dataHighScoreUser = ProfileManager.sharedInstance.GetScoreUserArray();
        int difficulty = difficultyDropdown.value;

        if (dataHighScoreUser.Length > 0)
        {
            if(dataHighScoreUser[3 * difficulty + 1] == "") { dataHighScoreUser[3 * difficulty + 1] = "0"; }

            ProfileManager.sharedInstance.HighScore = int.Parse(dataHighScoreUser[3 * difficulty + 1]);
            ProfileManager.sharedInstance.HighScoreUser = dataHighScoreUser[3 * difficulty + 2];
        }
    }

    public void StartGame()
    {
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

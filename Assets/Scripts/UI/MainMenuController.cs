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
    }

    void Update()
    {
        
    }

    public void SetDifficulty()
    {
        int selectedDifficulty = difficultyDropdown.value;
        ProfileManager.sharedInstance.SetDifficulty(selectedDifficulty);
    }

    public void SetName()
    {
        ProfileManager.sharedInstance.UserName = inputName.text; 
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

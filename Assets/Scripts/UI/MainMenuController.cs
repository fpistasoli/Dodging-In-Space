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
        if(ProfileManager.sharedInstance != null)
        {
            inputName.text = ProfileManager.sharedInstance.userName; 
        }
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
        ProfileManager.sharedInstance.userName = inputName.text; 
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode(); 
#else
        Application.Quit(); 
#endif
    }
}

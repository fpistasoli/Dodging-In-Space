using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class MainMenuController : MonoBehaviour
{
    
    [SerializeField] Dropdown difficultyDropdown;
    [SerializeField] TMP_InputField inputName;
    [SerializeField] GameObject inputNamePlaceholder;
    [SerializeField] Image activeAudioImage;
    [SerializeField] Image muteAudioImage;
    [SerializeField] Button creditsButton;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] Button controlsButton;
    [SerializeField] GameObject controlsPanel;

    private TextMeshProUGUI inputNamePlaceholderText;
    private bool placeholderFlashing = true;

    void Start()
    {
        inputName.text = ProfileManager.sharedInstance.UserName;
        difficultyDropdown.value = ProfileManager.sharedInstance.GetDifficulty();

        inputNamePlaceholderText = inputNamePlaceholder.GetComponent<TextMeshProUGUI>();

        SetHighScoreUser();
        ShowAudioIcon();
        StartCoroutine("ShowAndHidePlaceholder");
        HideCreditsPanel();
        HideControlsPanel();
    }

    public void HideControlsPanel()
    {
        controlsPanel.gameObject.SetActive(false);
        FindObjectOfType<AudioManager>().Play("ButtonClic");
    }

    public void HideCreditsPanel()
    {
        creditsPanel.gameObject.SetActive(false);
        FindObjectOfType<AudioManager>().Play("ButtonClic");
    }

    public void ShowCreditsPanel()
    {
        creditsPanel.gameObject.SetActive(true);
        FindObjectOfType<AudioManager>().Play("ButtonClic");
    }

    public void ShowControlsPanel()
    {
        controlsPanel.gameObject.SetActive(true);
        FindObjectOfType<AudioManager>().Play("ButtonClic");
    }

    private IEnumerator ShowAndHidePlaceholder()
    {
        int i = 0;
        while(placeholderFlashing)
        {
            ShowPlaceholder();
            yield return new WaitForSeconds(.5f);
            HidePlaceholder();
            yield return new WaitForSeconds(.5f);
            i++;
        }
        
    }

    private void HidePlaceholder()
    {
        inputNamePlaceholderText.enabled = false;
    }

    private void ShowPlaceholder()
    {
        inputNamePlaceholderText.enabled = true;
    }

    void Update()
    {

    }

    public void SetDifficulty()
    {
        int selectedDifficulty = difficultyDropdown.value;
        ProfileManager.sharedInstance.SetDifficulty(selectedDifficulty);
        FindObjectOfType<AudioManager>().Play("ButtonClic");
    }

    // Es llamado en OnValueChange de inputName 
    public void SetName()
    {
        placeholderFlashing = false;
        ProfileManager.sharedInstance.UserName = inputName.text.ToUpper().Trim(); 
    }

    private void SetHighScoreUser()
    {
        // Busco los datos de HighScore y tomo los datos de acuerdo a difficulty
        int difficulty = difficultyDropdown.value;

        Dictionary<int, (int, string)> dicDatos = ProfileManager.sharedInstance.GetDicLevelHighScore();
        if (dicDatos.Count > 0) 
        {
            ProfileManager.sharedInstance.HighScore = dicDatos[difficulty].Item1;
            ProfileManager.sharedInstance.HighScoreUser = dicDatos[difficulty].Item2; 
        }
    }

    public void StartGame()
    {
        SetHighScoreUser();
        FindObjectOfType<AudioManager>().Play("ButtonClic");
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClic");
        ProfileManager.sharedInstance.SaveUserLevel(); 

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode(); 
#else
        Application.Quit(); 
#endif
    }

    public void MuteAudio()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClic");
        if (AudioListener.volume == 1)
        {
            AudioListener.volume = 0;
            muteAudioImage.enabled = true;
            activeAudioImage.enabled = false; 
        }
        else
        {
            AudioListener.volume = 1;
            muteAudioImage.enabled = false;
            activeAudioImage.enabled = true; 
        }
    }

    private void ShowAudioIcon()
    {
        if (AudioListener.volume == 0)
        {
            muteAudioImage.enabled = true;
            activeAudioImage.enabled = false;
        }
        else
        {
            muteAudioImage.enabled = false;
            activeAudioImage.enabled = true;
        }
    }
}

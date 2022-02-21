using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{

    [SerializeField] private Text livesValue;   
    [SerializeField] private Text gameOverText;
    [SerializeField] private TMP_Text gameWonText;
    [SerializeField] private Text levelValue;
    [SerializeField] Text userName; 

    private GameObject player;
    private string default_name = "GUEST"; 

    private void Awake()
    {
        HideGameOverText();
        HideGameWonText();
        ShowDifficulty();
        ShowNameAndScore();
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        PlayerController.onGoalReached += onGoalReachedHandler; //el HUDController se suscribe a "onGoalReachedHandler", asi que cuando se llame a onGoalReached() en PlayerController, todos los suscriptores a este evento disparan sus handlers
    }

    private void onGoalReachedHandler()
    {
        gameWonText.gameObject.SetActive(true);
    }

    void Update()
    {
        UpdateLivesValue(); //se deberia hacer con un evento, para no sobrecargar el juego (ya que se actualiza solo cuando me atacan o gano una vida)
        GameOverHandler(); //tambien deberia hacerse con un evento
    }

    private void HideGameWonText()
    {
        gameWonText.gameObject.SetActive(false);
    }

    private void HideGameOverText()
    {
        gameOverText.gameObject.SetActive(false);
    }

    private void ShowDifficulty()
    {
        int difficulty = ProfileManager.sharedInstance.GetDifficulty();
        string difficultyString = "EASY"; //default value

        switch (difficulty)
        {
            case 0:
                difficultyString = "EASY";
                break;

            case 1:
                difficultyString = "MEDIUM";
                break;

            case 2:
                difficultyString = "HARD";
                break;
        }

        levelValue.text = difficultyString;

    }

    private void ShowNameAndScore()
    {
        string name = ProfileManager.sharedInstance.userName;

        // Si inputName == "" completar con default_name  
        userName.text = name.Trim() != "" ? name : default_name; 
    }

    private void GameOverHandler()
    {
        if (player.GetComponent<PlayerController>().GetLives() > 0) { return; }
        gameOverText.gameObject.SetActive(true);
    }

    private void UpdateLivesValue()
    {
        livesValue.text = player.GetComponent<PlayerController>()?.GetLives().ToString();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0); // load main menu
    }

    private void OnDestroy()
    {
        PlayerController.onGoalReached -= onGoalReachedHandler; //siempre que suscribo un evento al crear el objeto, lo desuscribo cuando lo borro por seguridad
    }

}

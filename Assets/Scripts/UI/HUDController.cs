using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{

    [SerializeField] private Text livesValue;   
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text levelValue;

    private GameObject player;

    private void Awake()
    {
        HideGameOverText();
        ShowDifficulty();
    }

    private void HideGameOverText()
    {
        gameOverText.enabled = false;
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

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        UpdateLivesValue(); //se deberia hacer con un evento, para no sobrecargar el juego (ya que se actualiza solo cuando me atacan o gano una vida)
        GameOverHandler();
    }

    private void GameOverHandler()
    {
        if (player.GetComponent<PlayerController>().GetLives() > 0) { return; }
        gameOverText.enabled = true;
        StartCoroutine(BackToMainMenu());
    }

    private void UpdateLivesValue()
    {
        livesValue.text = player.GetComponent<PlayerController>()?.GetLives().ToString();
    }

    private IEnumerator BackToMainMenu()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0); // load main menu
    }

}

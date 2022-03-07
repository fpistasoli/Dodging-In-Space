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
    [SerializeField] private TMP_Text newHighScoreText;
    [SerializeField] private TMP_Text pausedText;
    [SerializeField] private Text levelValue;
    [SerializeField] Text userName;
    [SerializeField] Text score;
    [SerializeField] Text highScoreUser;
    [SerializeField] Text highScore;
    [SerializeField] GameObject gameManager;
    [SerializeField] private float newHighScoreTextSpeed;
    [SerializeField] Canvas canvas;
    [SerializeField] Button playAgainButton;

    private GameObject player;
    private string default_name = "GUEST";


    private void Awake()
    {
        HideGameOverText();
        HideGameWonText();
        HideNewHighScoreText();
        HidePausedText();
        HidePlayAgainButton();
        ShowDifficulty();
    }

    private void HidePlayAgainButton()
    {
        playAgainButton.gameObject.SetActive(false);
    }

    private void HidePausedText()
    {
        pausedText.gameObject.SetActive(false);
    }

    private void HideNewHighScoreText()
    {
        newHighScoreText.gameObject.SetActive(false);
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        PlayerController.onGoalReached += onGoalReachedHandler; //el HUDController se suscribe a "onGoalReachedHandler", asi que cuando se llame a onGoalReached() en PlayerController, todos los suscriptores a este evento disparan sus handlers
        GameManager.onNewHighScore += onNewHighScoreHandler;

    }

    private void onNewHighScoreHandler()
    {
        ShowNewScoreText();
        //StartCoroutine(ShowNewScoreText());
    }

    private void ShowNewScoreText()
    {
        newHighScoreText.gameObject.SetActive(true);
        //yield return new WaitForSeconds(3);
        //newHighScoreText.gameObject.SetActive(false);
    }

    private void onGoalReachedHandler()
    {
        gameWonText.gameObject.SetActive(true);

        gameManager.GetComponent<GameManager>().isGameOver = true;

        player.GetComponent<PlayerController>().enabled = false;

        playAgainButton.gameObject.SetActive(true);
    }

    void Update()
    {
        UpdateLivesValue(); //se deberia hacer con un evento, para no sobrecargar el juego (ya que se actualiza solo cuando me atacan o gano una vida)
        ShowNameAndScore();
        GameOverHandler(); //tambien deberia hacerse con un evento
        PauseHandler();
        NewHighScoreTextMovement();
    }

    private void PauseHandler()
    {
        if (!gameManager.GetComponent<GameManager>().inTheGame)
        {
            pausedText.gameObject.SetActive(true);
        }
        else
        {
            pausedText.gameObject.SetActive(false);
        }
    }

    private void NewHighScoreTextMovement()
    {
        if (newHighScoreText.gameObject.activeSelf)
        {
            newHighScoreText.transform.Translate(transform.right * newHighScoreTextSpeed * Time.deltaTime);
            if (newHighScoreText.transform.position.x > canvas.GetComponent<RectTransform>().rect.position.x + canvas.GetComponent<RectTransform>().rect.width + 200f)
            {
                newHighScoreText.gameObject.SetActive(false);
            }
        }
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
        string name = ProfileManager.sharedInstance.UserName;
        userName.text = name.Trim() != "" ? name : default_name;

        score.text = gameManager.GetComponent<GameManager>().GetScore().ToString();
        highScore.text = gameManager.GetComponent<GameManager>().GetHighScore().ToString();
        highScoreUser.text = gameManager.GetComponent<GameManager>().GetHighScoreUser();
    }

    private void GameOverHandler()
    {
        if (player.GetComponent<PlayerController>().GetLives() > 0) { return; }
        gameOverText.gameObject.SetActive(true);

        gameManager.GetComponent<GameManager>().isGameOver = true;

        player.GetComponent<PlayerController>().enabled = false;

        playAgainButton.gameObject.SetActive(true);
    }

    private void UpdateLivesValue()
    {
        livesValue.text = "x " + player.GetComponent<PlayerController>()?.GetLives().ToString();
    }

    public void BackToMainMenu()
    {
        HandlePersistentData();
        SceneManager.LoadScene(0); // load main menu
    }

    private void HandlePersistentData()
    {
        ProfileManager.sharedInstance.HighScoreUser = highScoreUser.text;
        ProfileManager.sharedInstance.HighScore = int.Parse(highScore.text);
        ProfileManager.sharedInstance.SaveUserLevel();
        ProfileManager.sharedInstance.LoadUserLevel();
    }

    public void PlayAgain()
    {
        HandlePersistentData();
        SceneManager.LoadScene(1); // load current level again
    }


    private void OnDestroy()
    {
        PlayerController.onGoalReached -= onGoalReachedHandler; //siempre que suscribo un evento al crear el objeto, lo desuscribo cuando lo borro por seguridad
        GameManager.onNewHighScore -= onNewHighScoreHandler;
    }

}

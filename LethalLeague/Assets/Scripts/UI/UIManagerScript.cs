using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{
    [SerializeField] private GameManagerScript gameManager;

    // End Screen

    [SerializeField] private GameObject endScreenGroup;
    [SerializeField] private Text endScreenLabel;
    [SerializeField] private Button endRestartButton;
    [SerializeField] private Button endMenuButton;

    // Pause Screen

    [SerializeField] private GameObject pauseScreenGroup;
    [SerializeField] private Button pauseResumeButton;
    [SerializeField] private Button pauseMenuButton;
    [SerializeField] private Button pauseExitButton;

    // Game State UI

    [SerializeField] private GameObject gameUiGroup;
    [SerializeField] private HealthBar hp1;
    [SerializeField] private HealthBar hp2;

    // Effect on audio

    [SerializeField] private AudioMixer audioMixer;

    void Awake()
    {
        pauseResumeButton.onClick.AddListener(Resume);

        endMenuButton.onClick.AddListener(LoadMenuScene);
        pauseMenuButton.onClick.AddListener(LoadMenuScene);

        endRestartButton.onClick.AddListener(Restart);

        pauseExitButton.onClick.AddListener(Exit);
    }

    public void OnGameStart(Game game)
    {
        hp1.setMaxHealth(game.player1.hp);
        hp2.setMaxHealth(game.player2.hp);
    }

    public void OnGameTick(Game game)
    {
        hp1.setHealth(game.player1.hp);
        hp2.setHealth(game.player2.hp);
    }

    public void OnGameEnd(Game game)
    {
        gameUiGroup.SetActive(false);
        endScreenLabel.text = $"Player {game.GetResult()} has won !";
        endScreenGroup.SetActive(true);
    }

    void Restart()
    {
        endScreenGroup.SetActive(false);
        gameUiGroup.SetActive(true);
        gameManager.InitializeGame();
    }

    public void Resume()
    {
        audioMixer.SetFloat("thresholdMusic", 0.0f);
        pauseScreenGroup.SetActive(false);
        gameUiGroup.SetActive(true);
    }

    public void Pause()
    {
        audioMixer.SetFloat("thresholdMusic", -45.0f);
        pauseScreenGroup.SetActive(true);
        gameUiGroup.SetActive(false);
    }

    void Exit()
    {
        Application.Quit();
    }

    void LoadMenuScene()
    {
        SceneManager.LoadScene("homeScreen");
    }
}

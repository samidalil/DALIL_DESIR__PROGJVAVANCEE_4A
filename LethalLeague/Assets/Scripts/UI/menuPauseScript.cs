using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class menuPauseScript : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject playerUi;
    [SerializeField] private GameManagerScript gameManager;

    public AudioMixer audioMixer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameManager.IsRunning()) Pause();
            else Resume();
        }
    }

    public void Resume()
    {
        audioMixer.SetFloat("thresholdMusic", 0.0f);
        pauseMenuUI.SetActive(false);
        playerUi.SetActive(true);
        Time.timeScale = 1.0f;
        gameManager.ToggleRunning();
    }

    void Pause()
    {
        audioMixer.SetFloat("thresholdMusic", -45.0f);
        pauseMenuUI.SetActive(true);
        playerUi.SetActive(false);
        Time.timeScale = 0.0f;
        gameManager.ToggleRunning();
    }

    public void MenuLoad()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("homeScreen");
    }

    public void Exit()
    {
        Application.Quit();
    }
}

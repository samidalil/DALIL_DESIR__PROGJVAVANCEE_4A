using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class menuPauseScript : MonoBehaviour
{

    public static bool gameIsPaused = false;

    [SerializeField]
    public GameObject pauseMenuUI;

    public AudioMixer audioMixer;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        audioMixer.SetFloat("thresholdMusic", 0.0f);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        gameIsPaused = false;
    }

    void Pause()
    {
        audioMixer.SetFloat("thresholdMusic", -45.0f);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        gameIsPaused = true;
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

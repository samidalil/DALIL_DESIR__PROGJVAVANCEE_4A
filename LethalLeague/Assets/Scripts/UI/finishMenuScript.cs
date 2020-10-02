using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finishMenuScript : MonoBehaviour
{
    [SerializeField] public GameObject finishUI;
    [SerializeField] public GameManagerScript managerScript;

    public void Restart()
    {
        finishUI.SetActive(false);
        managerScript.Awake();
        managerScript.ToggleRunning();
    }

    public void finishLoadMenu()
    {
        SceneManager.LoadScene("homeScreen");
    }
}

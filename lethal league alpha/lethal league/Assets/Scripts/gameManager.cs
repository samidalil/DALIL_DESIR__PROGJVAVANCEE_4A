using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        Instantiate(ballPrefab, new Vector3(0.0f, 4.0f, 0.0f), Quaternion.identity);
    }
}

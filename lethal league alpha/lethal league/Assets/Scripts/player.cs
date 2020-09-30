using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField]
    private int life = 100;
    private Vector3 position = new Vector3(0.0f, 1.0f, -8.0f);

    public void takeDamage(int damage)
    {
        life -= damage;

        if(life <= 0)
        {
            Destroy(gameObject);
        }
    }
}

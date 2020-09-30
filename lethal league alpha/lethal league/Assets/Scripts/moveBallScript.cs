using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBallScript : MonoBehaviour
{
    private float speed = 10.0f;
    private Vector3 direction;

    [SerializeField]
    private LayerMask horizontalWallMask;

    [SerializeField]
    private LayerMask verticalWallMask;

    [SerializeField]
    private LayerMask playerMask;

    [SerializeField]
    private LayerMask swordMask;

    // Start is called before the first frame update
    void Start()
    {
        //float yDirection = Random.Range(-2.0f, -1.0f);
        //float zDirection = Random.Range(-4.0f, 4.0f);
        float yDirection = -1.0f; 
        direction = new Vector3(0.0f, yDirection, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += direction.normalized * Time.deltaTime * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((horizontalWallMask.value & (1 << collision.gameObject.layer)) > 0)
        {
            direction = new Vector3(0.0f, direction.y, -direction.z);
        }

        if ((verticalWallMask.value & (1 << collision.gameObject.layer)) > 0)
        {
            direction = new Vector3(0.0f, -direction.y, direction.z);
        }

        if ((playerMask.value & (1 << collision.gameObject.layer)) > 0)
        {
            player Player = collision.gameObject.GetComponent<player>();
            if(Player != null)
                Player.takeDamage(Mathf.RoundToInt(speed));
            
        }

        if ((swordMask.value & (1 << collision.gameObject.layer)) > 0)
        {
            Debug.Log(collision.gameObject.name);

        }
    }
}

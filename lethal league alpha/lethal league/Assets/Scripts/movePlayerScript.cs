using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlayerScript : MonoBehaviour
{
    [SerializeField]
    private string horizontalAxis;

    [SerializeField]
    private string verticalAxis;

    private float speed = 10.0f;
    private float jumpSpeed = 10.0f;
    private Vector3 direction;

    // Update is called once per frame
    void Update()
    {
        direction = Vector3.zero;
        if(Input.GetAxis(horizontalAxis) > 0)
        {
            direction += Vector3.forward;
            gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }

        if (Input.GetAxis(horizontalAxis) < 0)
        {
            direction += Vector3.back;
            gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }

        if(Input.GetAxis(verticalAxis) > 0)
        {
            direction += Vector3.up * jumpSpeed;
        } 
        gameObject.transform.position += direction.normalized * Time.deltaTime * speed;
    }

}

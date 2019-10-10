using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMovement : MonoBehaviour
{
    public AnimationController animationController;
    private Rigidbody2D rigidbody2D;
    public float playerSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 movement = GetDirection().normalized;
        SetDirection(movement);
        Vector2 force = movement * playerSpeed * SpeedManager.SpeedModifier;
        rigidbody2D.velocity = force;
    }

    Vector3 GetDirection() {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void SetDirection(Vector2 movement) {
        if(movement.x > 0f){    //RIGHT
            animationController.SetDirection(2);
            transform.eulerAngles = Vector3.zero;
        } 
        if(movement.y > 0f){     //UP
            animationController.SetDirection(0);
            transform.eulerAngles = new Vector3(0,0,90);
        }
        if(movement.x < 0f){     //LEFT
            animationController.SetDirection(3);
            transform.eulerAngles = new Vector3(0,0,180);
            transform.localRotation = Quaternion.Euler(0,180,0);
        }
        if(movement.y < 0f){     //DOWN
            animationController.SetDirection(1);
            transform.eulerAngles = new Vector3(0,0,270);
        }
    }
}

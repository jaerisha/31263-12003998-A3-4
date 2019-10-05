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
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 movement = GetDirection().normalized;
        SetDirection(movement);
        Vector2 force = movement * playerSpeed * SpeedManager.SpeedModifier;
        Debug.Log(force);
        rigidbody2D.velocity = force;
    }

    Vector3 GetDirection() {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void SetDirection(Vector2 movement) {
        if(movement.x > 0f) 
            animationController.SetDirection(2);
        if(movement.y > 0f) 
            animationController.SetDirection(0);
        if(movement.x < 0f)
            animationController.SetDirection(3);
        if(movement.y < 0f) 
            animationController.SetDirection(1);
    }
}

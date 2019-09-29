using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMovement : MonoBehaviour
{
    public AnimationController animationController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if(horizontalInput > 0f) 
            animationController.SetDirection(2);
        if(verticalInput > 0f) 
            animationController.SetDirection(0);
        if(horizontalInput < 0f)
            animationController.SetDirection(3);
        if(verticalInput < 0f) 
            animationController.SetDirection(1);
    }
}

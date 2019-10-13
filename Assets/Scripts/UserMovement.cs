using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMovement : MonoBehaviour
{
    public AnimationController animationController;
    private Rigidbody2D rigidbody2D;
    public float playerSpeed = 1f;
    public NodeGridGenerator nodeGridGenerator;
    private Node[,] nodes;
    private Node currentNode;
    public int startX, startY;
    private float offset = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        nodes = nodeGridGenerator.nodes;
        rigidbody2D = GetComponent<Rigidbody2D>();
        currentNode = nodes[startX, startY];
        Debug.Log("Current x: " + currentNode.gridX + offset + " y: " + currentNode.gridY + offset);
        gameObject.transform.position = new Vector3(currentNode.gridX + offset, currentNode.gridY + offset, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 movement = GetDirection().normalized;
        SetDirection(movement);
        Move(movement);
        // Debug.Log(transform.position);
        // Debug.Log("Target x: " + target.gridX + offset + " y: " + target.gridY + offset);
        
        // Vector2 force = movement * playerSpeed * SpeedManager.SpeedModifier;
        // rigidbody2D.velocity = force;
    }

    void Move(Vector2 movement){
        Node target = CheckNode(movement);
        Vector2 currentPos, targetPos;
        currentPos = new Vector2(currentNode.gridX + offset, currentNode.gridY);
        targetPos = new Vector2(target.gridX + offset, target.gridY + offset);
        if(movement.x == 0)
            transform.position = Vector2.MoveTowards(currentPos, targetPos, movement.y*playerSpeed*Time.deltaTime);
        else if(movement.y == 0)
            transform.position = Vector2.MoveTowards(currentPos, targetPos, movement.x*playerSpeed*Time.deltaTime);
        currentNode = target;
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

    Node CheckNode(Vector2 movement){
        if((movement.x < 0 || movement.x > 0) && movement.y == 0){
            if(nodes[currentNode.gridX + (int) movement.x, currentNode.gridY].walkable == true){
                return nodes[currentNode.gridX + (int) movement.x, currentNode.gridY];
            } else {
                return currentNode;
            }
        } else if((movement.y < 0 || movement.y > 0) && movement.x == 0) {
            if(nodes[currentNode.gridX, currentNode.gridY + (int) movement.y].walkable == true) {
                return nodes[currentNode.gridX, currentNode.gridY + (int) movement.y];
            } else {
                return currentNode;
            }
        }
        return currentNode;
    }
}

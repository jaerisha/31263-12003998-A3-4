using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public List<Node> openNodes;
    public List<Node> closedNodes;
    public NodeGridGenerator gridGenerator;

    // Start is called before the first frame update
    void Start()
    {
        gridGenerator = GetComponent<NodeGridGenerator>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindAPath(Node start, Node end){

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
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
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(start);

        while(openSet.Count > 0) {
            Node node = openSet[0];
            for(int i = 0; i < openSet.Count; i++) {
                if(openSet[i].fCost <= node.fCost) {
                    if(openSet[i].hCost < node.hCost){
                        node = openSet[i];
                    }
                }
            }
        }
    }
}

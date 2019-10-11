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
                openSet.Remove(node);
                closedSet.Add(node);

                if(node == end){
                    //retrace path
                    return;
                }

                foreach(Node neighbour in gridGenerator.GetNeighbours(node)){
                    if(!neighbour.walkable || closedSet.Contains(neighbour))
                        continue;
                    int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                    if(newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)){
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, end);
                        neighbour.parent = node;

                        if(!openSet.Contains(neighbour)) {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
        }
    }

    int GetDistance(Node a, Node b) {
        int xDistance = Mathf.Abs(a.gridX - b.gridX);
        int yDistance = Mathf.Abs(a.gridY - b.gridY);
        
        if(xDistance > yDistance)
            return 14 * yDistance + 10 * (xDistance - yDistance);
        else
            return 14 * xDistance + 10 * (yDistance - xDistance);
    }
}

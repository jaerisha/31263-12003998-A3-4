using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public int gridX, gridY;
    public int gCost, hCost;
    public Node parent;
    public NodeGridGenerator nodeGridGenerator;
    public Vector3 worldPos { get { return nodeGridGenerator.transform.position + new Vector3(gridX, gridY, 0); } }

    public Node(bool walkable, int gridX, int gridY, NodeGridGenerator nodeGridGenerator){
        this.walkable = walkable;
        this.gridX = gridX;
        this.gridY = gridY;
        this.nodeGridGenerator = nodeGridGenerator;
    }

    public int fCost { get { return gCost + fCost; } }

}

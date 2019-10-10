using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public int gridX, gridY;
    public int gCost, hCost;
    public Node parent;

    public Node(bool walkable, int gridX, int gridY){
        this.walkable = walkable;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost { get { return gCost + fCost; } }

}

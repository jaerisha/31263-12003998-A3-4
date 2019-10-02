using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    GameObject [,] level;
    int rows {get; set;}
    int cols {get; set;}

    public Level(int rows, int cols) {
        level = new GameObject[rows, cols];
    }

}

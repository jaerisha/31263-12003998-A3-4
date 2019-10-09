using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeGridGenerator : MonoBehaviour
{
    public Grid gridBase;
    public Tilemap floor;
    public List<Tilemap> obstacleLayers;
    public GameObject nodePrefab;
    public int startScanX, startScanY, scanFinishX, scanFinishY;
    public List<GameObject> unsortedNodes;
    public GameObject[,] nodes;
    
    // Start is called before the first frame update
    void Start()
    {
        unsortedNodes = new List<GameObject>();
        obstacleLayers = new List<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateNodes() {
        int gridX = 0;
        int gridY = 0;
        bool foundTilesOnLastPass = false;

        for(int x = startScanX; x < scanFinishX; x++){
            for(int y = startScanY; y < scanFinishY; y++){
                TileBase tb = floor.GetTile(new Vector3Int(x, y, 0));
                if(tb == null){} else {
                    bool foundObstacle = false;
                    foreach (Tilemap t in obstacleLayers)
                    {
                        TileBase tb2 = t.GetTile(new Vector3Int(x,y,0));
                    }
                }
            }
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeGridGenerator : MonoBehaviour
{
    public Grid gridBase;
    public Tilemap floor;
    public List<Tilemap> obstacleLayers;
    public Tilemap walls;
    public GameObject nodePrefab;
    public int startScanX, startScanY, scanFinishX, scanFinishY;
    public List<Node> unsortedNodes;
    public List<GameObject> uN;
    public Node[,] nodes;
    public int gridBoundX = 0, gridBoundY = 0;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        unsortedNodes = new List<Node>();
        obstacleLayers = new List<Tilemap>();
        uN = new List<GameObject>();
        CreateNodes();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
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
                bool foundObstacle = false;
                if(tb == null) {
                    TileBase tb2 = walls.GetTile(new Vector3Int(x,y,0));
                    if(tb2 != null){
                        foundObstacle = true;
                        GameObject node = Instantiate(nodePrefab,
                            new Vector3(x + gridBase.transform.position.x + 0.5f, y + gridBase.transform.position.y + 0.5f, 0), 
                            Quaternion.Euler(0,0,0));
                        node.GetComponent<SpriteRenderer>().color = Color.red;
                        Node obstacleNode = new Node(foundObstacle, gridX, gridY, this);
                        unsortedNodes.Add(obstacleNode);
                        uN.Add(node);
                        foundTilesOnLastPass = true;
                    }
                } else {
                    GameObject node = Instantiate(nodePrefab, new Vector3(x + 0.5f + gridBase.transform.position.x, y + 0.5f + gridBase.transform.position.y, 0), Quaternion.Euler(0,0,0));
                    node.GetComponent<SpriteRenderer>().color = Color.green;
                    Node walkableNode = new Node(foundObstacle, gridX, gridY, this);
                    unsortedNodes.Add(walkableNode);
                    uN.Add(node);
                    foundTilesOnLastPass = true;
                }
                gridY++;

                gridBoundX = gridX > gridBoundX ? gridX : gridBoundX;
                gridBoundY = gridY > gridBoundY ? gridY : gridBoundY;

                if(foundTilesOnLastPass == true) {
                    gridX++;
                    gridY = 0;
                    foundTilesOnLastPass = false;
                }
            } 
        }
        int leftestX = int.MaxValue, lowermostestY = int.MaxValue;
        nodes = new Node[gridBoundX+1,gridBoundY+1];
        foreach (Node n in unsortedNodes)
        {
            leftestX = Mathf.Min(leftestX, n.gridX);
            lowermostestY = Mathf.Min(lowermostestY, n.gridY);
        }
        foreach (Node n in unsortedNodes)
        {
            nodes[n.gridX + leftestX, n.gridY + lowermostestY] = n;
        }
        //insert neighboring tiles here
    }

    public List<Node> GetNeighbours(Node n) {
        List<Node> neighbours = new List<Node>();

        for(int x = -1; x <= 1; x++){
            for(int y = -1; y <= 1; y++) {
                if(x == 0 && y == 0) continue;

                int checkX = n.gridX;
                int checkY = n.gridY;
                
                if(checkX >= 0 && checkX < gridBoundX && checkY >= 0 && checkY < gridBoundY) {
                    neighbours.Add(nodes[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }
}

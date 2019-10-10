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
    public int startScanX = -20, startScanY = -12, scanFinishX = 20, scanFinishY = 12;
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
    }
    
    // Start is called before the first frame update
    void Start()
    {
        CreateNodes();
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
                        GameObject node = Instantiate(nodePrefab, new Vector3(x + 0.5f + gridBase.transform.position.x, y + 0.5f + gridBase.transform.position.y, 0), Quaternion.Euler(0,0,0));
                        node.GetComponent<SpriteRenderer>().color = Color.red;
                        Node obstacleNode = new Node(foundObstacle, x, y);
                        unsortedNodes.Add(obstacleNode);
                        uN.Add(node);
                        foundTilesOnLastPass = true;
                    }
                } else {
                    GameObject node = Instantiate(nodePrefab, new Vector3(x + 0.5f + gridBase.transform.position.x, y + 0.5f + gridBase.transform.position.y, 0), Quaternion.Euler(0,0,0));
                    node.GetComponent<SpriteRenderer>().color = Color.green;
                    Node walkableNode = new Node(foundObstacle, x, y);
                    unsortedNodes.Add(walkableNode);
                    uN.Add(node);
                    foundTilesOnLastPass = true;
                }
                gridY++;

                gridBoundX = gridX > gridBoundX ? gridX : gridBoundX;
                gridBoundY = gridY > gridBoundY ? gridX : gridBoundY;

                if(foundTilesOnLastPass == true) {
                    gridX++;
                    gridY = 0;
                    foundTilesOnLastPass = false;
                }
            } 
        }
        // Debug.Log("GridBoundX = " + gridBoundX + " & GridBoundY = " + gridBoundY);
        nodes = new Node[10000,10000];
        Debug.Log(unsortedNodes.Count);
        foreach (Node n in unsortedNodes)
        {
            Debug.Log(nodes[n.gridX, n.gridY]);
            // Debug.Log("nodeX = " + n.gridX + " and nodeY = " + n.gridY);
            // nodes[n.gridX, n.gridY] = n;
        }
        
        //insert neighboring tiles here
    }

    public List<Node> GetNeighbours(Node n, int width, int height) {
        List<Node> neighbours = new List<Node>();

        for(int x = -1; x <= 1; x++){
            for(int y = -1; y <= 1; y++) {
                if(x == 0 && y == 0) continue;

                int checkX = n.gridX;
                int checkY = n.gridY;
                
                if(checkX >= 0 && checkX < width && checkY >= 0 && checkY < height) {
                    // neighbours.Add(nodes[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }
}

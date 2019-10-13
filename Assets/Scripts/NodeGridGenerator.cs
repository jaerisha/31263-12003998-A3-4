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
	private Node[,] nodes;
	public int gridBoundX = 0, gridBoundY = 0;
	public float tileOffset = 0.5f;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		unsortedNodes = new List<Node>();
		obstacleLayers = new List<Tilemap>();
		CreateNodes();
	}

	private void CreateNodes()
	{
		int gridX = 0;
		int gridY = 0;

		for(int x = startScanX; x < scanFinishX; x++)
		{
			for(int y = startScanY; y < scanFinishY; y++)
			{
				TileBase tb2 = walls.GetTile(new Vector3Int(x, y, 0));
				bool foundObstacle = tb2 != null;
				Node obstacleNode = new Node(!foundObstacle, gridX, gridY, this);
				unsortedNodes.Add(obstacleNode);
				gridY++;

				gridBoundX = gridX > gridBoundX ? gridX : gridBoundX;
				gridBoundY = gridY > gridBoundY ? gridY : gridBoundY;
			}
			gridX++;
			gridY = 0;
		}
		//Debug.Log("gridX = " + gridX);
		//Debug.Log("gridY = " + gridY);
		//Debug.Log("gridBoundX = " + gridBoundX);
		//Debug.Log("gridBoundY = " + gridBoundY);

		int leftestX = int.MaxValue, lowermostestY = int.MaxValue;
		nodes = new Node[gridBoundX,gridBoundY];

		//Debug.Log("Nodes length: " + nodes.Length);
		foreach (Node n in unsortedNodes)
		{
			leftestX = Mathf.Min(leftestX, n.gridX);
			lowermostestY = Mathf.Min(lowermostestY, n.gridY);
		}
		foreach (Node n in unsortedNodes)
		{
			nodes[n.gridX + leftestX, n.gridY + lowermostestY] = n;
		}
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

	public Node GetClosestNode(Vector3 position)
	{
		float closestDistance = Mathf.Infinity;
		Node closestNode = null;
		for (int x = 0; x < nodes.GetUpperBound(0); x++)
		{
			for (int y = 0; y < nodes.GetUpperBound(0); y++)
			{
				Node n = NodeAt(x, y);
				if (n == null) continue;
				float dist = Vector3.SqrMagnitude(n.WorldPosition - position);
				if (dist < closestDistance)
				{
					closestDistance = dist;
					closestNode = n;
				}
			}
		}
		return closestNode;
	}

	public Node NodeAt(int x, int y)
	{
		if (x < 0
			|| y < 0
			|| x >= nodes.GetUpperBound(0)
			|| y >= nodes.GetUpperBound(1)) return null;
		return nodes[x, y];
	}
}

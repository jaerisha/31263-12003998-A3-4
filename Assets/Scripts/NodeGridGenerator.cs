using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/* Generating nodes from a tilemap was coded with reference to https://www.youtube.com/watch?v=htZFdfSLiYo */
public class NodeGridGenerator : MonoBehaviour
{
	public Tilemap floorLayer, wallLayer, cornersLayer, startAreaLayer;
	public List<Tilemap> portalLayers;
	public int startScanX, startScanY, scanFinishX, scanFinishY;
	private Node[,] nodes;
	private List<List<Node>> portals;
	private List<Node> corners, startArea;
	public Node topLeft, topRight, bottomLeft, bottomRight;
	public float nodeSize = 1f;
	public float HalfNodeSize => nodeSize / 2f;
	public bool isReady = false;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	private void Awake()
	{
		int gridWidth = scanFinishX - startScanX;
		int gridHeight = scanFinishY - startScanY;
		nodes = new Node[gridWidth, gridHeight];
		portals = new List<List<Node>>();
		corners = new List<Node>();
		startArea = new List<Node>();
		CreateNodes();
	}

	private void CreateNodes()
	{
		int gridX = 0;
		int gridY = 0;	//Determines array size

		for(int x = startScanX; x < scanFinishX; x++)
		{
			for(int y = startScanY; y < scanFinishY; y++)
			{
				Vector3Int position = new Vector3Int(x, y, 0);
				TileBase floorTile = floorLayer.GetTile(position);
				TileBase wallTile = wallLayer.GetTile(position);
				TileBase cornerTile = cornersLayer.GetTile(position);
				TileBase startTile = startAreaLayer.GetTile(position);
				int portalID = GetPortalID(position);
				bool walkable
					= wallTile == null
					&& (floorTile != null || portalID >= 0);	//Just checking to see if this is a wall tile
				Node newNode = new Node(walkable, gridX, gridY, this, portalID);
				nodes[gridX, gridY] = newNode;
				if (portalID >= 0)
				{
					AddToPortals(newNode);
				}
				if (cornerTile != null)
				{
					corners.Add(newNode);
				}
				if (startTile != null)
				{
					startArea.Add(newNode);
				}
				gridY++;
			}
			gridX++;
			gridY = 0;
		}
		topLeft = FindTopLeftCorner();
		topRight = FindTopRightCorner();
		bottomLeft = FindBottomLeftCorner();
		bottomRight = FindBottomRightCorner();

		isReady = true;
	}

	public Node GetRandomStartTile(Node excluding)
	{
		Node n = null;
		while (n == null || n == excluding)
		{
			int randomIndex = Random.Range(0, startArea.Count);
			n = startArea[randomIndex];
		}
		return n;
	}

	private Node FindTopLeftCorner()
	{
		int leftestX = int.MaxValue;
		int highestY = int.MinValue;
		Node expectedNode = null;

		for (int i = 0; i < corners.Count; i++)
		{
			Node n = corners[i];
			int x = n.gridX;
			int y = n.gridY;
			if (x <= leftestX && y >= highestY)
			{
				leftestX = x;
				highestY = y;
				expectedNode = n;
			}
		}

		return expectedNode;
	}

	private Node FindTopRightCorner()
	{
		int rightestX = int.MinValue;
		int highestY = int.MinValue;
		Node expectedNode = null;

		for (int i = 0; i < corners.Count; i++)
		{
			Node n = corners[i];
			int x = n.gridX;
			int y = n.gridY;
			if (x >= rightestX && y >= highestY)
			{
				rightestX = x;
				highestY = y;
				expectedNode = n;
			}
		}

		return expectedNode;
	}

	private Node FindBottomLeftCorner()
	{
		int leftestX = int.MaxValue;
		int lowestY = int.MaxValue;
		Node expectedNode = null;

		for (int i = 0; i < corners.Count; i++)
		{
			Node n = corners[i];
			int x = n.gridX;
			int y = n.gridY;
			if (x <= leftestX && y <= lowestY)
			{
				leftestX = x;
				lowestY = y;
				expectedNode = n;
			}
		}

		return expectedNode;
	}

	private Node FindBottomRightCorner()
	{
		int rightestX = int.MinValue;
		int lowestY = int.MaxValue;
		Node expectedNode = null;

		for (int i = 0; i < corners.Count; i++)
		{
			Node n = corners[i];
			int x = n.gridX;
			int y = n.gridY;
			if (x >= rightestX && y <= lowestY)
			{
				rightestX = x;
				lowestY = y;
				expectedNode = n;
			}
		}

		return expectedNode;
	}

	public Node MatchingPortal(Node n)
	{
		int id = n.portalID;
		if (id < 0 || id >= portals.Count) return null;

		List<Node> matchingPortals = portals[id];
		for (int i = 0; i < matchingPortals.Count; i++)
		{
			if (matchingPortals[i] != n) return matchingPortals[i];
		}
		return null;
	}

	private void AddToPortals(Node n)
	{
		int id = n.portalID;
		if (id < 0 || id >= portalLayers.Count) return;

		while (portals.Count <= id)
		{
			portals.Add(new List<Node>());
		}

		portals[id].Add(n);
	}

	private int GetPortalID(Vector3Int position)
	{
		for (int i = 0; i < portalLayers.Count; i++)
		{
			if (portalLayers[i].GetTile(position)) return i;
		}
		return -1;
	}

	public List<Node> GetNeighbours(Node n) {
		List<Node> neighbours = new List<Node>();

		for(int x = -1; x <= 1; x++){
			for(int y = -1; y <= 1; y++) {
				if (Mathf.Abs(x) == Mathf.Abs(y)) continue;
				Node neighbour = NodeAt(n.gridX + x, n.gridY + y);
				if (neighbour == null) continue;
				neighbours.Add(neighbour);
			}
		}
		Node matchingPortal = MatchingPortal(n);
		if (matchingPortal != null)
		{
			neighbours.Add(matchingPortal);
		}
		return neighbours;
	}

	public Node GetClosestNode(Vector3 position)
	{
		float closestDistance = Mathf.Infinity;
		Node closestNode = null;
		for (int x = 0; x < MapWidth; x++)
		{
			for (int y = 0; y < MapHeight; y++)
			{
				Node n = NodeAt(x, y);
				if (n == null) continue;
				float dist = Vector3.SqrMagnitude(n.WorldPosition - position);
				if (dist < closestDistance)
				{
					closestDistance = dist;
					closestNode = n;
				}

				if (closestDistance <= HalfNodeSize * HalfNodeSize)
				{
					return closestNode;
				}
			}
		}
		return closestNode;
	}

	public int MapWidth => nodes.GetUpperBound(0);

	public int MapHeight => nodes.GetUpperBound(1);

	public Vector3 OffsetTiles()
	{
		return transform.position + new Vector3(startScanX, startScanY, 0f);
	}

	public Node NodeAt(int x, int y)
	{
		if (x < 0
			|| y < 0
			|| x >= MapWidth
			|| y >= MapHeight) return null;
		return nodes[x, y];
	}

	public void DrawPath(List<Node> path)
	{
		if (path == null) return;
		foreach (Node n in path)
		{
			n.Draw(Color.white);
		}
	}
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeGridGenerator : MonoBehaviour
{
	public Grid gridBase;
	public List<Tilemap> obstacleLayers;
	public Tilemap floor, walls;
	public List<Tilemap> portalLayers;
	public GameObject nodePrefab;
	public int startScanX, startScanY, scanFinishX, scanFinishY;
	private Node[,] nodes;
	private List<List<Node>> portals;
	public float nodeSize = 1f;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		obstacleLayers = new List<Tilemap>();
		int gridWidth = scanFinishX - startScanX;
		int gridHeight = scanFinishY - startScanY;
		nodes = new Node[gridWidth, gridHeight];
		portals = new List<List<Node>>();
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
				Vector3Int position = new Vector3Int(x, y, 0);
				TileBase floorTile = floor.GetTile(position);
				TileBase wallTile = walls.GetTile(position);
				int portalID = GetPortalID(position);
				bool walkable
					= wallTile == null
					&& (floorTile != null || portalID >= 0);
				Node newNode = new Node(walkable, gridX, gridY, this, portalID);
				nodes[gridX, gridY] = newNode;
				if (portalID >= 0)
				{
					AddToPortals(newNode);
				}
				gridY++;
			}
			gridX++;
			gridY = 0;
		}
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

				if (closestDistance <= nodeSize * nodeSize)
				{
					return closestNode;
				}
			}
		}
		return closestNode;
	}

	public Vector3 OffsetTiles()
	{
		return transform.position + new Vector3(startScanX, startScanY, 0f);
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

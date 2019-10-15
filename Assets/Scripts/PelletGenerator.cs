using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class PelletGenerator : MonoBehaviour
{
	public Tilemap pelletLayer;
	public NodeGridGenerator nodeMap;
	public GameObject pelletPrefab, ghostPelletPrefab, cherryPrefab;
	public float ghostPelletChance = 0.05f;
	public int maxGhostPellets = 4;
	public float ghostPelletMinDistance = 5f;
	public float cherryChance = 0.05f;
	public int maxCherries = 1;
	public float cherryMinDistance = 10f;
	private List<GameObject> ghostPellets, cherries;
	private bool created;

	private void Awake()
	{
		ghostPellets = new List<GameObject>();
		cherries = new List<GameObject>();
	}

	private void Update()
	{
		if (!nodeMap.isReady) return;
		if (created)
		{
			//no more pellets, you win
			if (transform.childCount == 0)
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
		else
		{
			CreatePellets();
		}
	}

	private void CreatePellets()
	{
		int ghostPelletCount = 0, cherryCount = 0;
		for (int x = 0; x < nodeMap.MapWidth; x++)
		{
			for (int y = 0; y < nodeMap.MapHeight; y++)
			{
				Node n = nodeMap.NodeAt(x, y);
				if (n == null || !n.walkable || n.portalID >= 0) continue;
				Vector3 worldPos = n.WorldPosition;
				Vector3Int position = new Vector3Int(
					(int)worldPos.x,
					(int)worldPos.y,
					0);
				TileBase pelletTile = pelletLayer.GetTile(position);
				if (pelletTile == null) continue;
				GameObject newPellet = null;
				if (ghostPelletCount < maxGhostPellets
					&& !IsNearObjects(ghostPellets, worldPos, ghostPelletMinDistance)
					&& Random.value <= ghostPelletChance)
				{
					newPellet = Instantiate(ghostPelletPrefab);
					ghostPellets.Add(newPellet);
					ghostPelletCount++;
				}
				else if (cherryCount < maxCherries
					&& !IsNearObjects(cherries, worldPos, cherryMinDistance)
					&& Random.value <= cherryChance)
				{
					newPellet = Instantiate(cherryPrefab);
					cherries.Add(newPellet);
					cherryCount++;
				}
				else
				{
					newPellet = Instantiate(pelletPrefab);
				}
				newPellet.transform.position = worldPos;
				newPellet.transform.parent = transform;
			}
		}
		created = true;
	}

	private bool IsNearObjects(List<GameObject> objs, Vector3 position, float minDistance)
	{
		for (int i = 0; i < objs.Count; i++)
		{
			Vector3 obj = objs[i].transform.position;
			float dist = Vector3.Distance(position, obj);
			if (dist < minDistance) return true;
		}
		return false;
	}
}

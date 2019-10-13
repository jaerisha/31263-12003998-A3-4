using UnityEngine;

public class Node
{
	public bool walkable;
	public int gridX, gridY;
	public int gCost, hCost;
	public Node parent;
	public NodeGridGenerator nodeGridGenerator;

	public Node(bool walkable, int gridX, int gridY,
		NodeGridGenerator nodeGridGenerator){
		this.walkable = walkable;
		this.gridX = gridX;
		this.gridY = gridY;
		this.nodeGridGenerator = nodeGridGenerator;
	}

	public int fCost { get { return gCost + fCost; } }

	public Vector3 WorldPosition
	{
		get
		{
			return nodeGridGenerator.transform.position
			   + Vector3.one * nodeGridGenerator.tileOffset
			   + new Vector3(gridX, gridY, 0);
		}
	}

	public override string ToString()
	{
		return $"Position: ({gridX}, {gridY}), Walkable: {walkable}\n" +
			$"World Position: {WorldPosition}";
	}

	public void Draw(Color col)
	{
		float nodeSize = 1f;
		Vector3 bottom = WorldPosition + Vector3.down * nodeSize / 2f;
		Vector3 top = bottom + Vector3.up * nodeSize;
		Vector3 left = WorldPosition + Vector3.left * nodeSize / 2f;
		Vector3 right = left + Vector3.right * nodeSize;
		Debug.DrawLine(bottom, top, col);
		Debug.DrawLine(left, right, col);
	}
}

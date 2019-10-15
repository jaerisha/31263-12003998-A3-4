using System.Collections.Generic;
using UnityEngine;

/* Was made with reference to A* algorithm tutorial by Sebastian Lague, which can be found at
https://www.youtube.com/watch?v=mZfyt03LDH4&list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW&index=3 */
public static class AStar
{
	public static List<Node> FindAPath(NodeGridGenerator nodeMap, Node start, Node end)
	{
		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();

		openSet.Add(start);

		while(openSet.Count > 0)	//while there are still nodes that haven't been checked
		{
			Node node = openSet[0];
			for(int i = 0; i < openSet.Count; i++)
			{
				if(openSet[i].fCost <= node.fCost)
				{
					if(openSet[i].hCost < node.hCost)
					{
						node = openSet[i];
					}
				}
				openSet.Remove(node);
				closedSet.Add(node);

				if(node == end)
				{
					return RetracePath(start, end);
				}

				foreach (Node neighbour in nodeMap.GetNeighbours(node))
				{
					if(!neighbour.walkable
						|| closedSet.Contains(neighbour)) continue;	

					int newCostToNeighbour = node.gCost
						+ GetDistance(nodeMap, node, neighbour);
					if(newCostToNeighbour < neighbour.gCost
						|| !openSet.Contains(neighbour))
					{
						neighbour.gCost = newCostToNeighbour;
						neighbour.hCost = GetDistance(nodeMap, neighbour, end);
						neighbour.parent = node;

						if(!openSet.Contains(neighbour))
						{
							openSet.Add(neighbour);
						}
					}
				}
			}
		}
		return null;
	}

	private static List<Node> RetracePath(Node start, Node end)
	{
		List<Node> path = new List<Node>();
		Node current = end;

		while (current != start)
		{
			path.Add(current);
			current = current.parent;
		}
		path.Reverse();
		return path;
	}

	private static int GetDistance(NodeGridGenerator nodeMap, Node a, Node b)
	{
		if (nodeMap.MatchingPortal(a) == b) return 0;
		return 1;
	}
}

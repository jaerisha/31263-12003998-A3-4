using System.Collections.Generic;
using UnityEngine;

public class FleeFromPacman : GhostMovement
{
	protected override Vector3 GetDirection()
	{
		Node otherPortal = nodeGrid.MatchingPortal(targetNode);
		if (otherPortal != null) return intendedDirection;

		List<Node> neighbours = nodeGrid.GetNeighbours(targetNode);

		float furthestDistanceFromPacman = 0f;
		Node furthestNode = null;
		for (int i = 0; i < neighbours.Count; i++)
		{
			Node n = neighbours[i];
			if (n == null || !n.walkable) continue;
			float distFromPacman = Vector3.SqrMagnitude(
				n.WorldPosition - mrsPacman.targetNode.WorldPosition);
			if (distFromPacman > furthestDistanceFromPacman)
			{
				furthestDistanceFromPacman = distFromPacman;
				furthestNode = n;
			}
		}

		return furthestNode.WorldPosition - targetNode.WorldPosition;
	}
}

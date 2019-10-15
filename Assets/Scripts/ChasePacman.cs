using System.Collections.Generic;
using UnityEngine;

public class ChasePacman : GhostMovement
{
	protected override Vector3 GetDirection()
	{
		if (targetNode == mrsPacman.targetNode) return GetRandomDirection();

		List<Node> path = AStar.FindAPath(
			nodeGrid, targetNode, mrsPacman.targetNode);

		nodeGrid.DrawPath(path);
		if (path != null && path.Count > 0)
		{
			Node nextnode = path[0];
			if (nextnode.portalID >= 0 && path.Count > 2)	//Ensures that ghosts don't get stuck in the portals
			{
				nextnode = path[2];		//Aim for the node after the portal
				return -(nextnode.WorldPosition - targetNode.WorldPosition);
			}
			return nextnode.WorldPosition - targetNode.WorldPosition;
		}
		return intendedDirection;
	}
}

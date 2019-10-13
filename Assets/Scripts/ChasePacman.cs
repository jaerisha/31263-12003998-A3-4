using System.Collections.Generic;
using UnityEngine;

public class ChasePacman : GhostMovement
{
	protected override Vector3 GetDirection()
	{
		List<Node> path = AStar.FindAPath(
			nodeGrid, targetNode, mrsPacman.targetNode);
		DrawPath(path);
		if (path != null && path.Count > 0)
		{
			Node nextnode = path[0];
			if (nextnode.portalID >= 0 && path.Count > 2)
			{
				nextnode = path[2];
				return -(nextnode.WorldPosition - targetNode.WorldPosition);
			}
			return nextnode.WorldPosition - targetNode.WorldPosition;
		}
		return Vector3.zero;
	}

	private void DrawPath(List<Node> path)
	{
		foreach (Node n in path)
		{
			n.Draw(Color.white);
		}
	}
}

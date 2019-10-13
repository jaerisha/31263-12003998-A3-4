using System.Collections.Generic;
using UnityEngine;

public class ReturnToStartMovement : GhostMovement
{
	private Node targetStartPos;
	private List<Node> path;

	protected override Vector3 GetDirection()
	{
		if (targetStartPos == null
			|| WorldPosition == targetStartPos.WorldPosition)
		{
			targetStartPos = nodeGrid.GetRandomStartTile(targetNode);
			path = AStar.FindAPath(nodeGrid, targetNode, targetStartPos);
		}

		if (path != null && path.Count > 0)
		{
			targetNode = path[0];
		}

		if (WorldPosition == targetNode.WorldPosition)
		{
			path.RemoveAt(0);
			if (targetNode.portalID >= 0 && path.Count > 1)
			{
				path.RemoveAt(0);
				transform.position = path[0].WorldPosition;
			}
			return GetDirection();
		}

		if (targetNode.portalID >= 0 && path.Count > 2)
		{

			Node nextnode = path[2];
			return -(nextnode.WorldPosition - WorldPosition);
		}
		return targetNode.WorldPosition - WorldPosition;
	}
}

﻿using System.Collections.Generic;
using UnityEngine;

public class ClockwiseMovement : GhostMovement
{
	private int targetCornerID = 0;
	private List<Node> path;
	private Node targetCorner;

	protected override Vector3 GetDirection()
	{
		if (targetCorner == null
			|| WorldPosition == targetCorner.WorldPosition)
		{
			targetCorner = GetNextTarget();
			path = AStar.FindAPath(nodeGrid, targetNode, targetCorner);
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

	private Node GetNextTarget()
	{
		targetCornerID = (targetCornerID + 1) % 4;

		switch (targetCornerID)
		{
			default: return nodeGrid.topLeft;
			case 0: return nodeGrid.topLeft;
			case 1: return nodeGrid.topRight;
			case 2: return nodeGrid.bottomRight;
			case 3: return nodeGrid.bottomLeft;
		}
	}

	public override void Frighten(bool frighten)
	{
		if (!frighten && frightened)
		{
			targetCorner = null;
		}
		base.Frighten(frighten);
	}
}

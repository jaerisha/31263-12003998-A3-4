using System.Collections.Generic;
using UnityEngine;

public abstract class GhostMovement : Movement
{
	public Movement mrsPacman;
	public FleeFromPacman fleeMovement;
	public bool frightened;
	private bool caught;
	public ReturnToStartMovement startReturnMovement;

	protected Vector3 GetRandomDirection()
	{
		if (WorldPosition != targetNode.WorldPosition) return intendedDirection;
		if (targetNode.portalID >= 0) return intendedDirection;

		List<Node> neighbours = nodeGrid.GetNeighbours(targetNode);
		Node randomNode = null;
		neighbours.Remove(previousNode);
		previousNode.Draw(Color.magenta);
		while ((randomNode == null || !randomNode.walkable)	//If there is no current target to go to
			&& neighbours.Count > 0)
		{
			int randomIndex = Random.Range(0, neighbours.Count);
			randomNode = neighbours[randomIndex];
			neighbours.RemoveAt(randomIndex);
		}
		if (randomNode == null) return intendedDirection;
		return randomNode.WorldPosition - targetNode.WorldPosition;
	}

	public virtual void Frighten(bool frighten)
	{
		if (frighten == frightened) return;

		animationController.SetFrightened(frighten);

		Movement[] movementControllers = GetComponents<Movement>();
		if (fleeMovement != this)
		{
			fleeMovement.enabled = frighten;
		}
		if (frighten)
		{
			fleeMovement.targetNode = targetNode;
		}
		for (int i = 0; i < movementControllers.Length; i++)
		{
			if (fleeMovement == movementControllers[i]) continue;
			movementControllers[i].enabled = !frighten;
			if (!frighten)
			{
				if (caught)
				{
					movementControllers[i].targetNode = startReturnMovement.targetNode;
				}
				else if (frightened)
				{
					movementControllers[i].targetNode = fleeMovement.targetNode;
				}
			}
		}
		frightened = frighten;
		caught = false;
		startReturnMovement.enabled = false;
	}

	public void Catch()
	{
		caught = true;
		fleeMovement.enabled = false;
		startReturnMovement.enabled = true;
	}
}

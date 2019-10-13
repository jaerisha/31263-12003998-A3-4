using System;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
	public AnimationController animationController;
	public NodeGridGenerator nodeGrid;
	public float speed = 1f;
	public Node targetNode;
	protected Node previousNode;
	protected Vector2 intendedDirection;
	protected bool canTravelThroughWalls;
	
	private void Start()
	{
		targetNode = nodeGrid.GetClosestNode(WorldPosition);
		previousNode = targetNode;
		gameObject.transform.position = targetNode.WorldPosition;
	}

	protected virtual void Update()
	{
		Vector2 inputDirection = GetDirection().normalized;
		if (inputDirection != Vector2.zero)
		{
			intendedDirection = inputDirection;
		}
		targetNode.Draw(Color.red);
	}
	
	private void FixedUpdate()
	{
		Move();
	}

	private void Move()
	{
		if (WorldPosition == targetNode.WorldPosition)
		{
			if (intendedDirection == Vector2.zero)
			{
				return;
			}
			else
			{
				SetDirection(intendedDirection);
			}
			Node otherPortal = nodeGrid.MatchingPortal(targetNode);
			if (otherPortal != null)
			{
				previousNode = targetNode;
				targetNode = otherPortal;
				transform.position = targetNode.WorldPosition;
			}
			Node nextTargetNode = CheckNode(intendedDirection);
			if (nextTargetNode == null)
			{
				Stop();
				return;
			}
			else
			{
				if (!canTravelThroughWalls && !nextTargetNode.walkable)
				{
					Stop();
					return;
				}
			}
			nextTargetNode.Draw(Color.green);
			previousNode = targetNode;
			targetNode = nextTargetNode;
		}
		else
		{
			WorldPosition = Vector3.MoveTowards(
				WorldPosition,
				targetNode.WorldPosition,
				speed * Time.deltaTime);
		}
	}

	protected virtual void Stop()
	{
		intendedDirection = Vector2.zero;
	}

	protected abstract Vector3 GetDirection();

	private void SetDirection(Vector2 direction)
	{
		float angle = Vector2.SignedAngle(Vector2.up, direction);
		animationController.SetDirection(angle);
	}

	protected Node CheckNode(Vector2 movement)
	{
		int xMove = Mathf.RoundToInt(movement.x);
		int yMove = Mathf.RoundToInt(movement.y);
		int x = targetNode.gridX + xMove;
		int y = targetNode.gridY + yMove;
		Node nextNode = nodeGrid.NodeAt(x, y);
		if (nextNode != null)
		{
			return nextNode;
		}
		return null;
	}

	protected Vector3 WorldPosition
	{
		get
		{
			return transform.position;
		}
		set
		{
			transform.position = value;
		}
	}
}

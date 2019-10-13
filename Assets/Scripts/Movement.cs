using UnityEngine;

public abstract class Movement : MonoBehaviour
{
	public AnimationController animationController;
	public NodeGridGenerator nodeGrid;
	public float speed = 1f;
	public Node targetNode;
	protected Vector2 intendedDirection;
	
	private void Start()
	{
		targetNode = nodeGrid.GetClosestNode(WorldPosition);
		gameObject.transform.position = targetNode.WorldPosition;
	}

	private void Update()
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
				targetNode = otherPortal;
				transform.position = targetNode.WorldPosition;
			}
			Node nextTargetNode = CheckNode(intendedDirection);
			if (nextTargetNode == null || !nextTargetNode.walkable)
			{
				intendedDirection = Vector2.zero;
				return;
			}
			nextTargetNode.Draw(Color.green);
			targetNode = nextTargetNode;
		}
		else
		{
			Vector3 previousPosition = WorldPosition;
			WorldPosition = Vector3.MoveTowards(
				WorldPosition,
				targetNode.WorldPosition,
				speed * Time.deltaTime);
			Vector3 currentDirection = WorldPosition - previousPosition;
			SetDirection(currentDirection);
		}
	}

	protected abstract Vector3 GetDirection();

	private void SetDirection(Vector2 direction)
	{
		float angle = Vector2.SignedAngle(Vector2.up, direction);
		animationController.SetDirection(angle);
	}

	private Node CheckNode(Vector2 movement)
	{
		int xMove = (int)movement.x;
		int yMove = (int)movement.y;
		int x = targetNode.gridX + xMove;
		int y = targetNode.gridY + yMove;
		Node nextNode = nodeGrid.NodeAt(x, y);
		if (nextNode != null)
		{
			return nextNode;
		}
		return null;
	}

	private Vector3 WorldPosition
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

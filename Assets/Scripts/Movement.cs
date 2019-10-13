using UnityEngine;

public abstract class Movement : MonoBehaviour
{
	public AnimationController animationController;
	public NodeGridGenerator nodeGridGenerator;
	public float speed = 1f;
	private Node targetNode;
	private Vector2 intendedDirection;
	
	private void Start()
	{
		targetNode = nodeGridGenerator.GetClosestNode(WorldPosition);
		Debug.Log($"Current node: {targetNode}");
		gameObject.transform.position = targetNode.WorldPosition;
	}

	private void Update()
	{
		Vector2 inputDirection = GetDirection().normalized;
		if(Mathf.Abs(inputDirection.x) == Mathf.Abs(inputDirection.y))
		{
			inputDirection.y = 0f;
		}
		if (inputDirection != Vector2.zero)
		{
			intendedDirection = inputDirection;
		}
		targetNode.Draw(Color.red);
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		Move();
	}

	private void Move()
	{
		if (WorldPosition == targetNode.WorldPosition)
		{
			SetDirection(intendedDirection);
			if (intendedDirection == Vector2.zero) return;
			Node nextTargetNode = CheckNode(intendedDirection);
			nextTargetNode.Draw(Color.green);
			if (!nextTargetNode.walkable)
			{
				intendedDirection = Vector2.zero;
				return;
			}
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
		Node targetnode = nodeGridGenerator.NodeAt(x, y);
		return targetnode != null ? targetnode : targetNode;
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

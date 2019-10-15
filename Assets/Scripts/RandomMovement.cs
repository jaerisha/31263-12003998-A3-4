using UnityEngine;

public class RandomMovement : GhostMovement
{
	protected override Vector3 GetDirection()
	{
		return GetRandomDirection();
	}
}

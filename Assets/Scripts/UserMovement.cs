using UnityEngine;

public class UserMovement : Movement
{
	protected override Vector3 GetDirection()
	{
		return new Vector2(
			Input.GetAxisRaw("Horizontal"),
			Input.GetAxisRaw("Vertical"));
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("Ouch");
	}
}

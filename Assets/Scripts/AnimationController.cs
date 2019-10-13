using UnityEngine;

public class AnimationController : MonoBehaviour
{
	public Animator animator;

	public void SetDirection(float angle)
	{
		animator.SetFloat("Angle", angle);
	}

	public void SetFrightened(bool frightened)
	{
		animator.SetBool("Frightened", frightened);
	}
}

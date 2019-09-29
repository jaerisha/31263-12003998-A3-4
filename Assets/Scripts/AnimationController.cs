using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;

    public void SetDirection(int direction) {
        animator.SetInteger("Direction", direction);
    }
}

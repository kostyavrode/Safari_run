using UnityEngine;
using System.Collections;

public class AnimatorController : MonoBehaviour
{
    public PlayerController playerController = null;
    private Animator animator = null;

    void Start ()
    {
        animator = this.GetComponent<Animator> ();
    }
    void Update ()
    {
        if ( playerController.isDead )
        {
            animator.SetBool ( "dead", true );
        }
        if ( playerController.jumpStart )
        {
            animator.SetBool ( "jumpStart", true );
        }
        else if ( playerController.isJumping )
        {
            animator.SetBool ( "jump", true );
        }
        else
        {
            animator.SetBool ( "jump", false );
            animator.SetBool ( "jumpStart", false );
        }
    }
}

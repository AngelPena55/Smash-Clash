using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followBehaviour : StateMachineBehaviour
{
    public float followDistance;
    private Transform playerPos;
    public float speed;
    public float lastDirection = 0f;
    bool isMoving = false;
    public AudioClip walking;
    private AudioSource audioSrc;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //--------------------------------------------------------------------------------------------------------
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        audioSrc = animator.GetComponent<AudioSource>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //--------------------------------------------------------------------------------------------------------
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Moves the character towards the player relative to the position
        animator.SetBool("isAttacking", false);
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, playerPos.position, speed * Time.deltaTime);
        
        //If the player is right of the character move to the right
        if (playerPos.position.x > animator.transform.position.x)
        {
            isMoving = true;
            audioSrc.clip = walking;
            lastDirection = 1f;
            animator.transform.localScale = new Vector3(Mathf.Abs(animator.transform.localScale.x), Mathf.Abs(animator.transform.localScale.y), Mathf.Abs(animator.transform.localScale.z));
        }
        //if the player is on the left of the character, move to the left
        else if (playerPos.position.x < animator.transform.position.x)
        {
            isMoving = true;
            audioSrc.clip = walking;
            lastDirection = -1f;
            animator.transform.localScale = new Vector3(Mathf.Abs(animator.transform.localScale.x) * -1f, Mathf.Abs(animator.transform.localScale.y), Mathf.Abs(animator.transform.localScale.z));
        }
        else
        {
            isMoving = false;
        }

        if (isMoving)
        {
            if (!audioSrc.isPlaying)
                audioSrc.PlayOneShot(walking);
        }
        else
            audioSrc.Stop();

        //If player is too far go back to the patrolling behavior
        if (Vector2.Distance(playerPos.position, animator.transform.position) >= followDistance)
        {
            animator.SetBool("IsFollowing", false);
            animator.SetBool("isPatroling", true);

        }

        //If player is close, start attacking the player
        if (Vector2.Distance(playerPos.position, animator.transform.position) <= 3f)
        {           
            animator.GetComponent<EnemyCombat>().SetDirection(lastDirection);
            animator.SetBool("isAttacking", true);
            //call attack function
            
            //if (vector2.distance(playerpos.position, animator.transform.position) <= 3f)
            //{
            //    animator.setbool("isattacking2", true);
            //}
        }

        //If the player is still visible, but is too far, activate the ability
        if (Vector2.Distance(playerPos.position, animator.transform.position) > 3f && followDistance > 3f)
        {
            animator.SetTrigger("Ability");
        }
    }

   
    

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}

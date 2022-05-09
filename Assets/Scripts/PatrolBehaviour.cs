using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : StateMachineBehaviour
{
    public float detectionDistance;
    private Transform playerPos;
    private PatrolSpots patrol;
    public float speed;
    private int randomSpot;
    private float waitTime;
    public float maxWaitTime;
    public float minWaitTime;
    bool isMoving = false;
    public AudioClip walking;
    private AudioSource audioSrc;
    private GameObject parentObject;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //--------------------------------------------------------------------------------------------------------
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        parentObject = animator.GetComponent<EnemyCombat>().parentObj;
        Debug.Log(parentObject.tag);
        if (parentObject.CompareTag("Room") || parentObject.CompareTag("BossRoom")|| parentObject.CompareTag("StoreRoom") || parentObject.CompareTag("StartRoom"))
        {
            patrol = parentObject.GetComponentInChildren<PatrolSpots>();
        }

        else
        {
            patrol = GameObject.FindGameObjectWithTag("PatrolSpots").GetComponent<PatrolSpots>();
        }
            
        randomSpot = Random.Range(0, patrol.patrolPoints.Length);
        waitTime = Random.Range(minWaitTime, maxWaitTime);
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        audioSrc = animator.GetComponent<AudioSource>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //--------------------------------------------------------------------------------------------------------
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if character is far from the patrol spot
        if (Vector2.Distance(animator.transform.position, patrol.patrolPoints[randomSpot].position) > 0.2f)
        {
            isMoving = true;
            audioSrc.clip = walking;
            //move the character towards the patrol spot 
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, patrol.patrolPoints[randomSpot].position, speed * Time.deltaTime);

            //If the patrol spot is right of the character move to the right
            if (patrol.patrolPoints[randomSpot].position.x > animator.transform.position.x)
            {
                animator.transform.localScale = new Vector3(Mathf.Abs(animator.transform.localScale.x), Mathf.Abs(animator.transform.localScale.y), Mathf.Abs(animator.transform.localScale.z));
            }

            //If the patrol spot is left of the character move to the left
            else if (patrol.patrolPoints[randomSpot].position.x < animator.transform.position.x)
            {
                animator.transform.localScale = new Vector3(Mathf.Abs(animator.transform.localScale.x) * -1f, Mathf.Abs(animator.transform.localScale.y), Mathf.Abs(animator.transform.localScale.z));
            }

        }
        else
        {
            isMoving = false;
            //Stay at the patrol spot until waitTime is 0
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, patrol.patrolPoints.Length);
                waitTime = Random.Range(minWaitTime, maxWaitTime);
            }
            else
            {
                
                waitTime -= Time.deltaTime;
            }
        }
        if (isMoving)
        {
            if (!audioSrc.isPlaying)
                audioSrc.PlayOneShot(walking);
        }
        else
            audioSrc.Stop();
        //if player is close to the character then stop patrolling
        if (Vector2.Distance(playerPos.position, animator.transform.position) <= detectionDistance)
        {
            animator.SetBool("isPatroling", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}

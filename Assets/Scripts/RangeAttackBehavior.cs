using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackBehavior : StateMachineBehaviour
{
    private void Awake()
    {
        GameObject Boss1 = GameObject.Find("Demon");
    }


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.CompareTag("Enemy"))
        {
             animator.GetComponent<EnemyCombat>().AbilityActivated();

        }
        if (animator.CompareTag("Boss"))
        {
            animator.GetComponent<BossFIght>().AbilityActivated();
        }

        

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}

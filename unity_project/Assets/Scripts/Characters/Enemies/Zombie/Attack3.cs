using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3 : StateMachineBehaviour
{
    Transform target;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
       float distance = Vector2.Distance(target.position, animator.transform.position);
       if(distance > 4)
            animator.SetBool("isAttack3", false);
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }



}

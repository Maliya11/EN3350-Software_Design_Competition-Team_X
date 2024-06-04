using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idle5 : StateMachineBehaviour
{
    Transform target;
    Transform borderCheck;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       target = GameObject.FindGameObjectWithTag("Player").transform;
       borderCheck = animator.GetComponent<Reaper>().borderCheck;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(Physics2D.Raycast(borderCheck.position, Vector2.down, 2) == false)
          return;

        float distance = Vector2.Distance(target.position, animator.transform.position);

        if(distance < 15)
            animator.SetBool("isChasing5", true);
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

  
  
}

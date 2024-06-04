using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : StateMachineBehaviour
{
  
    Transform target;   //target to attack
    Transform borderCheck;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       target = GameObject.FindGameObjectWithTag("Player").transform;    //find the player
       borderCheck = animator.GetComponent<Golem>().borderCheck;     //find the border check
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Physics2D.Raycast(borderCheck.position, Vector2.down, 2) == false)
          return;

        float distance = Vector2.Distance(target.position, animator.transform.position);
        if(distance < 15)
            animator.SetBool("isChasing", true);            //if the distance is less than 15, the goblin will start chasing the player
        
        
         
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

}

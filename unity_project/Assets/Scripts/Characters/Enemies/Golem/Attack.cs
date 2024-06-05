using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : StateMachineBehaviour
{
   /*
      This script is used to Manage the attack of the Golem
   */

   Transform target; // Target to follow

   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      target = GameObject.FindGameObjectWithTag("Player").transform; // Find the player
   }

   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      // Calculate the distance between the player and the goblin
      float distance = Vector2.Distance(target.position, animator.transform.position);
      
      // If the distance is greater than 4, the goblin will stop attacking
      if(distance > 4)
      {
         animator.SetBool("isAttack", false);
      }
   }

   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      // Nothing to do here   
   }
}
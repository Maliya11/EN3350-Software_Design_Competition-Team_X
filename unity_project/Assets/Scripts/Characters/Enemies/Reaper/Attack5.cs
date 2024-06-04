using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack5 : StateMachineBehaviour
{
   /*
   This script is used to control the attack of the reaper enemy.
   */

   Transform target;

   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      target = GameObject.FindGameObjectWithTag("Player").transform; // Find the player's position
   }

   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      // Check the distance between the reaper enemy and the player
      float distance = Vector2.Distance(target.position, animator.transform.position);

      // If the distance is greater than 4, stop attacking the player
      if(distance > 4)
      {
         animator.SetBool("isAttack5", false);
      }
   }

   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      // Do nothing
   }   
}
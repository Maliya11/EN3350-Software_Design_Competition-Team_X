using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3 : StateMachineBehaviour
{
   /*
   This script is responsible for the attack3 state of the zombie enemy.
   */

   Transform target;

   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      target = GameObject.FindGameObjectWithTag("Player").transform; // Find the player object
   }


   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      // Calculate the distance between the player and the zombie
      float distance = Vector2.Distance(target.position, animator.transform.position);
      
      // If the distance is greater than 4, the zombie will stop attacking
      if(distance > 4)
      {
         animator.SetBool("isAttack3", false);
      }
   }

   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      // Do nothing
   }
}
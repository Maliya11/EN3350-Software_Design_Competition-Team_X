using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idle5 : StateMachineBehaviour
{
   /*
   This script is responsible for the idle state of the Reaper enemy.
   */

   Transform target;
   Transform borderCheck;

   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      target = GameObject.FindGameObjectWithTag("Player").transform; // Find the player's position
      borderCheck = animator.GetComponent<Reaper>().borderCheck; // Find the border check position
   }


   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      // If the reaper enemy is at the edge of the platform, stop chasing the player
      if(Physics2D.Raycast(borderCheck.position, Vector2.down, 2) == false) return;

      // Check the distance between the reaper enemy and the player
      float distance = Vector2.Distance(target.position, animator.transform.position);

      // If the distance is less than 15, start chasing the player
      if(distance < 15)
      {
         animator.SetBool("isChasing5", true);
      }
   }

   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      // Do nothing
   } 
}
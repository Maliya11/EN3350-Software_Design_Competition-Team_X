using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idle2 : StateMachineBehaviour
{
   /*
   This script is responsible for the goblin's idle state.
   The goblin will be in this state when it is not chasing the player.
   The goblin will start chasing the player if the player is within a certain distance.
   */
   
   Transform target; // Target to follow
   Transform borderCheck; // Border check

   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      target = GameObject.FindGameObjectWithTag("Player").transform;  // Find the player
      borderCheck = animator.GetComponent<Goblin>().borderCheck;  // Find the border check
   }

   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      // If the goblin is not on the ground
      if(Physics2D.Raycast(borderCheck.position, Vector2.down, 2) == false) return;

      // Calculate the distance between the player and the goblin
      float distance = Vector2.Distance(target.position, animator.transform.position);

      // If the distance is less than 15, the goblin will start chasing the player
      if(distance < 15)
         animator.SetBool("isChasing2", true);
   }

   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      // Do nothing
   }
}
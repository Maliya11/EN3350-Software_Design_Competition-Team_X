using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2 : StateMachineBehaviour
{
   /*
   This script is responsible for the goblin's attack2 state.
   The goblin will attack the player if the player is within a certain distance.
   If the player is not within a certain distance, the goblin will stop attacking.
   */

   Transform target; // Target to attack

   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      target = GameObject.FindGameObjectWithTag("Player").transform; // Find the player
   }

   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      float distance = Vector2.Distance(target.position, animator.transform.position);

      // If the distance is greater than 4, the goblin will stop attacking
      if(distance > 4) animator.SetBool("isAttack2", false); 
   }

   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      // Nothing to do here
   }
}

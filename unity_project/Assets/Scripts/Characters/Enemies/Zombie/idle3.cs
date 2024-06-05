using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idle3 : StateMachineBehaviour
{
   /*
   This script is attached to the Zombie prefab.
   This script is responsible for the idle3 state of the zombie enemy.
   */

   Transform target;
   Transform borderCheck;

   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      target = GameObject.FindGameObjectWithTag("Player").transform; // Find the player object
      borderCheck = animator.GetComponent<Zombie>().borderCheck; // Find the borderCheck object
   }

   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      // If the borderCheck object is not touching the ground, return
      if(Physics2D.Raycast(borderCheck.position, Vector2.down, 2) == false) return;

      // Calculate the distance between the player and the enemy
      float distance = Vector2.Distance(target.position, animator.transform.position);
      
      // If the distance is less than 15, the zombie will start chasing the player
      if(distance < 15)
      {
         animator.SetBool("isChasing3", true);
      }
   }

   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      // Do nothing  
   }
}
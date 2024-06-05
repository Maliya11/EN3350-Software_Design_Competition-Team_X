using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack4 : StateMachineBehaviour
{
  /*
  This script is responsible for the attack4 state of the Zombie1 enemy.
  */

  Transform target;

  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    target = GameObject.FindGameObjectWithTag("Player").transform; // Find the player
  }
  
  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    // Calculate the distance between the player and the enemy
    float distance = Vector2.Distance(target.position, animator.transform.position);
    
    // If the distance is greater than 4, the enemy will stop attacking
    if(distance > 4)
    {
      animator.SetBool("isAttack4", false);
    }
  }

  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    // Do nothing
  }
}
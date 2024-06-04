using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk1 : StateMachineBehaviour
{
  /*
  This script is responsible for the Minotaur's walk state. The Minotaur will walk towards the player and stop when the player is within a certain distance.
  */

  // Variables
  Transform target;
  public float speed = 3;
  Transform borderCheck;
  public Animator animator;

  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    target = GameObject.FindGameObjectWithTag("Player").transform; // Find the player
    borderCheck = animator.GetComponent<Minotaur>().borderCheck; // Find the border check object
  }

  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    // Move the Minotaur towards the player
    Vector2 newPos = new Vector2(target.position.x, animator.transform.position.y);
    animator.transform.position = Vector2.MoveTowards(animator.transform.position, newPos, speed*Time.deltaTime);
    if(Physics2D.Raycast(borderCheck.position, Vector2.down, 2) == false)
    {
      animator.SetBool("isChasing1", false);
    }
    
    // Check if the player is within a certain distance
    float distance = Vector2.Distance(target.position, animator.transform.position);

    // If the player is within a certain distance, set the Minotaur's attack animation to true
    if(distance < 3)
    {
      animator.SetBool("isAttack1", true);
    }
  }

  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    // Do nothing
  }
}
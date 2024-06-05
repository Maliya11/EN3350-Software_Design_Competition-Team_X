using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk2 : StateMachineBehaviour
{
  /*
  This script is responsible for the goblin's movement when it is chasing the player.
  The goblin will move towards the player's position.
  */

  // Target to follow
  Transform target;
  public float speed = 3;
  Transform borderCheck;
  public Animator animator;

  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    // Find the player
    target = GameObject.FindGameObjectWithTag("Player").transform;

    // Find the border check
    borderCheck = animator.GetComponent<Goblin>().borderCheck;
  }

  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    // Move the goblin towards the player's position
    Vector2 newPos = new Vector2(target.position.x, animator.transform.position.y);
    animator.transform.position = Vector2.MoveTowards(animator.transform.position, newPos, speed*Time.deltaTime);
    
    // If the goblin is not on the ground
    if(Physics2D.Raycast(borderCheck.position, Vector2.down, 2) == false)
    {
      // Stop chasing the player
      animator.SetBool("isChasing2", false);
    }
    
    // Calculate the distance between the player and the goblin
    float distance = Vector2.Distance(target.position, animator.transform.position);

    // If the distance is less than 3, the goblin will start attacking the player
    if(distance < 3)
    {
      // animator.SetBool("isChasing", false);
      animator.SetBool("isAttack2", true);               
    }
  }

  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    // Do nothing
  }
}

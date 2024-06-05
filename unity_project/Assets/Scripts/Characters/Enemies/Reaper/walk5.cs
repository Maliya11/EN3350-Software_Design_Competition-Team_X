using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walk5 : StateMachineBehaviour
{
  /*
  This script is used to control the movement of the reaper enemy.
  */

  Transform target; // The player's position
  public float speed = 3; // The speed of the reaper enemy
  Transform borderCheck; // The position of the border check
  public Animator animator; // The animator of the reaper enemy

  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    target = GameObject.FindGameObjectWithTag("Player").transform; // Find the player's position
    borderCheck = animator.GetComponent<Reaper>().borderCheck; // Find the border check position
  }


  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    // Move the reaper enemy towards the player
    Vector2 newPos = new Vector2(target.position.x, animator.transform.position.y);
    animator.transform.position = Vector2.MoveTowards(animator.transform.position, newPos, speed*Time.deltaTime);
    
    // If the reaper enemy is at the edge of the platform, stop chasing the player
    if(Physics2D.Raycast(borderCheck.position, Vector2.down, 2) == false)
    {
      animator.SetBool("isChasing5", false);
    }
    
    // Check the distance between the reaper enemy and the player
    float distance = Vector2.Distance(target.position, animator.transform.position);

    // If the distance is less than 3, attack the player
    if(distance < 3)
    {
      // animator.SetBool("isChasing", false);
      animator.SetBool("isAttack5", true);
    }
  }

  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    // Do nothing
  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk3 : StateMachineBehaviour
{
  /*
  This script is used to control the movement of the zombie3 enemy.
  */

  Transform target; // The player's position
  public float speed = 3; // The speed of the zombie3 enemy
  Transform borderCheck; // The position of the border check
  public Animator animator; // The animator of the zombie3 enemy

  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    target = GameObject.FindGameObjectWithTag("Player").transform; // Find the player's position
    borderCheck = animator.GetComponent<Zombie>().borderCheck; // Find the border check position
  }


  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    // Move the zombie3 enemy towards the player
    Vector2 newPos = new Vector2(target.position.x, animator.transform.position.y);
    animator.transform.position = Vector2.MoveTowards(animator.transform.position, newPos, speed*Time.deltaTime);
    
    // If the zombie3 enemy is at the edge of the platform, stop chasing the player
    if(Physics2D.Raycast(borderCheck.position, Vector2.down, 2) == false)
    {
      animator.SetBool("isChasing3", false);
    }
    
    // Check the distance between the zombie3 enemy and the player
    float distance = Vector2.Distance(target.position, animator.transform.position);

    // If the distance is less than 3, attack the player
    if(distance < 3)
    {
      animator.SetBool("isAttack3", true);
    }
  }
  
  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    // Do nothing    
  }
}
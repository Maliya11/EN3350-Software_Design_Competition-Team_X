using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk2 : StateMachineBehaviour
{
    Transform target;
    public float speed = 3;
    Transform borderCheck;
    public Animator animator;
 
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       target = GameObject.FindGameObjectWithTag("Player").transform;
        borderCheck = animator.GetComponent<Goblin>().borderCheck;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       Vector2 newPos = new Vector2(target.position.x, animator.transform.position.y);
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, newPos, speed*Time.deltaTime);
        if(Physics2D.Raycast(borderCheck.position, Vector2.down, 2) == false)
          animator.SetBool("isChasing2", false);                                    //if the goblin is not on the ground
        
        
        float distance = Vector2.Distance(target.position, animator.transform.position);

        if(distance < 3)
            // animator.SetBool("isChasing", false);
            animator.SetBool("isAttack2", true);                //if the distance is less than 3, the goblin will start attacking the player
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }


}

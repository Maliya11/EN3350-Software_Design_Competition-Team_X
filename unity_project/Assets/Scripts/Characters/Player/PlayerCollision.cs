using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    /*
    This is script is used to give damage/death to the player after it has been hit by an enemy
    */
    public Animator animator;
    private int playerLayer;
    private int enemyLayer;

    private void Awake()
    {
        //get the layermask of the player and enemie
        playerLayer = LayerMask.NameToLayer("player");
        enemyLayer = LayerMask.NameToLayer("enemy");
    }

    public void PlayerTakeDamage()
    {
        //reduce the health by 1 if player gets hit by an enemy
        HealthManager.health--;
        if(HealthManager.health <= 0)
        {
            StartCoroutine(Dead());  //if health is 0 or less player is dead
        }
        else
        {
            StartCoroutine(GetHurt());  //else player gets hurt
        }   
    }

    IEnumerator Dead()
    {
        if(animator == null) yield break;

        if(gameObject.activeSelf)
        {
            //performes the player's death animations
            animator.SetTrigger("isDead");
            animator.SetTrigger("dead");
            //plays the death sound effect for ninja or robot
            if(PlayerManager.isNinja)
            {
                AudioManagerPlayer.instance.Play("NinjaDeath");
            }
            else
            {
                AudioManagerPlayer.instance.Play("RobotDeath");
            }
            
            yield return new WaitForSeconds(2); //small wait for finish playing the animations

            animator.SetTrigger("backToIdle");  //set the player back to idle
            PlayerManager.isPlayerDead = true;  //set player state to dead
        }

        else
        {
            Debug.LogWarning("Cannot start coroutine: GameObject is inactive.");
        }
    }

    IEnumerator GetHurt()
    {
        if(animator == null) yield break;

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);  //ignores layer collision for some seconds
        GetComponent<Animator>().SetLayerWeight(1, 1);   //blinking animation starts

        yield return new WaitForSeconds(3);   //waiting to blink

        GetComponent<Animator>().SetLayerWeight(1, 0);   //blinking animation stops
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);  //again enabling player to collide with enemies
    }
}


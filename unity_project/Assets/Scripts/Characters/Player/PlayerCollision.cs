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
        // Get the layermask of the player and enemie
        playerLayer = LayerMask.NameToLayer("player");
        enemyLayer = LayerMask.NameToLayer("enemy");
    }

    public void PlayerTakeDamage()
    {
        // Reduce the health by 1 if player gets hit by an enemy
        HealthManager.health--;
        if(HealthManager.health <= 0)
        {
            // If health is 0 or less player is dead
            StartCoroutine(Dead());  
        }
        else
        {
            // Else player gets hurt
            StartCoroutine(GetHurt());  
        }   
    }

    private IEnumerator Dead()
    {
        if(animator == null) yield break;

        if(gameObject.activeSelf)
        {
            // Performes the player's death animations
            animator.SetTrigger("isDead");
            animator.SetTrigger("dead");

            // Plays the death sound effect for ninja or robot
            if(PlayerManager.isNinja)
            {
                AudioManagerPlayer.instance.Play("NinjaDeath");
            }
            else
            {
                AudioManagerPlayer.instance.Play("RobotDeath");
            }
            
            
            // Wait for finishing playing the animations
            yield return new WaitForSeconds(1); 

            PlayerManager.isPlayerDead = true;  // Set player state to dead
            animator.SetTrigger("backToIdle");  // Set the player back to idle
        }

        else
        {
            Debug.LogWarning("Cannot start coroutine: GameObject is inactive.");
        }
    }

    private IEnumerator GetHurt()
    {
        if(animator == null) yield break;

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);  // Ignores layer collision for some seconds
        GetComponent<Animator>().SetLayerWeight(1, 1);   // Blinking animation starts

        yield return new WaitForSeconds(3);   // waiting to blink

        GetComponent<Animator>().SetLayerWeight(1, 0);   // Blinking animation stops
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);  // Again enabling player to collide with enemies
    }
}


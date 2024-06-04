using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    /*
    This script is attached to the Zombie prefab. 
    It handles the zombie's health, damage, and death. 
    It also finds the player and ignores collisions with the player.    
    */

    Transform target; // Target to follow
    public Transform borderCheck; // Border check to stop the zombie from falling off the platform
    public int ZombieHP = 100; // Zombie's health
    public Animator animator; // Animator component
    PlayerManager playerManager; // PlayerManager instance
    
    // Start is called before the first frame update
    private void Start()
    {
        FindTarget(); // Call to find the target
        IgnoreCollisions(); // Call to ignore collisions with the player
    
        // Find the PlayerManager instance
        playerManager = FindObjectOfType<PlayerManager>();
        if (playerManager == null)
        {
            Debug.LogError("PlayerManager not found in the scene.");
        }

        // Ensure the animator is assigned
        if (animator == null)
        {
            // Find the animator component
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found on the Zombie.");
            }
        }
    }

    private void FindTarget()
    {
        // Find the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Set the player as the target
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found. Zombie cannot detect player position.");
        }
    }

    private void IgnoreCollisions()
    {
        // Ignore collisions between the zombie and the player
        Collider2D zombieCollider = GetComponent<Collider2D>();
        if(target != null)
        {
            // Get the player's collider
            Collider2D playerCollider = target.GetComponent<Collider2D>();
            if (zombieCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, zombieCollider);
            }
            else
            {
                Debug.LogWarning("Zombie or player collider not found. Ignoring collision failed.");
            }
        }
    }

    private void Update()
    {
        if (target != null)
        {
            // Update zombie's scale based on player position
            transform.localScale = new Vector2(target.position.x > transform.position.x ? 1.5f : -1.5f, 1.5f);
        }
    }

    public void ZombieTakeDamage(int damage)
    {
        // Damage the zombie
        ZombieHP -= damage;
        // Play the hurt sound
        AudioEnemy.instance.Play("Hurt");
        if (ZombieHP <= 0)
        {
            Die();
        }
        else
        {
            if(animator != null)
            {
                animator.SetTrigger("Damage3");
            }
        }
    }

    private void Die()
    {
        if (animator != null)
        {
            // Play the death animation
            animator.SetTrigger("deth3");
            StartCoroutine(DisableGameObject());
        }
        if (playerManager != null)
        {
            // Add points to the player's score
            playerManager.AddPoints(10);
            playerManager.enemyKills++;
        }
        else
        {
            Debug.LogWarning("PlayerManager not found. Points not added.");
        }

        // Disable the collider to prevent further damage
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        else
        {
            Debug.LogWarning("Collider2D not found on the Zombie.");
        }   
    }

    private IEnumerator DisableGameObject()
    {
        // Wait for the death animation to finish
        yield return new WaitForSeconds(2.0f); // Adjust the wait time if needed
        this.enabled = false;
    }

    public void PlayerDamage()
    {
        if (target != null)
        {
            // Damage the player
            PlayerCollision playerCollision = target.GetComponent<PlayerCollision>();
            if (playerCollision != null && HealthManager.health > 0)
            {
                // Play the punch sound
                AudioEnemy.instance.Play("Punch");
                playerCollision.PlayerTakeDamage();
            }
        }
        else
        {
            Debug.LogWarning("Player not found. Cannot damage player.");
        }
    }
}

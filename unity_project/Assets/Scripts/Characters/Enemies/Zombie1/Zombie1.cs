using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie1 : MonoBehaviour
{
    /*
    * The Zombie1 class is responsible for managing the Zombie1 enemy.
    * It handles the Zombie1's health, damage, and death.
    */
    
    Transform target;   // Target to follow
    public Transform borderCheck;  // Border check to stop the goblin from falling off the platform
    public int Zombie1HP = 100;  // Zombie1's health
    public Animator animator;   // Animator component
    PlayerManager playerManager;  // PlayerManager instance

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
            // Get the animator component
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found on the Zombie.");
            }
        }
    }

    // Method to find the target
    private void FindTarget()
    {
        // Find the player object
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Get the player's transform
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found. Zombie1 cannot detect player position.");
        }
    }

    // Method to ignore collisions with the player
    private void IgnoreCollisions()
    {
        // Get the colliders
        Collider2D zombie1Collider = GetComponent<Collider2D>();
        if(target != null)
        {
            Collider2D playerCollider = target.GetComponent<Collider2D>();
            if (zombie1Collider != null && playerCollider != null)
            {
                // Ignore collision between the player and the Zombie1
                Physics2D.IgnoreCollision(playerCollider, zombie1Collider);
            }
            else
            {
                Debug.LogWarning("Zombie1 or player collider not found. Ignoring collision failed.");
            }
        }
    }

    private void Update()
    {
        if (target != null)
        {
            // Update golem's scale based on player position
            transform.localScale = new Vector2(target.position.x > transform.position.x ? 1.5f : -1.5f, 1.5f);
        }
    }

    // Method to take damage
    public void Zombie1TakeDamage(int damage)
    {
        // Reduce the Zombie1's health
        Zombie1HP -= damage;
        // Play the hurt sound
        AudioEnemy.instance.Play("Hurt");
        if (Zombie1HP <= 0)
        {
            Die();
        }
        else
        {
            if(animator != null)
            {
                animator.SetTrigger("Damage4");
            }
        }
    }

    private void Die()
    {
        if (animator != null)
        {
            // Play the death animation
            animator.SetTrigger("deth4");
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

        // Disable the collider
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        else
        {
            Debug.LogWarning("Collider2D not found on the Zombie1.");
        }
    }

    // Method to disable the game object
    private IEnumerator DisableGameObject()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2.0f); // Adjust the wait time if needed
        //gameObject.SetActive(false);
        this.enabled = false;
    }

    // Method to damage the player
    public void PlayerDamage()
    {
        if (target != null)
        {
            // Get the PlayerCollision component
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

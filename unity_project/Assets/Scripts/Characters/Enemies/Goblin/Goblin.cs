using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    /*
    This script is attached to the Goblin prefab. It handles the goblin's health, damage, and death.
    The goblin will take damage when hit by the player's weapon.
    The goblin will die when its health reaches 0.
    The goblin will damage the player if the player collides with it.
    */

    // Variables
    Transform target; // Target to follow
    public Transform borderCheck; // Border check to stop the goblin from falling off the platform
    public int GoblinHP = 100; // Goblin's health
    public Animator animator; // Animator component
    PlayerManager playerManager; // PlayerManager instance

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
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found on the Goblin.");
            }
        }
    }

    // Method to find the player
    private void FindTarget()
    {
        // Find the player object with the "Player" tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Set the player as the target to follow
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found. Goblin cannot detect player position.");
        }
    }

    // Method to ignore collisions between the goblin and the player
    private void IgnoreCollisions()
    {
        // Get the goblin's collider
        Collider2D goblinCollider = GetComponent<Collider2D>();

        if(target != null)
        {
            // Get the player's collider
            Collider2D playerCollider = target.GetComponent<Collider2D>();
            if (goblinCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, goblinCollider);
            }
            else
            {
                Debug.LogWarning("Goblin or player collider not found. Ignoring collision failed.");
            }
        }
    }

    private void Update()
    {
        if (target != null)
        {
            // Update goblin's scale based on player position
            transform.localScale = new Vector2(target.position.x > transform.position.x ? 1.5f : -1.5f, 1.5f);
        }
    }

    public void GoblinTakeDamage(int damage)
    {
        // Reduce the goblin's health by the damage amount
        GoblinHP -= damage;

        // Play the hurt sound
        AudioEnemy.instance.Play("Hurt");
        if (GoblinHP <= 0)
        {
            Die();  // Call the Die function if the goblin's health is less than or equal to 0
        }
        else
        {
            if(animator != null)
            {
                animator.SetTrigger("Damage2");
            }
        }
    }

    // Die function to handle the goblin's death
    private void Die()
    {
        if(animator != null)
        {
            // Play the death animation
            animator.SetTrigger("deth2");
            StartCoroutine(DisableGameObject());
        }
        if(playerManager != null)
        {
            // Add points to the player's score and increment the enemyKills count
            playerManager.AddPoints(10);
            playerManager.enemyKills++;
        }
        else
        {
            Debug.LogWarning("PlayerManager not found. Points not added.");
        }

        // Get the goblin's collider
        Collider2D collider = GetComponent<Collider2D>();

        if (collider != null)
        {
            // Disable the collider to prevent further collisions
            collider.enabled = false;
        }
        else
        {
            Debug.LogWarning("Collider2D not found on the Goblin.");
        }
    }

    // Coroutine to disable the game object after a delay
    private IEnumerator DisableGameObject()
    {
        yield return new WaitForSeconds(2.0f); // Adjust the wait time if needed
        //gameObject.SetActive(false);
        this.enabled = false;
    }

    // Function to damage the player
    public void PlayerDamage()   
    {
        if (target != null)
        {
            // Get the PlayerCollision component from the player
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

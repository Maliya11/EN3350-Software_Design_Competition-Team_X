using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper : MonoBehaviour
{
    /*
    * The Reaper class is responsible for managing the Reaper enemy.
    * It handles the Reaper's health, damage, and death.
    */

    Transform target; // Target to follow
    public Transform borderCheck; // Border check to stop the Reaper from falling off the platform
    public int GolemHP = 100; // Reaper's health
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
            // Get the animator component
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found on the Reaper.");
            }
        }
    }

    // Update is called once per frame
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
            Debug.LogWarning("Player not found. Reaper cannot detect player position.");
        }
    }

    // Method to ignore collisions with the player
    private void IgnoreCollisions()
    {
        // Ignore collisions between the Reaper and the player
        Collider2D ReaperCollider = GetComponent<Collider2D>();
        if(target != null)
        {
            // Get the colliders
            Collider2D playerCollider = target.GetComponent<Collider2D>();
            if (ReaperCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, ReaperCollider);
            }
            else
            {
                Debug.LogWarning("Reaper or player collider not found. Ignoring collision failed.");
            }
        }
    }


    private void Update()
    {
        if (target != null)
        {
            // Update Reaper's scale based on player position
            transform.localScale = new Vector2(target.position.x > transform.position.x ? 1.5f : -1.5f, 1.5f);
        }
    }

    // Method to take damage
    public void ReaperTakeDamage(int damage)
    {
        // Reduce the Reaper's health
        GolemHP -= damage;
        // Play the hurt sound
        AudioEnemy.instance.Play("Hurt");
        if (GolemHP <= 0)
        {
            Die();
        }
        else
        {
            if(animator != null)
            {
                animator.SetTrigger("Damage5");
            }
        }
    }

    private void Die()
    {
        if (animator != null)
        {
            // Play the death animation
            animator.SetTrigger("deth5");
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
            Debug.LogWarning("Collider2D not found on the Reaper.");
        }
    }

    // Coroutine to disable the game object
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

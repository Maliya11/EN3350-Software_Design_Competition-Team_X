using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur : MonoBehaviour
{
    /*
    This script is attached to the Minotaur prefab.
    This script is responsible for the Minotaur's health, damage, and death.
    The Minotaur will take damage from the player and die when its health reaches 0.
    The Minotaur will also damage the player when it attacks.
    The Minotaur will also ignore collisions with the player.
    */
    
    Transform target; // Target to follow
    public Transform borderCheck; // Border check to stop the Minotaur from falling off the platform
    public int MinotaurHP = 100; // Minotaur's health
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
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found on the Minotaur.");
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
            Debug.LogWarning("Player not found. Minotaur cannot detect player position.");
        }
    }

    // Method to ignore collisions between the Minotaur and the player
    private void IgnoreCollisions()
    {
        // Get the Minotaur and player colliders
        Collider2D minotaurCollider = GetComponent<Collider2D>();
        if(target != null)
        {
            // Ignore collisions between the Minotaur and the player
            Collider2D playerCollider = target.GetComponent<Collider2D>();
            if (minotaurCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, minotaurCollider);
            }
            else
            {
                Debug.LogWarning("Minotaur or player collider not found. Ignoring collision failed.");
            }
        }
    }

    private void Update()
    {
        if (target != null)
        {
            // Update minotuar's scale based on player position
            transform.localScale = new Vector2(target.position.x > transform.position.x ? 1.5f : -1.5f, 1.5f);
        }
    }

    public void MinotaurTakeDamage(int damage)
    {
        // Minotaur takes damage
        MinotaurHP -= damage;
        // Play the Minotaur hurt sound
        AudioEnemy.instance.Play("Hurt");
        if (MinotaurHP <= 0)
        {
            Die();
        }
        else
        {
            if(animator != null)
            {
                animator.SetTrigger("Damage1");
            }
        }
    }

    private void Die()
    {
        if(animator != null)
        {
            // Play the Minotaur death animation
            animator.SetTrigger("deth1");
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

        // Disable the Minotaur's collider
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        else
        {
            Debug.LogWarning("Collider2D not found on the Minotaur.");
        }   
    }

    private IEnumerator DisableGameObject()
    {
        // Wait for the death animation to finish
        yield return new WaitForSeconds(1.0f); // Adjust the wait time if needed
        //gameObject.SetActive(false);
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
                // Play the Minotaur attack sound
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

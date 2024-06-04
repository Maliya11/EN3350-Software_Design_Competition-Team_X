using System.Collections;
using UnityEngine;

public class Golem : MonoBehaviour
{
    /*
    This script is attached to the Golem prefab. It is responsible for the Golem's health, damage, and death. 
    The Golem will take damage when hit by the player and will die when its health reaches zero. 
    The Golem will also damage the player when the player collides with it. 
    The Golem will also follow the player and change its scale based on the player's position.
    */
    
    // Variables
    Transform target; // Target to follow
    public Transform borderCheck; // Border check to stop the golem from falling off the platform
    public int GolemHP = 100; // Golem's health
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
                Debug.LogError("Animator component not found on the Golem.");
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
            Debug.LogWarning("Player not found. Golem cannot detect player position.");
        }
    }

    // Method to ignore collisions between the golem and the player
    private void IgnoreCollisions()
    {
        // Get the collider of the golem
        Collider2D golemCollider = GetComponent<Collider2D>();
        if(target != null)
        {
            // Get the collider of the player
            Collider2D playerCollider = target.GetComponent<Collider2D>();
            if (golemCollider != null && playerCollider != null)
            {
                // Ignore collisions between the golem and the player
                Physics2D.IgnoreCollision(playerCollider, golemCollider);
            }
            else
            {
                Debug.LogWarning("Golem or player collider not found. Ignoring collision failed.");
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (target != null)
        {
            // Update golem's scale based on player position
            transform.localScale = new Vector2(target.position.x > transform.position.x ? 1.5f : -1.5f, 1.5f);
        }
    }

    // Damage the golem
    public void GolemTakeDamage(int damage)
    {
        // Reduce the golem's health by the damage amount
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
                animator.SetTrigger("Damage");
            }
        }
    }

    private void Die()
    {
        if(animator != null)
        {
            // Play the death animation
            animator.SetTrigger("deth");
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
            Debug.LogWarning("Collider2D not found on the Zombie.");
        }     
    }

    private IEnumerator DisableGameObject()
    {
        // Wait for 2 seconds before disabling the game object
        yield return new WaitForSeconds(2.0f); // Adjust the wait time if needed
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

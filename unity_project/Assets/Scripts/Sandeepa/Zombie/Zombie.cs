using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    Transform target;
    public Transform borderCheck;
    public int ZombieHP = 100;
    public Animator animator;
    PlayerManager playerManager;
    // Start is called before the first frame update
    void Start()
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
                Debug.LogError("Animator component not found on the Zombie.");
            }
        }
    }

    void FindTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found. Zombie cannot detect player position.");
        }
    }

    void IgnoreCollisions()
    {
        Collider2D zombieCollider = GetComponent<Collider2D>();
        if(target != null)
        {
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

    void Update()
    {
        if (target != null)
        {
            // Update zombie's scale based on player position
            transform.localScale = new Vector2(target.position.x > transform.position.x ? 1.5f : -1.5f, 1.5f);
        }
    }

    public void ZombieTakeDamage(int damage)
    {
        ZombieHP -= damage;
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

    void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("deth3");
        }
        if (playerManager != null)
        {
            playerManager.AddPoints(10);
        }
        else
        {
            Debug.LogWarning("PlayerManager not found. Points not added.");
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        else
        {
            Debug.LogWarning("Collider2D not found on the Zombie.");
        }

        this.enabled = false; // Disable the Zombie script
        // Optionally, disable the entire game object after some delay to allow death animation to play
        StartCoroutine(DisableGameObject());
    }

    private IEnumerator DisableGameObject()
    {
        yield return new WaitForSeconds(1.0f); // Adjust the wait time if needed
        gameObject.SetActive(false);
    }

    public void PlayerDamage()
    {
        if (target != null)
        {
            PlayerCollision playerCollision = target.GetComponent<PlayerCollision>();
            if (playerCollision != null && HealthManager.health > 0)
            {
                playerCollision.PlayerTakeDamage();
            }
        }
        else
        {
            Debug.LogWarning("Player not found. Cannot damage player.");
        }
    }
}

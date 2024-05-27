using System.Collections;
using UnityEngine;

public class Golem : MonoBehaviour
{
    Transform target;
    public Transform borderCheck;
    public int GolemHP = 100;
    public Animator animator;
    PlayerManager playerManager;

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
                Debug.LogError("Animator component not found on the Golem.");
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
            Debug.LogWarning("Player not found. Golem cannot detect player position.");
        }
    }

    void IgnoreCollisions()
    {
        Collider2D golemCollider = GetComponent<Collider2D>();
        if(target != null)
        {
            Collider2D playerCollider = target.GetComponent<Collider2D>();
            if (golemCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, golemCollider);
            }
            else
            {
                Debug.LogWarning("Golem or player collider not found. Ignoring collision failed.");
            }
        }
    }

    void Update()
    {
        if (target != null)
        {
            // Update golem's scale based on player position
            transform.localScale = new Vector2(target.position.x > transform.position.x ? 1.5f : -1.5f, 1.5f);
        }
    }

    public void GolemTakeDamage(int damage)
    {
        GolemHP -= damage;
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

    void Die()
    {
        if(animator != null)
        {
            animator.SetTrigger("deth");
            StartCoroutine(DisableGameObject());
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
    }

    private IEnumerator DisableGameObject()
    {
        yield return new WaitForSeconds(2.0f); // Adjust the wait time if needed
        //gameObject.SetActive(false);
        this.enabled = false;
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

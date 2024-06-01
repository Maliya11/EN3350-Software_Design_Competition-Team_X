using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerMovement playerMovement;
    PlayerControls controls;
    public Animator animator;
    public int meleeDamage = 25;
    public float meleeAttackRange = 1.0f;
    public LayerMask enemyLayer;

    public void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Land.Attack.performed += OnMeleePerformed;
    }

    private void OnDisable()
    {
        controls.Land.Attack.performed -= OnMeleePerformed;
        controls.Disable();
    }

    private void OnMeleePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Melee();
        if(PlayerManager.isNinja)
        {
            AudioManagerPlayer.instance.Play("NinjaSword");
        }
        else
        {
            AudioManagerPlayer.instance.Play("RobotSword");
        }
    }

    void Melee()
    {
        if(animator == null) return;

        if(!playerMovement.isGrounded)
            animator.SetTrigger("jumpAttack");

        else
            animator.SetTrigger("attack");
    }   

    public void dealMeleeDamage()
    {
        if(enemyLayer == 0) return;
        // Detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, meleeAttackRange, enemyLayer);

        // Apply damage to each enemy hit
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Golem"))
            {
                Golem golem = enemy.GetComponent<Golem>();
                if(golem != null)
                {
                    golem.GolemTakeDamage(meleeDamage);
                }
            }

            if (enemy.CompareTag("Goblin"))
            {
                Goblin goblin = enemy.GetComponent<Goblin>();
                if(goblin != null)
                {
                    goblin.GoblinTakeDamage(meleeDamage);
                }
            }

            if (enemy.CompareTag("Minotaur"))
            {
                Minotaur minotaur = enemy.GetComponent<Minotaur>();
                if(minotaur != null)
                {
                    minotaur.MinotaurTakeDamage(meleeDamage);
                }
            }

            if (enemy.CompareTag("Zombie"))
            {
                Zombie zombie = enemy.GetComponent<Zombie>();
                if(zombie != null)
                {
                    zombie.ZombieTakeDamage(meleeDamage);
                }
            }

            if (enemy.CompareTag("Zombie1"))
            {
                Zombie1 zombie1 = enemy.GetComponent<Zombie1>();
                if(zombie1 != null)
                {
                    zombie1.Zombie1TakeDamage(meleeDamage);
                }
            }

            if (enemy.CompareTag("Bat"))
            {
                Bat bat = enemy.GetComponent<Bat>();
                if(bat != null)
                {
                    bat.BatTakeDamage(meleeDamage);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);
    }
}

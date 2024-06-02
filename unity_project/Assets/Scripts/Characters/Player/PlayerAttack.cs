using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    /*
    This script is used to control the melee attack of the player and identify to what enemy the attack is performed
    */
    private PlayerMovement playerMovement;
    PlayerControls controls;
    public Animator animator;
    public LayerMask enemyLayer;
    public int meleeDamage = 25;  //melee attack damage of players
    public float meleeAttackRange = 1.0f;  //melee attack range of players

    public void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();  //get PlayerMovement component
        controls = new PlayerControls();  //making PlayerControls instance
    }

    private void OnEnable()
    {
        //if attack is performed
        controls.Enable();
        controls.Land.Attack.performed += OnMeleePerformed;
    }

    private void OnDisable()
    {
        //if attack is not performed
        controls.Land.Attack.performed -= OnMeleePerformed;
        controls.Disable();
    }

    private void OnMeleePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Melee();
        //play the sword attack sound effect after checking the player is a robot or a ninja
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
        //change the attack animation by checking the player is grounded or not
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
            //In each code block checks the tag of the object that is in range to the attack, and give damage to that object
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
        //used to visalize the attack range of the player
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);
    }
}

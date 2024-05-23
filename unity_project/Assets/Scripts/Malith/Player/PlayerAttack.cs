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
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);
    }
}

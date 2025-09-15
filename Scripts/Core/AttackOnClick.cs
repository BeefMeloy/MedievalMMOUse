using UnityEngine;

public class AttackOnClick : MonoBehaviour
{
    [Header("Combat Settings")]
    public Animator animator;
    public string attackTrigger = "Attack";
    public float attackRange = 2.5f;
    public float attackCooldown = 1.5f;
    public int baseDamage = 25;
    public int maxDamage = 35;

    [Header("Audio")]
    public AudioClip[] attackSounds;

    private Camera playerCamera;
    private float lastAttackTime;
    private GameObject currentTarget;
    private TargetEnemy targetEnemy;
    private AudioSource audioSource;

    void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
            playerCamera = FindFirstObjectByType<Camera>(); // Fixed: Updated deprecated method

        // Find TargetEnemy component in the scene
        targetEnemy = FindFirstObjectByType<TargetEnemy>(); // Fixed: Updated deprecated method
        audioSource = GetComponent<AudioSource>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleAttackInput();
        HandleAutoAttack();
    }

    void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            // Get the currently selected target from TargetEnemy system
            if (targetEnemy != null && targetEnemy.Selected != null)
            {
                GameObject target = targetEnemy.Selected.gameObject;
                Health enemyHealth = target.GetComponent<Health>();

                if (enemyHealth != null && !enemyHealth.IsDead) // Fixed: Property access not method call
                {
                    AttemptAttack(target);
                }
            }
        }
    }

    void HandleAutoAttack()
    {
        // Auto-attack if we have a target and cooldown is ready
        if (currentTarget != null && Time.time >= lastAttackTime + attackCooldown)
        {
            Health targetHealth = currentTarget.GetComponent<Health>();

            // Clear target if it's dead
            if (targetHealth == null || targetHealth.IsDead) // Fixed: Property access not method call
            {
                ClearTarget();
                return;
            }

            if (IsTargetInRange(currentTarget))
            {
                PerformAttack(currentTarget);
            }
            else
            {
                // Move towards target (integrate with your CharacterMotor)
                MoveTowardsTarget(currentTarget);
            }
        }
    }

    void AttemptAttack(GameObject target)
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        currentTarget = target;

        if (IsTargetInRange(target))
        {
            PerformAttack(target);
        }
        else
        {
            // Move towards target (integrate with your CharacterMotor)
            MoveTowardsTarget(target);
        }
    }

    void PerformAttack(GameObject target)
    {
        if (target == null) return;

        Health enemyHealth = target.GetComponent<Health>();
        if (enemyHealth == null || enemyHealth.IsDead) return; // Fixed: Property access not method call

        // Trigger attack animation
        if (animator != null)
        {
            animator.SetTrigger(attackTrigger);
        }

        // Play attack sound
        PlayAttackSound();

        // Calculate damage
        int damage = Random.Range(baseDamage, maxDamage + 1);

        // Deal damage
        enemyHealth.TakeDamage(damage, gameObject);

        // Visual feedback
        ShowDamageNumber(target, damage);

        lastAttackTime = Time.time;

        // Check if target died
        if (enemyHealth.IsDead) // Fixed: Property access not method call
        {
            ClearTarget();
        }
    }

    bool IsTargetInRange(GameObject target)
    {
        if (target == null) return false;
        float distance = Vector3.Distance(transform.position, target.transform.position);
        return distance <= attackRange;
    }

    void MoveTowardsTarget(GameObject target)
    {
        // This should integrate with your CharacterMotor script
        // For now, just face the target
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void PlayAttackSound()
    {
        if (audioSource != null && attackSounds.Length > 0)
        {
            AudioClip clip = attackSounds[Random.Range(0, attackSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    void ShowDamageNumber(GameObject target, int damage)
    {
        // Create floating damage text
        GameObject damageText = new GameObject("DamageText");
        damageText.transform.position = target.transform.position + Vector3.up * 2f;

        // Add DamageNumber component
        DamageNumber damageNumber = damageText.AddComponent<DamageNumber>();
        damageNumber.Initialize(damage.ToString(), Color.red);
    }

    public void ClearTarget()
    {
        currentTarget = null;
    }

    public bool HasTarget()
    {
        return currentTarget != null;
    }

    public GameObject GetCurrentTarget()
    {
        return currentTarget;
    }

    // Public method to set target from external systems
    public void SetTarget(GameObject target)
    {
        currentTarget = target;
    }
}
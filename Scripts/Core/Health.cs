<<<<<<< HEAD
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public bool regenerateHealth = false;
    public float regenRate = 1f;
    public float regenDelay = 5f;

    [Header("Death Settings")]
    public bool destroyOnDeath = false;
    public float destroyDelay = 5f;
    public GameObject deathEffect;

    [Header("Events")]
    public UnityEvent<int, int> OnHealthChanged;
    public UnityEvent<GameObject> OnDeath;
    public UnityEvent<int, GameObject> OnDamageTaken;

    private int currentHealth;
    private bool isDead = false;
    private float lastDamageTime;
    private Animator animator;

    // Properties for easy access
    public int Current => currentHealth;
    public int Max => maxHealth;
    public float Normalized => maxHealth > 0 ? (float)currentHealth / maxHealth : 0f;
    public bool IsDead => isDead;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Update()
    {
        if (!isDead && regenerateHealth && currentHealth < maxHealth)
        {
            if (Time.time >= lastDamageTime + regenDelay)
            {
                RegenerateHealth();
            }
        }
    }

    public void TakeDamage(int damage, GameObject attacker = null)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        lastDamageTime = Time.time;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnDamageTaken?.Invoke(damage, attacker);

        // Trigger hit animation
        if (animator != null && !isDead)
        {
            animator.SetTrigger("Hit");
        }

        if (currentHealth <= 0 && !isDead)
        {
            Die(attacker);
        }
    }

    // Alternative method name for compatibility
    public void Damage(int amount)
    {
        TakeDamage(amount, null);
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void RegenerateHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += Mathf.RoundToInt(regenRate);
            currentHealth = Mathf.Min(maxHealth, currentHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }

    void Die(GameObject killer = null)
    {
        isDead = true;

        // Trigger death animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Spawn death effect
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, transform.rotation);
        }

        OnDeath?.Invoke(killer);

        // Disable components
        DisableOnDeath();

        if (destroyOnDeath)
        {
            Destroy(gameObject, destroyDelay);
        }
    }

    void DisableOnDeath()
    {
        // Disable colliders
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // Disable movement scripts
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this && script.GetType() != typeof(Animator))
            {
                script.enabled = false;
            }
        }
    }

    public void ResetToFull()
    {
        isDead = false;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        // Re-enable colliders
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }
    }

    // Legacy method names for compatibility
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealthPercentage()
    {
        return Normalized;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
=======
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public bool regenerateHealth = false;
    public float regenRate = 1f;
    public float regenDelay = 5f;

    [Header("Death Settings")]
    public bool destroyOnDeath = false;
    public float destroyDelay = 5f;
    public GameObject deathEffect;

    [Header("Events")]
    public UnityEvent<int, int> OnHealthChanged;
    public UnityEvent<GameObject> OnDeath;
    public UnityEvent<int, GameObject> OnDamageTaken;

    private int currentHealth;
    private bool isDead = false;
    private float lastDamageTime;
    private Animator animator;

    // Properties for easy access
    public int Current => currentHealth;
    public int Max => maxHealth;
    public float Normalized => maxHealth > 0 ? (float)currentHealth / maxHealth : 0f;
    public bool IsDead => isDead;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Update()
    {
        if (!isDead && regenerateHealth && currentHealth < maxHealth)
        {
            if (Time.time >= lastDamageTime + regenDelay)
            {
                RegenerateHealth();
            }
        }
    }

    public void TakeDamage(int damage, GameObject attacker = null)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        lastDamageTime = Time.time;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnDamageTaken?.Invoke(damage, attacker);

        // Trigger hit animation
        if (animator != null && !isDead)
        {
            animator.SetTrigger("Hit");
        }

        if (currentHealth <= 0 && !isDead)
        {
            Die(attacker);
        }
    }

    // Alternative method name for compatibility
    public void Damage(int amount)
    {
        TakeDamage(amount, null);
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void RegenerateHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += Mathf.RoundToInt(regenRate);
            currentHealth = Mathf.Min(maxHealth, currentHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }

    void Die(GameObject killer = null)
    {
        isDead = true;

        // Trigger death animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Spawn death effect
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, transform.rotation);
        }

        OnDeath?.Invoke(killer);

        // Disable components
        DisableOnDeath();

        if (destroyOnDeath)
        {
            Destroy(gameObject, destroyDelay);
        }
    }

    void DisableOnDeath()
    {
        // Disable colliders
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // Disable movement scripts
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this && script.GetType() != typeof(Animator))
            {
                script.enabled = false;
            }
        }
    }

    public void ResetToFull()
    {
        isDead = false;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        // Re-enable colliders
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }
    }

    // Legacy method names for compatibility
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealthPercentage()
    {
        return Normalized;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
>>>>>>> 80f95843f249f1039bc541cb63c6e1bbbb3f5fac
}
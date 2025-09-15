using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }

    [Header("References")]
    public TargetEnemy targetEnemy;

    // Added missing Selected property that references TargetEnemy's Selected
    public Highlightable Selected => targetEnemy != null ? targetEnemy.Selected : null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (targetEnemy == null)
            targetEnemy = FindFirstObjectByType<TargetEnemy>(); // Fixed: Updated deprecated method
    }

    // Wrapper methods to work with your existing TargetEnemy
    public GameObject GetCurrentTarget()
    {
        return targetEnemy != null ? targetEnemy.GetCurrentTarget() : null;
    }

    public bool HasTarget()
    {
        return GetCurrentTarget() != null;
    }

    public Highlightable GetSelectedHighlightable()
    {
        return targetEnemy != null ? targetEnemy.Selected : null;
    }
}
using UnityEngine;

public class TargetEnemy : MonoBehaviour  // Changed from TargetManager to TargetEnemy
{
    [Header("Targeting")]
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mask = ~0;
    [SerializeField] private Highlightable hovered;
    [SerializeField] private Highlightable selected;

    [Header("Combat Integration")]
    [SerializeField] private AttackOnClick playerAttack;
    [SerializeField] private bool autoTargetOnAttack = true;
    [SerializeField] private LayerMask enemyLayers = -1;

    // Public read-only access to the currently selected enemy
    public Highlightable Selected => selected;

    private void Start()
    {
        // Auto-find player attack component if not assigned
        if (!playerAttack)
            playerAttack = FindFirstObjectByType<AttackOnClick>(); // Fixed: Updated deprecated method
    }

    public void SetCamera(Camera c) => cam = c;

    private void Update()
    {
        EnsureCamera();
        if (!cam) return;

        UpdateHover();
        HandleClick();
        HandleCombatInput();
    }

    private void EnsureCamera()
    {
        if (!cam)
            cam = Camera.main ? Camera.main : FindFirstObjectByType<Camera>(); // Fixed: Updated deprecated method
    }

    private void UpdateHover()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 1000f, mask))
        {
            var newHovered = hit.collider.GetComponentInParent<Highlightable>()
                           ?? hit.collider.GetComponentInChildren<Highlightable>();

            if (newHovered != hovered)
            {
                if (hovered && hovered != selected) hovered.SetHighlighted(false);
                hovered = newHovered;
                if (hovered && hovered != selected) hovered.SetHighlighted(true);
            }
        }
        else
        {
            if (hovered && hovered != selected) hovered.SetHighlighted(false);
            hovered = null;
        }
    }

    private void HandleClick()
    {
        if (Input.GetMouseButtonDown(0) && hovered)
        {
            if (selected && selected != hovered) selected.SetHighlighted(false);
            selected = hovered;
            selected.SetHighlighted(true);
        }
    }

    private void HandleCombatInput()
    {
        // Handle right-click for manual targeting
        if (Input.GetMouseButtonDown(1))
        {
            RightClickTarget();
        }

        // Handle Tab targeting
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TabTarget();
        }
    }

    private void RightClickTarget()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 1000f, enemyLayers))
        {
            var newTarget = hit.collider.GetComponentInParent<Highlightable>()
                           ?? hit.collider.GetComponentInChildren<Highlightable>();

            if (newTarget)
            {
                if (selected && selected != newTarget) selected.SetHighlighted(false);
                selected = newTarget;
                selected.SetHighlighted(true);

                // Notify combat system of new target
                if (playerAttack && autoTargetOnAttack)
                {
                    playerAttack.SetTarget(selected.gameObject);
                }
            }
        }
    }

    private void TabTarget()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy)
        {
            var highlightable = nearestEnemy.GetComponent<Highlightable>();
            if (highlightable)
            {
                if (selected) selected.SetHighlighted(false);
                selected = highlightable;
                selected.SetHighlighted(true);

                if (playerAttack)
                {
                    playerAttack.SetTarget(selected.gameObject);
                }
            }
        }
    }

    private GameObject FindNearestEnemy()
    {
        if (!playerAttack) return null;

        Collider[] enemies = Physics.OverlapSphere(playerAttack.transform.position, 20f, enemyLayers);
        GameObject nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider enemy in enemies)
        {
            Health health = enemy.GetComponent<Health>();
            if (health && !health.IsDead) // Fixed: Property access not method call
            {
                float distance = Vector3.Distance(playerAttack.transform.position, enemy.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = enemy.gameObject;
                }
            }
        }

        return nearest;
    }

    // Public method for external systems to get current target
    public GameObject GetCurrentTarget()
    {
        return selected ? selected.gameObject : null;
    }
}
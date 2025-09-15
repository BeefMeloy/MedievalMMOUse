using UnityEngine;

public class NPCBatchSetup : MonoBehaviour
{
    [Header("Batch Assignment")]
    public EnemyType defaultBasicType;
    public EnemyType defaultEliteType;
    public EnemyType defaultBossType;

    [ContextMenu("Setup All NPCs in Scene")]
    public void SetupAllNPCs()
    {
        // FIXED: Use FindObjectsByType instead of FindObjectsOfType
        var allTargetables = FindObjectsByType<Targetable>(FindObjectsSortMode.None);

        foreach (var targetable in allTargetables)
        {
            SetupNPC(targetable);
        }

        Debug.Log($"Configured {allTargetables.Length} NPCs");
    }

    private void SetupNPC(Targetable targetable)
    {
        string name = targetable.gameObject.name.ToLower();

        // Auto-assign based on name patterns
        if (name.Contains("boss") || name.Contains("king") || name.Contains("lord"))
        {
            targetable.entityType = defaultBossType;
            targetable.level = Random.Range(12, 20);
        }
        else if (name.Contains("elite") || name.Contains("captain") || name.Contains("champion"))
        {
            targetable.entityType = defaultEliteType;
            targetable.level = Random.Range(6, 12);
        }
        else
        {
            targetable.entityType = defaultBasicType;
            targetable.level = Random.Range(1, 8);
        }
    }
}
using System.Collections;
using UnityEngine;

[AddComponentMenu("Gameplay/NPC Death (Simple)")]
public class NpcDeath : MonoBehaviour
{
    [SerializeField] private float respawnDelay = 60f;
    [SerializeField] private Transform respawnPoint; // optional override

    private Health health;
    private Vector3 spawnPos;
    private Quaternion spawnRot;
    private Collider[] colliders;
    private Renderer[] renderers;
    private bool processing;

    private void Awake()
    {
        health = GetComponent<Health>();
        spawnPos = transform.position;
        spawnRot = transform.rotation;
        colliders = GetComponentsInChildren<Collider>(true);
        renderers = GetComponentsInChildren<Renderer>(true);
    }

    public void Die()
    {
        if (processing || health == null || !health.IsDead) return;
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        processing = true;
        SetVisible(false);

        yield return new WaitForSeconds(respawnDelay);

        var pos = respawnPoint ? respawnPoint.position : spawnPos;
        var rot = respawnPoint ? respawnPoint.rotation : spawnRot;

        transform.SetPositionAndRotation(pos, rot);
        health.ResetToFull();
        SetVisible(true);

        processing = false;
    }

    private void SetVisible(bool v)
    {
        if (colliders != null) foreach (var c in colliders) if (c) c.enabled = v;
        if (renderers != null) foreach (var r in renderers) if (r) r.enabled = v;
    }
}

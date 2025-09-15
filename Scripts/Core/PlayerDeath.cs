using System.Collections;
using UnityEngine;

[AddComponentMenu("Gameplay/Player Death (Simple)")]
public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private float respawnDelay = 2f;
    [SerializeField] private Animator animator;      // optional
    [SerializeField] private string deathTrigger = "Die";
    [SerializeField] private Transform respawnPoint; // optional override

    private Health health;
    private Vector3 spawnPos;
    private Quaternion spawnRot;
    private bool processing;

    private void Awake()
    {
        health = GetComponent<Health>();
        spawnPos = transform.position;
        spawnRot = transform.rotation;
    }

    public void Die()
    {
        if (processing || health == null || !health.IsDead) return;
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        processing = true;

        if (animator && !string.IsNullOrEmpty(deathTrigger))
            animator.SetTrigger(deathTrigger);

        yield return new WaitForSeconds(respawnDelay);

        var pos = respawnPoint ? respawnPoint.position : spawnPos;
        var rot = respawnPoint ? respawnPoint.rotation : spawnRot;

        transform.SetPositionAndRotation(pos, rot);
        health.ResetToFull();

        if (animator) { animator.Rebind(); animator.Update(0f); }
        processing = false;
    }
}

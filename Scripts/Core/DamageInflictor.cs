using UnityEngine;

[AddComponentMenu("Gameplay/Damage Inflictor (Simple)")]
public class DamageInflictor : MonoBehaviour
{
    /// <summary>
    /// Deal 'amount' damage to the target if it has Health.
    /// Returns true if this hit reduced Health to 0.
    /// </summary>
    public bool Inflict(GameObject target, int amount)
    {
        if (target == null || amount <= 0) return false;

        var hp = target.GetComponent<Health>();
        if (hp == null || hp.IsDead) return false;

        hp.Damage(amount);
        return hp.IsDead;
    }
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TeamAssociation))]
public class AttackBehaviour : MonoBehaviour
{
    [HideInInspector]
    public DamageInterface CurrentEnemy;

    float LastAttackAt = 0.0f;
    public float AttackRate = 0.8f;

    public void Attack()
    {
        if (CurrentEnemy == null)
        {
            return;
        }

        if (Vector3.Distance(transform.position, CurrentEnemy.transform.position) < 7.5f)
        {
            if ((Time.time - LastAttackAt) > AttackRate)
            {
                LastAttackAt = Time.time;
                CurrentEnemy.ApplyDamage(20 + Random.Range(1, 6));
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public enum AIState
{
    Idle,
    Attacking
}

[RequireComponent(typeof(TeamAssociation))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    NavMeshAgent NavMeshAgent;
    TeamAssociation TeamAssociation;

    AIState CurrentState = AIState.Idle;
 
    #region Lanes
    Lane AssociatedLane;
    int CurrentNodeID = 0;
    #endregion

    #region Attacks
    DamageInterface CurrentEnemy;
    float LastAttackAt = 0.0f;
    public float AttackRate = 0.8f;
    #endregion


    public void Start()
    {
        TeamAssociation = GetComponent<TeamAssociation>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Init(Lane associatedLane)
    {
        AssociatedLane = associatedLane;
    }

    void ProgressToNextNode()
    {
        CurrentNodeID++;
        if (CurrentNodeID < AssociatedLane.Nodes.Length)
        {
            NavMeshAgent.SetDestination(AssociatedLane.Nodes[CurrentNodeID]);
        }
    }

    bool EngageInCombat()
    {
        RaycastHit[] allHits = Physics.SphereCastAll(transform.position, 3f, transform.forward, 3f, 1 << 8);
        foreach (RaycastHit hit in allHits)
        {
            TeamAssociation teamAssociation = hit.transform.GetComponent<TeamAssociation>();
            if (teamAssociation)
            {
                if (teamAssociation.IsLeftTeam != TeamAssociation.IsLeftTeam)
                {
                    // is enemy
                    CurrentEnemy = hit.transform.GetComponent<DamageInterface>();
                    return true;
                }
            }
        }
        return false;
    }

	void Update () {
        if (AssociatedLane == null)
        {
            return;
        }

        switch (CurrentState)
        {
            case AIState.Attacking:
                if (CurrentEnemy == null)
                {
                    if (!EngageInCombat())
                    {
                        CurrentState = AIState.Idle;
                    }
                }
                else
                {
                    if (NavMeshAgent.remainingDistance < 1.0f)
                    {
                        NavMeshAgent.SetDestination(CurrentEnemy.transform.position);
                    }

                    if (Vector3.Distance(transform.position, CurrentEnemy.transform.position) < 3.0f)
                    {
                        if ((Time.time - LastAttackAt) > AttackRate)
                        {
                            LastAttackAt = Time.time;
                            CurrentEnemy.ApplyDamage(20 + Random.Range(1, 6));
                        }
                    }
                }
                break;
            case AIState.Idle:
                if (EngageInCombat())
                {
                    CurrentState = AIState.Attacking;
                    NavMeshAgent.SetDestination(CurrentEnemy.transform.position);
                    break;
                }

                if (NavMeshAgent.remainingDistance < 1.0f)
                {
                    ProgressToNextNode();
                }
                break;
        }
	}
}

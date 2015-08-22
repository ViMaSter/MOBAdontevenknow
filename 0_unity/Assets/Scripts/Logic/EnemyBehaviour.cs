using UnityEngine;
using System.Collections;

public enum AIState
{
    Idle,
    Attacking
}

[RequireComponent(typeof(AttackBehaviour))]
[RequireComponent(typeof(TeamAssociation))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    bool IsAttackingHero = false;
    AttackBehaviour AttackBehaviour;
    TeamAssociation TeamAssociation;
    NavMeshAgent NavMeshAgent;

    AIState CurrentState = AIState.Idle;
 
    #region Lanes
    Lane AssociatedLane;
    int CurrentNodeID = 0;
    #endregion


    public void Start()
    {
        AttackBehaviour = GetComponent<AttackBehaviour>();
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

    bool EngageInCombat(bool withHeroes)
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 3f, transform.forward, 3f, withHeroes ? 1 << 10 : 1 << 8);
        foreach (RaycastHit hit in hits)
        {
            TeamAssociation teamAssociation = hit.transform.GetComponent<TeamAssociation>();
            if (teamAssociation)
            {
                if (teamAssociation.IsLeftTeam != TeamAssociation.IsLeftTeam)
                {
                    if (hit.transform.GetComponent<DamageInterface>())
                    {
                        AttackBehaviour.CurrentEnemy = hit.transform.GetComponent<DamageInterface>();
                        IsAttackingHero = withHeroes;
                        return true;
                    }
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
                if (AttackBehaviour.CurrentEnemy == null)
                {
                    if (!EngageInCombat(false) && !EngageInCombat(true))
                    {
                        CurrentState = AIState.Idle;
                    }
                }
                else
                {
                    // If a minion is attacking a hero, constantly search for
                    // minions to attack with higher priority
                    if (IsAttackingHero)
                    {
                        EngageInCombat(false);
                    }

                    if (NavMeshAgent.remainingDistance < 1.0f || (Vector3.Distance(AttackBehaviour.CurrentEnemy.transform.position, NavMeshAgent.destination) > 1.0f))
                    {
                        NavMeshAgent.SetDestination(AttackBehaviour.CurrentEnemy.transform.position);
                    }
                    AttackBehaviour.Attack();
                }
                break;
            case AIState.Idle:
                if (EngageInCombat(false) || EngageInCombat(true))
                {
                    CurrentState = AIState.Attacking;
                    NavMeshAgent.SetDestination(AttackBehaviour.CurrentEnemy.transform.position);
                    break;
                }

                if (NavMeshAgent.remainingDistance < 3.0f)
                {
                    ProgressToNextNode();
                }
                break;
        }
	}
}

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
public class MinionBehaviour : MonoBehaviour
{
    Map.EntityTypes CurrentlyAttackedType = Map.EntityTypes.NONE;
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
            NavMeshAgent.Resume();
        }
    }

    bool EngageInCombat(Map.EntityTypes entityType)
    {
        int layerMask = 0;
        switch (entityType)
        {
            case Map.EntityTypes.Minion:
                layerMask = 8;
                break;
            case Map.EntityTypes.Hero:
                layerMask = 10;
                break;
            case Map.EntityTypes.Building:
                layerMask = 11;
                break;
            default:
                Debug.LogWarningFormat(this, "Trying to engage into combat with {0} - no case for that available!", entityType);
                return false;
        }

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 3f, transform.forward, 3f, 1 << layerMask);
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
                        CurrentlyAttackedType = entityType;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // @TODO: Rewrite this to utilize a sphere collider instead of
    //        recasting a Ray over and over again
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
                    if (!EngageInCombat(Map.EntityTypes.Minion) && !EngageInCombat(Map.EntityTypes.Minion) && !EngageInCombat(Map.EntityTypes.Building))
                    {
                        CurrentState = AIState.Idle;
                    }
                }
                else
                {
                    // If a minion is attacking a hero, constantly search for
                    // minions to attack with higher priority
                    if (CurrentlyAttackedType == Map.EntityTypes.Hero)
                    {
                        if (!EngageInCombat(Map.EntityTypes.Minion))
                        {
                            EngageInCombat(Map.EntityTypes.Building);
                        }
                    }

                    float DistanceFromEnemyToNavDestination = Vector3.Distance(AttackBehaviour.CurrentEnemy.transform.position, NavMeshAgent.destination);

                    AttackBehaviour.Attack();

                    if (DistanceFromEnemyToNavDestination > 0.2f)
                    {
                        NavMeshAgent.SetDestination(AttackBehaviour.CurrentEnemy.transform.position);
                        NavMeshAgent.Resume();
                    }
                    if (NavMeshAgent.remainingDistance < 2.0f)
                    {
                        NavMeshAgent.Stop();
                        return;
                    }
                }
                break;
            case AIState.Idle:
                if (EngageInCombat(Map.EntityTypes.Minion) || EngageInCombat(Map.EntityTypes.Hero) || EngageInCombat(Map.EntityTypes.Building))
                {
                    CurrentState = AIState.Attacking;
                    NavMeshAgent.SetDestination(AttackBehaviour.CurrentEnemy.transform.position);
                    NavMeshAgent.Resume();
                    break;
                }

                if (NavMeshAgent.remainingDistance < 3.0f && !NavMeshAgent.pathPending)
                {
                    ProgressToNextNode();
                }
                break;
        }
	}
}

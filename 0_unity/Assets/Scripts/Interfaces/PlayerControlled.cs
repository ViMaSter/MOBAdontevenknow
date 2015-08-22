using UnityEngine;
using System.Collections;

public class PlayerControlled : MonoBehaviour
{
    NavMeshAgent NavMeshAgent;
    AttackBehaviour AttackBehaviour;

	void Start () {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        AttackBehaviour = GetComponent<AttackBehaviour>();
    }
    void GoToRoutine()
    {
        if (NavMeshAgent == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 200.0f, ~(1 << 8)))
            {
                AttackBehaviour.CurrentEnemy = null;
                NavMeshAgent.SetDestination(hit.point);
                NavMeshAgent.Resume();
                Particles.MousePointer(hit.point, true);
            }
            else
            {
                Particles.MousePointer(hit.point, false);
            }
        }
    }
    void AttackOverwriteRoutine()
    {
        if (NavMeshAgent == null || AttackBehaviour == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 200.0f, 1 << 8))
            {
                if (hit.transform.GetComponent<DamageInterface>())
                {
                    NavMeshAgent.SetDestination(hit.transform.position);
                    NavMeshAgent.Resume();
                    AttackBehaviour.CurrentEnemy = hit.transform.GetComponent<DamageInterface>();
                    Particles.MousePointer(hit.point, false);
                }
            }
        }
    }

    void AttackBehaviourRoutine()
    {
        if (NavMeshAgent == null || AttackBehaviour == null)
        {
            return;
        }

        if (AttackBehaviour.CurrentEnemy)
        {
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
    }
	    
	void Update () {
        StartCoroutine("GoToRoutine");
        StartCoroutine("AttackOverwriteRoutine");
        StartCoroutine("AttackBehaviourRoutine");
    }
}

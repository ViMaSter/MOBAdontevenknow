using UnityEngine;
using System.Collections;

public class PlayerControlled : MonoBehaviour
{
    NavMeshAgent NavMeshAgent;

	void Start () {
        NavMeshAgent = GetComponent<NavMeshAgent>();
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
                NavMeshAgent.ResetPath();
                NavMeshAgent.SetDestination(hit.point);
                Particles.MousePointer(hit.point, true);
            }
            else
            {
                Particles.MousePointer(hit.point, false);
            }
        }
    }
	    
	void Update () {
        StartCoroutine("GoToRoutine");
    }
}

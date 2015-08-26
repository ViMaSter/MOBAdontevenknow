using UnityEngine;
using System.Collections;

public class PlayerAnimationProxy : MonoBehaviour
{
    public Animator Animator;
    public NavMeshAgent NavMeshAgent;
    public AttackBehaviour AttackBehaviour;

	void Update () {
        foreach (AnimatorControllerParameter parameter in Animator.parameters)
        {
            switch (parameter.name)
            {
                case "currentMovementSpeed":
                    Animator.SetFloat(parameter.nameHash, NavMeshAgent.velocity.magnitude);
                    break;
                case "isAttacking":
                    Animator.SetBool(parameter.nameHash, AttackBehaviour.CurrentEnemy != null);
                    break;
            }
        }
	}
}

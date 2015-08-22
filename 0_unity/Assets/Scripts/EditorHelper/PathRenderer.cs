using UnityEngine;
using System.Collections;

public class PathRenderer : MonoBehaviour
{
	void OnDrawGizmosSelected ()
    {
        Transform[] childTransforms = GetComponentsInChildren<Transform>();

        for (int i = 0; i < childTransforms.Length - 1; i++)
        {
            if (childTransforms[i] == transform)
            {
                continue;
            }

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(childTransforms[i].position, childTransforms[i + 1].position);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(childTransforms[i + 1].position, 0.5f);
        }
	}
}

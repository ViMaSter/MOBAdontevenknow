using UnityEngine;
using System.Collections.Generic;

public class LaneRenderer : MonoBehaviour
{
    void OnDrawGizmosSelected()
    {
        Vector3 Root = -Vector3.one;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform laneObject = transform.GetChild(i);

            if (laneObject == transform)
            {
                continue;
            }

            if (laneObject.name == "Root")
            {
                Root = laneObject.position;
            }
            else
            {
                List<Vector3> positions = new List<Vector3>();

                positions.Add((Vector3)Root);

                foreach (Transform node in laneObject.GetComponentsInChildren<Transform>())
                {
                    if (node.GetComponent<PathRenderer>() != null)
                    {
                        continue;
                    }

                    positions.Add(node.position);
                }

                for (int n = 0; n < positions.Count - 1; n++)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(positions[n], positions[n + 1]);
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(positions[n + 1], 0.5f);
                }
            }
        }
    }
}

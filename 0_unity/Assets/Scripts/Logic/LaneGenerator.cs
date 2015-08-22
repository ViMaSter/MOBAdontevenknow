using UnityEngine;
using System.Collections.Generic;

public static class Lanes
{
    public static bool Initialized;
    public static Dictionary<string, Lane> LeftTeam;
    public static Dictionary<string, Lane> RightTeam;
    public static void Init () {
        if (Initialized)
        {
            return;
        }

        Initialized = true;

        LeftTeam = new Dictionary<string, Lane>();
        RightTeam = new Dictionary<string, Lane>();
    }
}

public class Lane
{
    public Vector3 Root;
    public Vector3[] Nodes;
    public Lane(Vector3 root, Vector3[] nodes)
    {
        Root = root;
        Nodes = nodes;
    }
}

public class LaneGenerator : MonoBehaviour {
    public bool IsLeftTeam = true;

	void Start () {
        Lanes.Init();

        // Location of root/spawn
        Vector3 Root = -Vector3.one;

        // Loop over all nodes
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform laneObject = transform.GetChild(i);
            List<Vector3> positions = new List<Vector3>();

            if (laneObject.name == "Root")
            {
                Root = laneObject.position;
            }
            else
            {
                positions.Add(Root);

                foreach (Transform node in laneObject.GetComponentsInChildren<Transform>())
                {
                    if (node == transform || node == laneObject)
                    {
                        continue;
                    }

                    positions.Add(node.position);
                }

                (IsLeftTeam ? Lanes.LeftTeam : Lanes.RightTeam).Add(laneObject.name, new Lane(Root, positions.ToArray()));
            }
        }
	}
}

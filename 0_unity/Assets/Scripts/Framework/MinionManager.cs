using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Minions
{
    public static MinionManager MinionManagerInstance;
    public static List<GameObject> AllMinions;
    public static List<GameObject> ControllableMinions;

    private static int CurrentID;
    private static int NextID
    {
        get {
            return CurrentID++;
        }
    }

    public static void Init(MinionManager minionManagerInstance) {
        CurrentID = 0;
        MinionManagerInstance = minionManagerInstance;

        AllMinions = new List<GameObject>();
        ControllableMinions = new List<GameObject>();
    }

    public static GameObject Create(bool isLeftTeam, Lane lane)
    {
        GameObject gameObject = (GameObject)GameObject.Instantiate(
            isLeftTeam ? MinionManagerInstance.LeftMinionPrefab : MinionManagerInstance.RightMinionPrefab,
            lane.Root,
            Quaternion.identity
        );

        gameObject.name = string.Format("[{0}] Minion {1:D5}", isLeftTeam ? "L" : "R", NextID);
        gameObject.transform.parent = MinionManagerInstance.ParentGameObject.transform;
        gameObject.transform.position = lane.Root;

        gameObject.GetComponent<EnemyBehaviour>().Init(lane);
        gameObject.GetComponent<TeamAssociation>().IsLeftTeam = isLeftTeam;

        AllMinions.Add(gameObject);
        ControllableMinions.Add(gameObject);

        return gameObject;
    }

    public static void SpawnAllLanes(bool leftTeam, int MinionsPerLane)
    {
        foreach (KeyValuePair<string, Lane> lane in leftTeam ? Lanes.LeftTeam : Lanes.RightTeam)
        {
            for (int i = 0; i < MinionsPerLane; i++)
            {
                Minions.Create(leftTeam, (leftTeam ? Lanes.LeftTeam : Lanes.RightTeam)[lane.Key]);
            }
        }
    }

    public static GameObject GetControllableMinion()
    {
        if (ControllableMinions.Count <= 0)
        {
            return null;
        }

        // Choose a random minion
        int randomIndex = Random.Range(0, ControllableMinions.Count-1);

        // Take it...
        GameObject minion = ControllableMinions[randomIndex];

        // ...remove it from the list...
        ControllableMinions.RemoveAt(randomIndex);

        // and return it
        return minion;
    }
}

public class MinionManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject LeftMinionPrefab;
    public GameObject RightMinionPrefab;

    [Header("Worldsettings")]
    public GameObject ParentGameObject;

    public void Start() {
        Minions.Init(this);
    }
}

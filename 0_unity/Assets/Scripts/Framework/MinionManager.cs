using UnityEngine;
using System.Collections;

public static class Minions
{
    public static MinionManager MinionManagerInstance;
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
        return gameObject;
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

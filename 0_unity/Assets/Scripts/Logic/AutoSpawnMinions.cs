using UnityEngine;
using System.Collections.Generic;

public class AutoSpawnMinions : MonoBehaviour {
    public GameObject LeftMinionPrefab;
    public GameObject RightMinionPrefab;

    public float SpawnInterval = 14.0f;
    private float LastSpawnAt = 0.0f;

    public int UnitsPerLane = 4;

    public void Start() {
        LastSpawnAt = -SpawnInterval;
    }

    void Update()
    {
        if ((Time.time - LastSpawnAt) > SpawnInterval)
        {
            LastSpawnAt = Time.time;

            foreach (KeyValuePair<string, Lane> lane in Lanes.LeftTeam)
            {
                for (int i = 0; i < UnitsPerLane; i++)
                {
                    Minions.Create(true, Lanes.LeftTeam[lane.Key]);
                }
            }

            foreach (KeyValuePair<string, Lane> lane in Lanes.RightTeam)
            {
                for (int i = 0; i < UnitsPerLane; i++)
                {
                    Minions.Create(false, Lanes.RightTeam[lane.Key]);
                }
            }
        }
    }
}

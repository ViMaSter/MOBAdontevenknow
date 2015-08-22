using UnityEngine;
using System.Collections.Generic;

public class AutoSpawnMinions : MonoBehaviour {
    public GameObject LeftMinionPrefab;
    public GameObject RightMinionPrefab;

    public float SpawnInterval = 14.0f;
    private float LastSpawnAt = 0.0f;

    public int UnitsPerLane = 4;

    void Start() {
        LastSpawnAt = -SpawnInterval;
    }

    void Update()
    {
        if ((Time.time - LastSpawnAt) > SpawnInterval)
        {
            LastSpawnAt = Time.time;

            Minions.SpawnAllLanes(true, UnitsPerLane);
            Minions.SpawnAllLanes(false, UnitsPerLane);
        }
    }
}

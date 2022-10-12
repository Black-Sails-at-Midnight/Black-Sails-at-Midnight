using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PrimaryWaveSystem;

public class PrimaryWaveSystem : MonoBehaviour
{
    [Serializable]
    public struct EnemySpawn
    {
        [SerializeField]
        GameObject prefab;
        [SerializeField]
        int Count;

        public GameObject GetShipType() { return prefab; }
        public int GetNumberOfEnemies() { return Count; }
    }

    [Serializable]
    public struct Wave
    {
        [SerializeField]
        EnemySpawn[] enemiesInWave;
        [SerializeField]
        int timeInSecondsBetweenSpawns;
        [SerializeField]
        int TimeUntilNextWave;
        public EnemySpawn[] GetEnemyData()  { return enemiesInWave; }

        public int GetTimeBetweenSpawns() { return timeInSecondsBetweenSpawns; }
        public int GetTimeUntilNextWave() { return TimeUntilNextWave; }
    }

    [SerializeField]
    RingSystem RingSystem;

    [SerializeField]
    int OuterMostRingNumber;

    [SerializeField]
    int numberOfPositions;

    [SerializeField]
    List<Wave> Waves;

    [SerializeField]
    public int currentWave;

    [SerializeField]
    public bool isDone;

    [SerializeField]
    public bool waveInProgress;

    bool isSpawning;
    bool isWaitingForSpawnDelay = false;
    int EnemySpawnID;

    // Start is called before the first frame update
    void Start()
    {
        GameObject temp = GameObject.Find("Rings");
        RingSystem = temp.GetComponent<RingsManager>().GetRing(temp.GetComponent<RingsManager>().GetNumberOfRings() - 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaitingForSpawnDelay && waveInProgress)
        {
            if (!isSpawning)
            {
                SpawnWave();
            }
        }
        else if (Waves[currentWave].GetEnemyData().Length <= EnemySpawnID )
        {
            StartCoroutine(DelayUntilNextWave(currentWave));
        }    
        
    }
    void SpawnWave()
    {
        StartCoroutine(SpawnShip());
    }

    IEnumerator DelayUntilNextWave(int wave)
    {
        waveInProgress = false;
        EnemySpawnID = 0;
        currentWave++;
        yield return new WaitForSeconds(Waves[wave].GetTimeUntilNextWave());
        waveInProgress = true;
    }
    IEnumerator SpawnShip()
    {
        isSpawning = true;
        EnemySpawn spawn = Waves[currentWave].GetEnemyData()[EnemySpawnID];
        for (int i = 0; i < spawn.GetNumberOfEnemies(); i++)
        {
            int nearestCoordiante = RingSystem.GetClosestPoint(this.transform.position);
            GameObject enemyShip = Instantiate(spawn.GetShipType(), RingSystem.GetNextPosition(nearestCoordiante), Quaternion.identity);

            if (enemyShip.name != "Basic Enemy")
            {
                enemyShip.GetComponent<SpecialEnemyNavigation>().CurrentPosition = nearestCoordiante;
                enemyShip.GetComponent<SpecialEnemyNavigation>().ForceWaypointUpdate();
            }
        }

        yield return new WaitForSeconds(Waves[currentWave].GetTimeBetweenSpawns());
        EnemySpawnID++;
        isSpawning = false;
    }
}

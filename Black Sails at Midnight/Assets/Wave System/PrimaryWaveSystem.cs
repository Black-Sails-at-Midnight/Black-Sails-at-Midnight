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
    List<Wave> Waves;

    [SerializeField]
    public int currentWave;

    [SerializeField]
    public bool isDone;

    [SerializeField]
    public bool waveInProgress = true;

    bool isWaitingForSpawnDelay = false;
    bool isSpawning;
    
    int EnemySpawnID;

    // Start is called before the first frame update
    void OnEnable()
    {
        RingsManager temp = FindObjectOfType<RingsManager>();
        RingSystem = temp.GetComponent<RingsManager>().GetRing(temp.GetNumberOfRings() - 1);

        StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWave >= Waves.Count && !waveInProgress)
        {
            isDone = true;
            enabled = false;
        }

        if (EnemySpawnID >= Waves[currentWave - 1].GetEnemyData().Length && waveInProgress)
        {
            StartCoroutine(DelayUntilNextWave(currentWave - 1));
            return;
        } 

        if (!isWaitingForSpawnDelay && waveInProgress)
        {
            if (!isSpawning)
            {
                SpawnWave();
            }
        }     
    }
    void SpawnWave()
    {
        StartCoroutine(SpawnShip());
    }

    public void StartWave()
    {
        if (isDone)
            return;

        waveInProgress = true;
        EnemySpawnID = 0;
        currentWave++;
    }

    IEnumerator DelayUntilNextWave(int wave)
    {
        waveInProgress = false;

        yield return new WaitForSeconds(Waves[wave].GetTimeUntilNextWave());
        StartWave();
    }
    IEnumerator SpawnShip()
    {
        isSpawning = true;
        EnemySpawn spawn = Waves[currentWave - 1].GetEnemyData()[EnemySpawnID];
        for (int i = 0; i < spawn.GetNumberOfEnemies(); i++)
        {
            int randomCoordinate = UnityEngine.Random.Range(0, RingSystem.GetNumberOfCoordinates() - 1);

            Vector3 Pos =  RingSystem.GetNextPosition(randomCoordinate);

            GameObject enemyShip = Instantiate(spawn.GetShipType(), RingSystem.GetNextPosition(randomCoordinate), Quaternion.identity);

            if (enemyShip.GetComponent<SpecialEnemyNavigation>() != null)
            {
                enemyShip.GetComponent<SpecialEnemyNavigation>().CurrentPosition = randomCoordinate;
                enemyShip.GetComponent<SpecialEnemyNavigation>().StartNavigation();
            }
            
            if (enemyShip.GetComponent<BasicEnemyNavigation>() != null)
            {
                enemyShip.GetComponent<BasicEnemyNavigation>().targetCoordinate = randomCoordinate;
                enemyShip.GetComponent<BasicEnemyNavigation>().StartNavigation();
            }
        }

        if (EnemySpawnID < Waves[currentWave - 1].GetEnemyData().Length - 1)
            yield return new WaitForSeconds(Waves[currentWave - 1].GetTimeBetweenSpawns());

        EnemySpawnID++;
        isSpawning = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [Serializable]
    public struct EnemySpawn
    {
        [SerializeField]
        GameObject prefab;
        [SerializeField]
        int numberOfEnemies;
    }

    [Serializable]
    public struct Wave
    {
        [SerializeField]
        EnemySpawn[] enemiesInWave;
    }

    [SerializeField]
    List<Wave> Waves;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

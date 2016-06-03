using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public Enemy enemy;

    private Wave currentWave;
    private int currentWaveNumber;
    private int enemiesRemainingToSpawn;
    private int enemiesRemainingAlive;
    private float nextSpawnTime;

    private void Start()
    {
        NextWave();
    }

    private void Update()
    {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            var spawnedEnemy = (Enemy)Instantiate(enemy, Vector3.zero, Quaternion.identity);
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
    }

    private void OnEnemyDeath()
    {
        print("Enemy died");
        enemiesRemainingAlive--;

        if (enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    private void NextWave()
    {
        currentWaveNumber++;
        print("Wave: " + currentWaveNumber);

        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];
        }

        enemiesRemainingToSpawn = currentWave.enemyCount;
        enemiesRemainingAlive = enemiesRemainingToSpawn;
    }

    [Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }
}
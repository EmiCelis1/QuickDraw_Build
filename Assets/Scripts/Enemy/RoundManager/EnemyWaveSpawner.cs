using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{
    [Header("Configuración de Rondas")]
    [SerializeField] private List<WaveData> waves;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float preparationTimeBetweenWaves = 5f;

    [Header("Referencias")]
    [SerializeField] private EnemyManager enemyManager;

    private int currentWaveIndex = 0;
    private bool isSpawning = false;

    // Evento que se dispara al terminar una ronda. 
    public event Action<int> OnWaveCompleted;
    public event Action OnAllWavesCompleted;

    private void Start()
    {
        if (enemyManager == null)
        {
            // Actualizado a la recomendación moderna de Unity
            enemyManager = FindFirstObjectByType<EnemyManager>();
        }

        StartCoroutine(StartWaveSequence());
    }

    private IEnumerator StartWaveSequence()
    {
        // Pequeña pausa antes de iniciar la primera ronda (Línea corregida)
        yield return new WaitForSeconds(2f);

        while (currentWaveIndex < waves.Count)
        {
            WaveData currentWave = waves[currentWaveIndex];
            Debug.Log($"--- INICIANDO RONDA {currentWave.waveNumber} ---");

            // 1. Spawnea los enemigos de esta ronda
            yield return StartCoroutine(SpawnWaveRoutine(currentWave));

            // 2. Espera hasta que todos los enemigos de la lista del EnemyManager sean derrotados
            yield return new WaitUntil(() => enemyManager.enemiesInTrigger.Count == 0 && !isSimulatingOrSpawning());

            Debug.Log($"--- RONDA {currentWave.waveNumber} COMPLETADA ---");

            // 3. Notificamos que la ronda terminó
            OnWaveCompleted?.Invoke(currentWave.waveNumber);

            // 4. Tiempo de descanso antes de la siguiente ronda
            yield return new WaitForSeconds(preparationTimeBetweenWaves);

            currentWaveIndex++;
        }

        Debug.Log("Todas las rondas completadas con éxito");
        OnAllWavesCompleted?.Invoke();
    }

    private bool isSimulatingOrSpawning()
    {
        return isSpawning;
    }

    private IEnumerator SpawnWaveRoutine(WaveData wave)
    {
        isSpawning = true;

        foreach (var enemyInfo in wave.enemiesToSpawn)
        {
            for (int i = 0; i < enemyInfo.count; i++)
            {
                SpawnEnemy(enemyInfo.enemyPrefab);
                yield return new WaitForSeconds(wave.timeBetweenSpawns);
            }
        }

        isSpawning = false;
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab == null) return;

        Transform spawnPoint = transform;
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        }

        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        Enemy enemyComponent = spawnedEnemy.GetComponent<Enemy>();
        if (enemyComponent != null && enemyManager != null)
        {
            enemyManager.AddEnemy(enemyComponent);
        }
    }
}
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

    // Evento que se dispara al terminar una ronda
    public event Action<int> OnWaveCompleted;
    public event Action OnAllWavesCompleted;

    private void Start()
    {
        if (enemyManager == null)
        {
            enemyManager = FindFirstObjectByType<EnemyManager>();
        }

        if (enemyManager == null)
        {
            Debug.LogError("EnemyWaveSpawner no encontró ningún EnemyManager en la escena");
            return;
        }

        StartCoroutine(StartWaveSequence());
    }

    private IEnumerator StartWaveSequence()
    {
        yield return new WaitForSeconds(2f); // Pausa inicial

        while (currentWaveIndex < waves.Count)
        {
            WaveData currentWave = waves[currentWaveIndex];
            Debug.Log($"- ROUND START {currentWave.waveNumber} -");

            // 1. Spawnea los enemigos de esta ronda
            yield return StartCoroutine(SpawnWaveRoutine(currentWave));

            // 2. Esperamos a que terminen de aparecer Y a que la lista del EnemyManager esté vacía
            yield return new WaitUntil(() => !isSpawning && enemyManager.enemiesInTrigger.Count == 0);

            Debug.Log($"- ROUND {currentWave.waveNumber} COMPLETE -");

            // 3. Registramos la ronda superada en GameStats
            if (GameStats.Instance != null)
            {
                GameStats.Instance.RoundCompleted();
            }

            // 4. Notificamos que la ronda terminó (Para UI u otros sistemas)
            OnWaveCompleted?.Invoke(currentWave.waveNumber);

            // 5. Tiempo de descanso antes de la siguiente ronda
            yield return new WaitForSeconds(preparationTimeBetweenWaves);

            currentWaveIndex++;
        }

        Debug.Log("¡Todas las rondas completadas con éxito!");
        OnAllWavesCompleted?.Invoke();
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

        // Si no hay puntos de spawn asignados, usa la posición del propio Spawner
        Transform spawnPoint = transform;
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
            if (spawnPoints[randomIndex] != null)
            {
                spawnPoint = spawnPoints[randomIndex];
            }
        }

        // Instanciamos el enemigo
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // Nos aseguramos de registrarlo en el EnemyManager de forma directa y segura
        Enemy enemyComponent = spawnedEnemy.GetComponent<Enemy>();
        if (enemyComponent != null && enemyManager != null)
        {
            enemyManager.AddEnemy(enemyComponent);
        }
        else
        {
            Debug.LogWarning($"El prefab {enemyPrefab.name} no tiene el componente Enemy o falta el EnemyManager.");
        }
    }
}
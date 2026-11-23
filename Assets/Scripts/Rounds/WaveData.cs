using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveData", menuName = "Game/Wave Data")]
public class WaveData : ScriptableObject
{
    [System.Serializable]
    public struct EnemySpawnInfo
    {
        public GameObject enemyPrefab;
        public int count;
    }

    [Header("Configuración de la Ronda")]
    public int waveNumber;
    public float timeBetweenSpawns = 1f;
    public EnemySpawnInfo[] enemiesToSpawn;
}
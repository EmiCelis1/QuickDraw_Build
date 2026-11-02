using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance { get; private set; }

    public int Score { get; private set; }
    public int Kills { get; private set; }
    public int RoundsSurvived { get; private set; }

    private void Awake() => Instance = this;

    public void AddScore(int points) => Score += points;
    public void AddKill() => Kills++;
    public void RoundCompleted() => RoundsSurvived++;
}
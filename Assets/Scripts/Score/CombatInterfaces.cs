using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float amount);
}

public interface IScoreable
{
    int BasePoints { get; }
    void AwardScore();
}
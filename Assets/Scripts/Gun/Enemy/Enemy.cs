using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyManager enemyManager;
    private Animator spriteAnim;
    /*private AngleToPlayer angleToPlayer;*/

    public float enemyHealth = 2f;

    public GameObject gunHitEffect;

    public int attackDamage = 10;

    private void Start()
    {
        spriteAnim = GetComponentInChildren<Animator>();
        /*angleToPlayer = GetComponent<AngleToPlayer>();*/

        enemyManager = FindObjectOfType<EnemyManager>(); // we can do this because theres only (1) in the scene.
    }

    void Update()
    {
        // Al inicio del update seteamos el índice de rotación de las animaciones
        /*spriteAnim.SetFloat("spriteRot", angleToPlayer.lastIndex);*/

        if (enemyHealth <= 0)
        {
            // Validamos si realmente existe el enemyManager antes de llamarlo
            if (enemyManager != null)
            {
                enemyManager.RemoveEnemy(this);
            }
            else
            {
                Debug.LogWarning($"Se destruyó {gameObject.name} pero no se encontró un EnemyManager en la escena.");
            }

            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        Instantiate(gunHitEffect, transform.position, Quaternion.identity);
        enemyHealth -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        /*PlayerHealth player = other.GetComponent<PlayerHealth>();*/

        
    }
}

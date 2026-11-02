using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float bulletDamage = 1f;

    private Coroutine returnCoroutine;
    private bool isReturning = false;

    void OnEnable()
    {
        isReturning = false;
        returnCoroutine = StartCoroutine(ReturnRoutine());
    }

    IEnumerator ReturnRoutine()
    {
        yield return new WaitForSeconds(lifetime);
        Deactivate();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Si ya nos estamos desactivando, ignoramos para evitar doble impacto
        if (isReturning) return;

        // Buscamos IDamageable tanto en el objeto colisionado como en sus padres
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable == null)
        {
            damageable = collision.gameObject.GetComponentInParent<IDamageable>();
        }

        if (damageable != null)
        {
            damageable.TakeDamage(bulletDamage);
            Debug.Log($"[Bullet] Impacto exitoso en: {collision.gameObject.name}, aplicando {bulletDamage} de daño.");
        }
        else
        {
            Debug.LogWarning($"[Bullet] Chocó con {collision.gameObject.name}, pero NO implementa IDamageable.");
        }

        Deactivate();
    }

    private void Deactivate()
    {
        if (isReturning) return;
        isReturning = true;

        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
        }

        if (BulletManager.Instance != null)
        {
            BulletManager.Instance.ReturnBullet(gameObject);
        }
        else
        {
            Destroy(gameObject); // Fallback por si acaso
        }
    }
}
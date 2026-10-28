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
        // Buscamos la interfaz IDamageable en lugar de la clase Enemy
        IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(bulletDamage);
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

        BulletManager.Instance.ReturnBullet(gameObject);
    }
}
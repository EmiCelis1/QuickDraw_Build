using UnityEngine;

public class SimpleGunShoot : MonoBehaviour
{
    [Header("Referencias")]
    public Transform firePoint;
    public float bulletSpeed = 30f;
    public ParticleSystem Flash;

    [Header("Configuración de Munición")]
    public int maxAmmo = 6;
    private int currentAmmo;

    void Start()
    {
        // Cargador lleno al iniciar
        currentAmmo = maxAmmo;
    }

    public void Shoot()
    {
        // No dispara si no hay munición
        if (currentAmmo <= 0)
        {
            Debug.Log("Sin munición");
            // Sonido de municion vacia ?
            return;
        }

        currentAmmo--;
        Flash.Play();

        GameObject bullet = BulletManager.Instance.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * bulletSpeed;

        Debug.Log("Balas restantes: " + currentAmmo);
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
        Debug.Log("¡Arma recargada! Balas: " + currentAmmo);
        // Sonido de recarga ?
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
}
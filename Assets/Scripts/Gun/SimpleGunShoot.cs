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

    [Header("Configuración de Sonido")]
    public AudioSource audioSource;
    public AudioClip sonidoDisparo;
    public AudioClip sonidoRecarga;
    public AudioClip sonidoVacio;

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
            audioSource.PlayOneShot(sonidoVacio);
            Debug.Log("Sin munición");
            return;
        }

        currentAmmo--;
        Flash.Play();
        audioSource.PlayOneShot(sonidoDisparo);
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
        audioSource.PlayOneShot(sonidoRecarga);
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
}
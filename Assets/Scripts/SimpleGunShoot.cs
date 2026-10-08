using UnityEngine;

public class SimpleGunShoot : MonoBehaviour
{
    [Header("Referencias")]
    public Transform firePoint;
    public float bulletSpeed = 30f;
    public ParticleSystem Flash;


    public void Shoot()
    {
        Flash.Play();
        GameObject bullet = BulletManager.Instance.GetBullet();

        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * bulletSpeed;
    }
}
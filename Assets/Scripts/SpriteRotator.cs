using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    [SerializeField] private bool _doomMode = true;
    [SerializeField] private float _spinSpeed = 100f;

    void LateUpdate()
    {
        if (_doomMode && Camera.main != null)
        {
            // Copia la rotación de la cámara para que siempre quede de frente al visor
            Vector3 targetEuler = Camera.main.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, targetEuler.y, 0); // Bloquea los ejes X y Z para que no se incline de cabeza
        }
        else
        {
            transform.Rotate(Vector3.up * _spinSpeed * Time.deltaTime);
        }
    }
}
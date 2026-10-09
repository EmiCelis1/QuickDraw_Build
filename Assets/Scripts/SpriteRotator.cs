using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    [SerializeField] private bool _doomMode = true;
    [SerializeField] private float _spinSpeed = 100f;

    void LateUpdate()
    {
        if (_doomMode && Camera.main != null)
        {
            
            Vector3 targetEuler = Camera.main.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, targetEuler.y, 0); 
        }
        else
        {
            transform.Rotate(Vector3.up * _spinSpeed * Time.deltaTime);
        }
    }
}
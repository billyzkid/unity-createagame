using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed = 10;

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
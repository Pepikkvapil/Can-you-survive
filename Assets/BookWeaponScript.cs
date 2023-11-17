using UnityEngine;

public class BookWeapon : MonoBehaviour
{
    public float rotationSpeed = 500f;
    public int damage = 10; // Damage that the book deals to enemies
    public Transform rotation;

    private void Start()
    {
        GameObject rotationCenter = GameObject.FindGameObjectWithTag("RotationPoint");

        // Check if the playerObject is not null before accessing its transform
        if (rotationCenter != null)
        {
            rotation = rotationCenter.transform;
        }
    }

    private void Update()
    {
        if (rotation)
        {
            // Rotate around the player
            transform.RotateAround(rotation.position, Vector3.forward, rotationSpeed * Time.deltaTime);

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Deal damage to the enemy
            other.GetComponent<Health>()?.Damage(damage);
            // Optionally destroy the book or create some visual effect
        }
    }
}

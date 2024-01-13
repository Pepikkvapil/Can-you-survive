using UnityEngine;

public class BookWeapon : MonoBehaviour
{
    private float rotationSpeed = 80f;
    public int damage = 10; // Damage that the book deals to enemies
    public Transform rotation;
    public bool FinalBookUpgraded = false;

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
            other.GetComponent<Enemy>()?.DamagingEnemy(damage);
            // Optionally destroy the book or create some visual effect
        }
    }

    public void ApplyFinalUpgrade()
    {
        if (!FinalBookUpgraded)
        {
            // Implement the final upgrade logic here
            // For example, increase damage significantly, change appearance, or add special abilities
            damage *= 2; // Double the damage
            rotationSpeed *= 2;

            // You can also add any other changes or effects you want for the final upgrade

            FinalBookUpgraded = true; // Mark the final upgrade as applied
        }
    }


}

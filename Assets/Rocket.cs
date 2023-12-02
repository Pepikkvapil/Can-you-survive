using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;
    private Vector3 lastDirection;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (target != null)
        {
            lastDirection = (target.position - transform.position).normalized;
            RotateTowards(lastDirection);
        }
    }

    void Update()
    {
        if (target != null)
        {
            lastDirection = (target.position - transform.position).normalized;
            RotateTowards(lastDirection);
        }

        transform.position += lastDirection * speed * Time.deltaTime;
    }

    private void RotateTowards(Vector3 direction)
    {
        // For a 2D game
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // Adjust the offset if needed

        // For a 3D game, use Quaternion.LookRotation and adjust as necessary
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Assuming your BookWeapon script deals with damaging the enemy
            collision.GetComponent<Enemy>()?.Damage(10); // Replace 10 with your desired damage value
            // Add any additional impact effects here

            Destroy(gameObject); // Destroy the rocket
        }
        else if (collision.CompareTag("Walls"))
        {
            // Handle hitting something else, like a wall
            Destroy(gameObject); // Optionally destroy the rocket if it hits something else
        }
    }
}
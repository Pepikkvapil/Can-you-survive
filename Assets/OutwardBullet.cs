using UnityEngine;

public class OutwardBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float force = 6;
    public int damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Calculate the rotation angle based on the bullet's initial velocity
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;

        // Set the bullet's rotation to face the direction it's moving
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

    }

    void Update()
    {
        // Check if the bullet is outside the camera's view
        if (!IsInsideCameraView())
        {
            // Destroy the bullet when it goes off-camera
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null && other.CompareTag("Enemy"))
        {
            Enemy health = other.GetComponent<Enemy>();
            health.Damage(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Walls"))
        {
            Destroy(gameObject);
        }
    }

    // Check if the bullet is inside the camera's view
    bool IsInsideCameraView()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);
            return viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1;
        }
        return false;
    }
}

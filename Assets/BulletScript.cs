using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float force;

    private int baseDamage = 5;
    public static float damageMultiplier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    // Update is called once per frame
    void Update()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            float minX = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
            float maxX = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
            float minY = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
            float maxY = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

            Vector3 bulletPosition = transform.position;

            // Check if the bullet is outside the camera's view
            if (bulletPosition.x < minX || bulletPosition.x > maxX || bulletPosition.y < minY || bulletPosition.y > maxY)
            {
                // Destroy the bullet when it goes off-camera
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Health>() != null && other.CompareTag("Enemy"))
        {
            Health health = other.GetComponent<Health>();
            health.Damage((int)(baseDamage * damageMultiplier)); // Apply the damage multiplier
            Destroy(gameObject);
        }
        else if (other.CompareTag("Walls"))
        {
            Destroy(gameObject);
        }
    }

    public static void IncreaseDamageMultiplier(float amount)
    {
        damageMultiplier += amount;
    }

}

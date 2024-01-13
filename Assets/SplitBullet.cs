using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBullet : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float force;

    private int baseDamage = 2; // Reduced damage

    public GameObject outwardBulletPrefab; // Prefab for the regular bullets

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
            health.DamagingEnemy((int)(baseDamage));
            SpawnOutwardBullets();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Walls"))
        {
            SpawnOutwardBullets();

            Destroy(gameObject);
        }
    }

    void SpawnOutwardBullets()
    {
        // Angle between each outward bullet (e.g., 60 degrees)
        float angleStep = 60f;
        float spawnDistance = 1.0f; // Adjust the spawn distance as needed

        for (int i = 0; i < 6; i++) // Spawn 6 outward bullets evenly distributed
        {
            float angle = i * angleStep;
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.up;

            // Calculate the spawn position for the outward bullet
            Vector3 spawnPosition = transform.position + direction.normalized * spawnDistance;

            // Create an outward bullet instance
            GameObject outwardBulletInstance = Instantiate(outwardBulletPrefab, spawnPosition, Quaternion.identity);

            // Rotate the bullet to face the direction it is sent
            float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            outwardBulletInstance.transform.rotation = Quaternion.Euler(0, 0, rot);

            // Get the Rigidbody2D component and set the velocity based on the direction
            Rigidbody2D rb = outwardBulletInstance.GetComponent<Rigidbody2D>();
            rb.velocity = direction.normalized * force;

            // Set the damage for the outward bullet
            outwardBulletInstance.GetComponent<OutwardBullet>(); // Adjust the damage as needed

            
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

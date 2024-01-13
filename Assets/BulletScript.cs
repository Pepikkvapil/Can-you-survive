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

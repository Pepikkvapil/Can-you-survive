using UnityEngine;

public class SplitBaseWeapon : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject splitBulletPrefab;
    public Transform bulletSpawnPoint;
    public bool canFire;
    private float timer;
    public float timeBetweenFiring;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButton(0) && canFire)
        {
            canFire = false;

            // Fire the bullet forward
            FireBullet(Vector3.up);
        }
    }

    void FireBullet(Vector3 direction)
    {
        // Create a bullet instance
        GameObject bulletInstance = Instantiate(splitBulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        // Get the Rigidbody2D component and set the velocity based on the direction
        Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * splitBulletPrefab.GetComponent<SplitBullet>().force;
    }
}

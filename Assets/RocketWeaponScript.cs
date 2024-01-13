using UnityEngine;

public class RocketsWeapon : MonoBehaviour
{
    public GameObject rocketPrefab;
    public float fireInterval = 2f;
    public int rocketDamage = 10;

    private float fireTimer;
    private bool isActive = false; // Flag to track activation

    public bool isFinalUpgrade = false;
    public int upgradecount = 0;


    void Update()
    {
        if (!isActive) return; // Don't proceed if the weapon is not active

        fireTimer += Time.deltaTime;

        if (fireTimer >= fireInterval)
        {
            FireRocket();
            fireTimer = 0;
        }

        IsFinalUpgradeConditionMet();
    }


    


    public void IsFinalUpgradeConditionMet()
    {
        if(upgradecount == 5)
        isFinalUpgrade = true;
    }
    public bool FinalRocketUpgraded = false;
    
    // Method to upgrade or activate the rocket weapon
    public void UpgradeRocketWeapon()
    {
        if (!isActive)
        {
            // Activate the weapon
            isActive = true;
            enabled = true; // Enable this script/component if it's initially disabled
        }
        else
        {
            // Upgrade the weapon
            if (isFinalUpgrade == true)
            {
                Debug.Log("Finalni upgrade");
                fireInterval = 1.5f;
                rocketDamage = Mathf.CeilToInt(rocketDamage * 1.5f);
                FinalRocketUpgraded = true;
                //playerController.ApplyFinalRocketUpgrade();
            }
            else
            {
                upgradecount++;
                Debug.Log("upgrade cislo " + upgradecount);
                rocketDamage += 5; // Increase damage
                fireInterval = Mathf.Max(fireInterval * 0.9f, 0.5f); // Decrease interval
            }
        }
    }


    void FireRocket()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            GameObject rocket = Instantiate(rocketPrefab, transform.position, Quaternion.identity);
            rocket.GetComponent<Rocket>().SetTarget(nearestEnemy.transform);
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float minDistance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, position);
            if (distance < minDistance)
            {
                nearest = enemy;
                minDistance = distance;
            }
        }

        return nearest;
    }
}

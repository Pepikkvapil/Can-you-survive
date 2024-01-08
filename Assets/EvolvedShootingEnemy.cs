using UnityEngine;

public class EvolvedShootingEnemy : ShootingEnemy
{
    public int bulletsToShoot = 10; // Number of bullets to shoot before waiting
    public float timeBetweenShots = 0.5f; // Time between each shot
    public float waitTime = 5.0f; // Time to wait before shooting again

    private int bulletsShot = 0;
    private bool isWaiting = false;
    private float lastShotTime;
    private float lastWaitTime;

    private new void Update()
    {
        base.Update();

        if (isWaiting)
        {
            // Check if enough time has passed since the last wait
            if (Time.time - lastWaitTime >= waitTime)
            {
                // Reset the waiting state and shot count
                isWaiting = false;
                bulletsShot = 0;
                lastShotTime = Time.time;
            }
        }
        else if (bulletsShot < bulletsToShoot)
        {
            // Check if enough time has passed since the last shot
            if (Time.time - lastShotTime >= timeBetweenShots)
            {
                // Shoot a bullet in the direction of the player
                Shoot();
                bulletsShot++;

                lastShotTime = Time.time;

                // If all bullets are shot, enter the waiting state
                if (bulletsShot >= bulletsToShoot)
                {
                    isWaiting = true;
                    lastWaitTime = Time.time;
                }
            }
        }
    }
}

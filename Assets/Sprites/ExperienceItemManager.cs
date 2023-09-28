using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceItemManager : MonoBehaviour
{

    int expAmount = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("HIT");

            //AddExperience(expAmount);
            ExperienceManager.Instance.AddExperience(expAmount);
            Destroy(gameObject);

        }
    }
}

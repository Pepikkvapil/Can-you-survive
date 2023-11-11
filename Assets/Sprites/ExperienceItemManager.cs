using UnityEngine;

public class ExperienceItemManager : MonoBehaviour
{
    public int expAmount = 5;
    public float pullRange = 5f; // Range within which the XP starts getting pulled towards the player
    public float pullSpeed = 5f; // Speed at which the XP moves towards the player

    private GameObject player; // Reference to the player object
    private bool isWithinRange = false; // Flag to check if within range to start pulling

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Find and assign the player object
    }

    private void Update()
    {
        // Check the distance to the player
        if (Vector2.Distance(transform.position, player.transform.position) <= pullRange)
        {
            isWithinRange = true;
        }

        // If within range, start moving the XP towards the player
        if (isWithinRange)
        {
            float step = pullSpeed * Time.deltaTime; // Calculate the step size
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, step);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("XP taken");
            ExperienceManager.Instance.AddExperience(expAmount);
            Destroy(gameObject);
        }
    }


}

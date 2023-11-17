using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int enemyDamage = 20;
    public float playerStamina = 100.0f;
    public static float speedMultiplier = 1f;

    [SerializeField] private float maxStamina = 100.0f;
    [SerializeField] private float dashCost = 20;
    [SerializeField] private float staminaDrain = 0.5f;
    [SerializeField] private float staminaRegen = 0.5f;
    [SerializeField] private int RunSpeed = 6;
    [SerializeField] private int normalRunSpeed = 4;
    [SerializeField] private Image staminaProgressUI = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;
    [SerializeField] private Image HPProgressUI = null;
    public GameManagerScript gameManager;

    public SpriteRenderer entitySpriteRenderer;
    public Color redColor = Color.red;
    public float redDuration = 0.5f;

    [SerializeField] private int health = 100;
    public int MAX_HEALTH = 100;

    private Vector2 _Movement;
    private Rigidbody2D _Rigidbody;
    private bool hasRegenerated = true;
    private bool weAreSprinting = false;
    private bool isDead = false;

    public GameObject bookPrefab; // Assign the book prefab in the Unity Editor
    public Transform firingPointer; // Assign your firing pointer GameObject in the Unity Editor
    private List<GameObject> books = new List<GameObject>();
    public float bookDistance = 5f; // Distance of books from the firing pointer
    public int maxBooks = 2; // Start with 2 books

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnMovement(InputValue value)
    {
        _Movement = value.Get<Vector2>();
    }

    void Update()
    {
        UpdateHP();

        if (playerStamina <= 0)
        {
            weAreSprinting = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            weAreSprinting = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            weAreSprinting = false;
        }

        if (weAreSprinting)
        {
            _Rigidbody.velocity = _Movement * RunSpeed * speedMultiplier;
            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);

            if (playerStamina <= 0)
            {
                hasRegenerated = false;
                sliderCanvasGroup.alpha = 0;
            }
        }
        else
        {
            _Rigidbody.velocity = _Movement * normalRunSpeed * speedMultiplier;
        }

        if (!weAreSprinting && playerStamina <= maxStamina - 0.01)
        {
            playerStamina += staminaRegen * Time.deltaTime;
            UpdateStamina(1);

            if (playerStamina >= maxStamina)
            {
                sliderCanvasGroup.alpha = 0;
                hasRegenerated = true;
            }
        }
    }

    public void AddBook()
    {
        // Instantiate a new book
        GameObject newBook = Instantiate(bookPrefab, firingPointer.position, Quaternion.identity, firingPointer);

        // Add the new book to the list
        books.Add(newBook);

        // Update the positions of all books
        UpdateBookPositions();
    }

    private void UpdateBookPositions()
    {
        float angleStep = 360f / books.Count;
        for (int i = 0; i < books.Count; i++)
        {
            // Calculate the angle and position for each book
            float angle = angleStep * i;
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.up;
            books[i].transform.localPosition = direction * bookDistance;
        }
    }



    public void UpgradeBookWeapon()
    {
        if(books.Count == 0)
        {
            AddBook();
        }
        if (books.Count != 0)
        {
            maxBooks++;
            AddBook();
        }
    }

    void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;
        sliderCanvasGroup.alpha = value == 0 ? 0 : 1;
    }

    void UpdateHP()
    {
        HPProgressUI.fillAmount = (float)health / MAX_HEALTH;
    }

    public void Damage(int amount)
    {
        health -= amount;
        entitySpriteRenderer.color = redColor;
        StartCoroutine(RevertColor());

        if (health <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    private IEnumerator RevertColor()
    {
        yield return new WaitForSeconds(redDuration);
        entitySpriteRenderer.color = Color.white;
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
        }

        health = Mathf.Min(health + amount, MAX_HEALTH);
        UpdateHP();
    }

    private void Die()
    {
        Debug.Log("DEAD!");
        gameManager.gameOver();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Damage(enemyDamage);
        }
    }

    // Method to increase the player's maximum health
    public void IncreaseMaxHealth(int increaseAmount)
    {
        MAX_HEALTH += increaseAmount;
        health += increaseAmount; // Also increase the current health
        UpdateHP(); // Update the health UI to reflect the new maximum
    }

    public void IncreaseSpeedMultiplier(float amount)
    {
        speedMultiplier += amount;
    }
}

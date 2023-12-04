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
    public float redDuration = 0.3f;

    [SerializeField] private float health = 100.0f;
    [SerializeField] private float MAX_HEALTH = 100.0f;
    [SerializeField] private float hpRegen = 0.1f;

    private Vector2 _Movement;
    private Rigidbody2D _Rigidbody;
    private bool hasRegenerated = true;
    private bool weAreSprinting = false;
    private bool isDead = false;

    public GameObject bookPrefab; // Assign the book prefab in the Unity Editor
    public Transform firingPointer; // Assign your firing pointer GameObject in the Unity Editor

    //Books
    private List<GameObject> books = new List<GameObject>();
    public float bookDistance = 5f; // Distance of books from the firing pointer
    public int maxBooks = 2; // Start with 2 books

    //Rockets
    public RocketsWeapon rocketsWeapon;

    //SpellShield
    private bool spellShield = false;
    private bool spellShieldReady = false;
    private float spellShieldCooldown = 10f; // 10 seconds cooldown
    private float spellShieldTimer = 0f;
    public GameObject spellShieldPrefab; // Assign this in the Unity Editor
    private GameObject activeSpellShield;
    private float minSpellShieldCooldown = 5f; // Minimum cooldown limit
    private float cooldownDecreaseAmount = 1f; // Cooldown decrease per upgrade

    //Lightning
    public GameObject lightningPrefab; // Assign this in the Unity Editor
    public float lightningSpawnInterval = 5f; // Initial spawn interval in seconds
    private float lightningTimer;

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

        if(health <= MAX_HEALTH - 0.01)
        {
            health += hpRegen * Time.deltaTime;
            UpdateHP();
        }

    }

    public void EnableLightningWeapon()
    {
        if (!IsInvoking(nameof(SpawnLightning)))
        {
            InvokeRepeating(nameof(SpawnLightning), lightningSpawnInterval, lightningSpawnInterval);
        }
        else
        {
            lightningSpawnInterval = Mathf.Max(1f, lightningSpawnInterval - 1f); // Decrease interval, 1 second minimum
            CancelInvoke(nameof(SpawnLightning));
            InvokeRepeating(nameof(SpawnLightning), lightningSpawnInterval, lightningSpawnInterval);
        }
    }

    private void SpawnLightning()
    {
        // Define the range around the player where lightning can spawn
        float range = 9f; // Adjust this value based on how far from the player you want the lightning to spawn

        // Generate a random position within the range
        Vector3 offset = new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0);
        Vector3 spawnPosition = transform.position + offset;

        GameObject lightning = Instantiate(lightningPrefab, spawnPosition, Quaternion.identity);
        StartCoroutine(ActivateColliderAfterDelay(lightning));
    }

    private IEnumerator ActivateColliderAfterDelay(GameObject lightning)
    {
        yield return new WaitForSeconds(0.35f);
        lightning.GetComponent<Collider2D>().enabled = true;
    }
    public void EnableSpellShieldUpgrade()
    {
        if (spellShieldReady == false && spellShield == false)
        {
            spellShield = true;
            spellShieldReady = true;
            StartCoroutine(SpellShieldRoutine());
            Debug.Log("SpellshieldUpgrade získán");
        }
        else if (spellShield == true)
        {
            DecreaseSpellShieldCooldown();
        }
    }

    public void DecreaseSpellShieldCooldown()
    {
        if (spellShieldCooldown > minSpellShieldCooldown)
        {
            spellShieldCooldown = Mathf.Max(minSpellShieldCooldown, spellShieldCooldown - cooldownDecreaseAmount);
        }
    }

    private IEnumerator SpellShieldRoutine()
    {
        while (true) // Infinite loop, can be stopped or modified based on game logic
        {
            if (spellShieldReady)
            {
                ActivateSpellShield();
                yield return new WaitForSeconds(spellShieldCooldown); // Wait for cooldown
            }
            yield return null; // Ensures the coroutine doesn't block the game
        }
    }

    private void ActivateSpellShield()
    {
        spellShieldReady = false;
        spellShieldTimer = spellShieldCooldown;

        if (activeSpellShield != null)
        {
            Destroy(activeSpellShield); // Remove previous instance if exists
        }

        //Adjusting position
        Vector3 shieldPosition = transform.position + new Vector3(-0.26f, 0.027f, 0);
        activeSpellShield = Instantiate(spellShieldPrefab, shieldPosition, Quaternion.identity, transform);
    }


    public void EnableRocketWeapon()
    {
        if (rocketsWeapon != null)
        {
            rocketsWeapon.enabled = true;
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
        if (!spellShieldReady)
        {
            spellShieldReady = true; // Reactivate the shield for the next cycle

            if (activeSpellShield != null)
            {
                Destroy(activeSpellShield); // Remove the spell shield visual
            }

            return; // Block the damage
        }

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
        hpRegen += 0.05f;
        UpdateHP(); // Update the health UI to reflect the new maximum
    }

    public void IncreaseSpeedMultiplier(float amount)
    {
        speedMultiplier += amount;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int enemyDamage;

    public float playerStamina = 100.0f;


    [SerializeField] private float maxStamina = 100.0f;
    [SerializeField] private float dashCost = 20;

    [HideInInspector] public bool hasRegenerated = true;
    [HideInInspector] public bool weAreSprinting = false;

    [SerializeField] private float staminaDrain = 0.5f;
    [SerializeField] private float staminaRegen = 0.5f;

    [SerializeField] private int RunSpeed = 6;
    [SerializeField] private int normalRunSpeed = 4;

    [SerializeField] private Image staminaProgressUI = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;

    public GameManagerScript gameManager;


    Vector2 _Movement;

    Rigidbody2D _Rigidbody;

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
        if(playerStamina <= 0)
        {
            weAreSprinting = false;
        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            weAreSprinting = true;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            weAreSprinting = false;
        }
        
        if(weAreSprinting)
        {
            _Rigidbody.velocity = _Movement * RunSpeed;
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
            _Rigidbody.velocity = _Movement * normalRunSpeed;
        }

        if (!weAreSprinting)
        {
            if (playerStamina <= maxStamina - 0.01)
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

        if (Input.GetKeyDown(KeyCode.D))
        {
            //Damage(10);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            // Heal(10);
        }

    }




    //public void Sprinting()
    //{
    //    if (hasRegenerated)
    //    {
    //        weAreSprinting = true;
    //        playerStamina -= staminaDrain * Time.deltaTime;
    //        UpdateStamina(1);

    //        if (playerStamina <= 0)
    //        {
    //            hasRegenerated = false;

    //            sliderCanvasGroup.alpha = 0;
    //        }
    //    }
    //}


    void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;

        if (value == 0)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
        }
    }

    public SpriteRenderer entitySpriteRenderer; // Assign the entity's SpriteRenderer component in the Inspector
    public Color redColor = Color.red; // Set the desired red color
    public float redDuration = 0.5f; // Adjust the duration as needed

    [SerializeField] private int health = 100;

    private int MAX_HEALTH = 100;

    public void Damage(int amount)
    {

        this.health -= amount;

        // Change the material to red temporarily
        entitySpriteRenderer.color = redColor;

        // Start a coroutine to revert the color back to the original material
        StartCoroutine(RevertColor());


        if (health <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    private IEnumerator RevertColor()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(redDuration);

        // Revert the sprite color back to the original color (usually white)
        entitySpriteRenderer.color = Color.white; // You can set it to the original color
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
        }

        bool wouldBeOverMaxHealth = health + amount > MAX_HEALTH;

        if (wouldBeOverMaxHealth)
        {
            this.health = MAX_HEALTH;
        }
        else
        {
            this.health += amount;
        }
    }

    private bool isDead;

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


}

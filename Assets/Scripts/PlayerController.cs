using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    
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

    public void StaminaDash()
    {
        if (playerStamina >= (maxStamina * dashCost / maxStamina))
        {
            playerStamina -= dashCost;

            UpdateStamina(1);
        }
    }

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
}

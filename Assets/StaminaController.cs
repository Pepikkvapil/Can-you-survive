using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public float playerStamina = 100.0f;

    [SerializeField] private float maxStamina = 100.0f;

    [HideInInspector] public bool hasRegenerated = true;
    [HideInInspector] public bool weAreSprinting = false;

    [SerializeField] private float staminaDrain = 0.5f;
    [SerializeField] private float staminaRegen = 0.5f;



    [SerializeField] private Image staminaProgressUI = null; 
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;


    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!weAreSprinting)
        {
            if(playerStamina <= maxStamina - 0.01)
            {
                playerStamina += staminaRegen * Time.deltaTime;
                UpdateStamina(1);

                if(playerStamina >= maxStamina)
                {



                    sliderCanvasGroup.alpha = 0;
                    hasRegenerated = true;
                }
            }
        }
    }

    public void Sprinting()
    {
        if(hasRegenerated)
        {
            weAreSprinting = true;
            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);

            if(playerStamina <= 0)
            {
                hasRegenerated = false;

                sliderCanvasGroup.alpha = 0;
            }
        }
    }




    void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;

        if(value == 0)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
        }
    }
}

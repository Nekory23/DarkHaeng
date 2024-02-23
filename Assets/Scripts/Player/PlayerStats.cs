using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public SlideBar healthBar;
    public SlideBar staminaBar;

    Player.PlayerAnimatorHandler animatorHandler;
    Player.PlayerManager playerManager;

    public float staminaRegenerationAmount = 30;
    public float staminaRegenerationTimer = 0;

    private void Awake()
    {
        animatorHandler = GetComponentInChildren<Player.PlayerAnimatorHandler>();
        playerManager = GetComponent<Player.PlayerManager>();
    }
    void Start()
    {
        maxHealth = SetMaxHealthFromHealthlLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxValue(maxHealth);
        healthBar.SetCurrentValue(currentHealth);
        
        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar.SetMaxValue(Mathf.RoundToInt(maxStamina));
        staminaBar.SetCurrentValue(Mathf.RoundToInt(currentStamina));
    }

    private int SetMaxHealthFromHealthlLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    private float SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    public void TakeDamage(int damage)
    {
        if (playerManager.isInvulnerable)
            return;
        if (isDead)
            return;
        currentHealth -= damage;
        healthBar.SetCurrentValue(currentHealth);

        animatorHandler.PlayTargetAnimation("Damage_01", true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animatorHandler.PlayTargetAnimation("Dead_01", true);
            isDead = true;
        }
    }

    public void DrainStamina(int staminaDrain)
    {
        currentStamina -= staminaDrain;

        staminaBar.SetCurrentValue(Mathf.RoundToInt(currentStamina));
        if (currentStamina <= 0)
        {
            currentStamina = 0;
        }
    }

    public void RegainStamina()
    {

        if (playerManager.isInteracting) {
            staminaRegenerationTimer = 0f;
            return;
        }
        staminaRegenerationTimer += Time.deltaTime;

        if (currentStamina < maxStamina && staminaRegenerationTimer > 1f)
        {
            currentStamina += staminaRegenerationAmount * Time.deltaTime;
            staminaBar.SetCurrentValue(Mathf.RoundToInt(currentStamina));
        }
    }

}

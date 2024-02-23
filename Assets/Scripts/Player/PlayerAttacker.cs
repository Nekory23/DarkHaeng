using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    Player.PlayerAnimatorHandler animatorHandler;
    PlayerStats playerStats;
    Player.InputHandler inputHandler;
    WeaponSlotManager weaponSlotManager;

    public string lastAttack;

    private void Awake()
    {
        animatorHandler = GetComponentInChildren<Player.PlayerAnimatorHandler>();
        playerStats = GetComponent<PlayerStats>();
        inputHandler = GetComponent<Player.InputHandler>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (inputHandler.comboFlag)
        {
            if (lastAttack == weapon.oneHandedLightAttack_1)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);
                animatorHandler.PlayTargetAnimation(weapon.oneHandedLightAttack_2, true);
            }
            else if (lastAttack == weapon.oneHandedHeavyAttack_1)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);
                animatorHandler.PlayTargetAnimation(weapon.oneHandedHeavyAttack_2, true);
            }
            else if (lastAttack == weapon.twoHandedLightAttack_1)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);
                animatorHandler.PlayTargetAnimation(weapon.twoHandedLightAttack_2, true);
            } else if (lastAttack == weapon.twoHandedHeavyAttack_1)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);
                animatorHandler.PlayTargetAnimation(weapon.twoHandedHeavyAttack_2, true);
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        if (playerStats.currentStamina <= 0)
            return;
        weaponSlotManager.attackingWeapon = weapon;
        if (weapon.weaponType == WeaponItem.WeaponType.TwoHanded)
        {
            animatorHandler.PlayTargetAnimation(weapon.twoHandedLightAttack_1, true);
            lastAttack = weapon.twoHandedLightAttack_1;
        }
        else if (weapon.weaponType == WeaponItem.WeaponType.OneHanded)
        {
            animatorHandler.PlayTargetAnimation(weapon.oneHandedLightAttack_1, true);
            lastAttack = weapon.oneHandedLightAttack_1;
        }
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        if (playerStats.currentStamina <= 0)
            return;
        weaponSlotManager.attackingWeapon = weapon;
        if (weapon.weaponType == WeaponItem.WeaponType.TwoHanded)
        {
            animatorHandler.PlayTargetAnimation(weapon.twoHandedHeavyAttack_1, true);
            lastAttack = weapon.twoHandedHeavyAttack_1;
        }
        else if (weapon.weaponType == WeaponItem.WeaponType.OneHanded)
        {
            animatorHandler.PlayTargetAnimation(weapon.oneHandedHeavyAttack_1, true);
            lastAttack = weapon.oneHandedHeavyAttack_1;
        }
    }
}

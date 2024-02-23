using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    Player.PlayerManager playerManager;
    public WeaponItem attackingWeapon;

    WeaponHolderSlot leftHandSlot;
    WeaponHolderSlot rightHandSlot;
    public WeaponHolderSlot backSlot;

    DamageCollider leftHandDamageCollider;
    DamageCollider rightHandDamageCollider;

    Animator anim;

    QuickSlotsUI quickSlotsUI;

    PlayerStats playerStats;
    Player.InputHandler inputHandler;

    private void Awake()
    {
        inputHandler = GetComponentInParent<Player.InputHandler>();
        playerManager = GetComponentInParent<Player.PlayerManager>();
        anim = GetComponent<Animator>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        playerStats = GetComponentInParent<PlayerStats>();
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponHolderSlot in weaponHolderSlots)
        {
            if (weaponHolderSlot.isLeftHandSlot)
            {
                leftHandSlot = weaponHolderSlot;
            }
            else if (weaponHolderSlot.isRightHandSlot)
            {
                rightHandSlot = weaponHolderSlot;
            }
            else if (weaponHolderSlot.isBackSlot)
            {
                backSlot = weaponHolderSlot;
            }
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (isLeft)
        {
            leftHandSlot.currentWeapon = weaponItem;
            leftHandSlot.LoadWeaponModel(weaponItem);
            LoadLeftWeaponDamageCollider();
            quickSlotsUI.UpdateWeaponIcon(true, weaponItem);

            #region Handle Left Weapon's Idle Animation
            if (weaponItem != null)
            {
                anim.CrossFade(weaponItem.oneHandedLeftIdle_1, 0.2f);
            }
            else
            {
                anim.CrossFade("Left Arm Empty", 0.2f);
            }
            #endregion
        }
        else
        {
            if (weaponItem.weaponType == WeaponItem.WeaponType.TwoHanded)
            {
                backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                leftHandSlot.UnloadWeaponAndDestroy();
                anim.CrossFade(weaponItem.twoHandedIdle_1, 0.2f);
            }
            else
            {
                #region Handle Right Weapon's Idle Animation

                anim.CrossFade("Both Arms Empty", 0.2f);

                backSlot.UnloadWeaponAndDestroy();
                if (weaponItem != null)
                {
                    anim.CrossFade(weaponItem.oneHandedRightIdle_1, 0.2f);
                }
                else
                {
                    anim.CrossFade("Right Arm Empty", 0.2f);
                }
                #endregion
            }
            rightHandSlot.currentWeapon = weaponItem;
            rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponDamageCollider();
            quickSlotsUI.UpdateWeaponIcon(false, weaponItem);
        }
    }

    #region Handle Weapon's Damage Collider

    private void LoadLeftWeaponDamageCollider()
    {
        if (leftHandSlot.currentWeaponModel != null)
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        } else {
            leftHandDamageCollider = null;
        }
    }

    private void LoadRightWeaponDamageCollider()
    {
        if (rightHandSlot.currentWeaponModel != null)
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        } else {
            rightHandDamageCollider = null;
        }
    }

    public void OpenDamageCollider()
    {
        if (playerManager.isUsingRightHand && rightHandDamageCollider != null)
        {
            rightHandDamageCollider.EnableDamageCollider();
        }
        else if (playerManager.isUsingLeftHand && leftHandDamageCollider != null)
        {
            leftHandDamageCollider.EnableDamageCollider();
        }
    }

    public void CloseDamageCollider()
    {
        if (rightHandDamageCollider != null)
        {
            rightHandDamageCollider.DisableDamageCollider();
        }
        if (leftHandDamageCollider != null)
        {
            leftHandDamageCollider.DisableDamageCollider();
        }
    }
    #endregion

    #region Handle Weapon's Stamina Drain
    public void DrainStaminaLightAttack()
    {
        playerStats.DrainStamina(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMutliplier));
    }

    public void DrainStaminaHeavyAttack()
    {
        playerStats.DrainStamina(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }
    #endregion
}

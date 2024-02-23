using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager weaponSlotManager;

    public WeaponItem rightHandWeapon;
    public WeaponItem leftHandWeapon;

    public WeaponItem unarmedWeapon;

    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

    public int currentRightHandWeaponIndex = -1;
    public int currentLeftHandWeaponIndex = -1;

    public List<WeaponItem> weaponsInventory;

    private void Awake()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    private void Start()
    {
        if (weaponsInLeftHandSlots.Length > 0)
        {
            leftHandWeapon = weaponsInLeftHandSlots[0];
            currentLeftHandWeaponIndex = 0;
            weaponSlotManager.LoadWeaponOnSlot(leftHandWeapon, true);
        }
        else
        {
            leftHandWeapon = unarmedWeapon;
            currentLeftHandWeaponIndex = -1;
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
        }
        if (weaponsInRightHandSlots.Length > 0)
        {
            rightHandWeapon = weaponsInRightHandSlots[0];
            currentRightHandWeaponIndex = 0;
            weaponSlotManager.LoadWeaponOnSlot(rightHandWeapon, false);
        }
        else
        {
            rightHandWeapon = unarmedWeapon;
            currentRightHandWeaponIndex = -1;
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
        }
    }

    public void ChangeRightWeapon()
    {
        currentRightHandWeaponIndex = currentRightHandWeaponIndex + 1;

        if (currentRightHandWeaponIndex > weaponsInRightHandSlots.Length - 1)
        {
            currentRightHandWeaponIndex = -1;

            //            
            if (rightHandWeapon.weaponType == WeaponItem.WeaponType.TwoHanded)
            {
                weaponSlotManager.LoadWeaponOnSlot(leftHandWeapon, true);
            }
            //

            rightHandWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
        }
        else if (weaponsInRightHandSlots[currentRightHandWeaponIndex] != null)
        {
            //
            if (rightHandWeapon.weaponType == WeaponItem.WeaponType.TwoHanded && weaponsInRightHandSlots[currentRightHandWeaponIndex].weaponType != WeaponItem.WeaponType.TwoHanded)
            {
                weaponSlotManager.LoadWeaponOnSlot(leftHandWeapon, true);
            }
            //
            rightHandWeapon = weaponsInRightHandSlots[currentRightHandWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightHandWeaponIndex], false);
        }
        else
        {
            currentRightHandWeaponIndex = currentRightHandWeaponIndex + 1;
        }
    }

    public void ChangeLeftWeapon()
    {
        currentLeftHandWeaponIndex = currentLeftHandWeaponIndex + 1;

        if (currentLeftHandWeaponIndex > weaponsInLeftHandSlots.Length - 1)
        {
            currentLeftHandWeaponIndex = -1;
            leftHandWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
        }
        else if (weaponsInLeftHandSlots[currentLeftHandWeaponIndex] != null)
        {
            leftHandWeapon = weaponsInLeftHandSlots[currentLeftHandWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftHandWeaponIndex], true);
        }
        else
        {
            currentLeftHandWeaponIndex = currentLeftHandWeaponIndex + 1;
        }
    }
}

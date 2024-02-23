using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventorySlot : MonoBehaviour
{
    PlayerInventory playerInventory;
    WeaponSlotManager weaponSlotManager;
    [SerializeField]
    QuickSlotsUI quickSlotsUI;

    UIManager uiManager;
    public Image icon;
    WeaponItem item;

    private void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        weaponSlotManager = FindObjectOfType<WeaponSlotManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void AddItem(WeaponItem newItem)
    {
        item = newItem;
        icon.sprite = item.itemIcon;
        icon.enabled = true;
        gameObject.SetActive(true);
    }

    public void ClearInventorySlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        gameObject.SetActive(false);
    }

    public void EquipThisItem()
    {
        if (uiManager.rightHandSlot01Selected)
        {
            if (playerInventory.weaponsInRightHandSlots[0].isUnarmed == false)
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlots[0]);
            playerInventory.weaponsInRightHandSlots[0] = item;
            playerInventory.weaponsInventory.Remove(item);
        }
        else if (uiManager.rightHandSlot02Selected)
        {
            if (playerInventory.weaponsInRightHandSlots[1].isUnarmed == false)
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlots[1]);
            playerInventory.weaponsInRightHandSlots[1] = item;
            playerInventory.weaponsInventory.Remove(item);
        }
        else if (uiManager.leftHandSlot01Selected)
        {
            if (playerInventory.weaponsInLeftHandSlots[0].isUnarmed == false)
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlots[0]);
            playerInventory.weaponsInLeftHandSlots[0] = item;
            playerInventory.weaponsInventory.Remove(item);
        }
        else if (uiManager.leftHandSlot02Selected)
        {
            if (playerInventory.weaponsInLeftHandSlots[1].isUnarmed == false)
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlots[1]);
            playerInventory.weaponsInLeftHandSlots[1] = item;
            playerInventory.weaponsInventory.Remove(item);
        }
        else
        {
            return;
        }
        if (playerInventory.currentRightHandWeaponIndex != -1)
            playerInventory.rightHandWeapon = playerInventory.weaponsInRightHandSlots[playerInventory.currentRightHandWeaponIndex];
        if (playerInventory.currentLeftHandWeaponIndex != -1)
            playerInventory.leftHandWeapon = playerInventory.weaponsInLeftHandSlots[playerInventory.currentLeftHandWeaponIndex];
        weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightHandWeapon, false);
        if (playerInventory.rightHandWeapon.weaponType == WeaponItem.WeaponType.TwoHanded)
        {
            weaponSlotManager.backSlot.LoadWeaponModel(playerInventory.leftHandWeapon);
            quickSlotsUI.UpdateWeaponIcon(true, playerInventory.leftHandWeapon);
        }
        else
        {
            weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftHandWeapon, true);
        }
        uiManager.uiEquipmentWindow.LoadWeaponsOnEquipmentSlots(playerInventory);
        uiManager.ResetAllEquipmentSlotSelected();
    }
}

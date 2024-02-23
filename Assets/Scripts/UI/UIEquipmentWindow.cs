using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEquipmentWindow : MonoBehaviour
{
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;

    public HandEquipmentSlotUI[] handEquipmentSlots;

    public void LoadWeaponsOnEquipmentSlots(PlayerInventory playerInventory)
    {
        for (int i = 0; i < handEquipmentSlots.Length; i++)
        {
            if (handEquipmentSlots[i].rightHandSlot01 && playerInventory.weaponsInRightHandSlots[0] != null)
            {
                handEquipmentSlots[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
            }
            else if (handEquipmentSlots[i].rightHandSlot02 && playerInventory.weaponsInRightHandSlots[1] != null)
            {
                handEquipmentSlots[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
            }
            else if (handEquipmentSlots[i].leftHandSlot01 && playerInventory.weaponsInLeftHandSlots[0] != null)
            {
                handEquipmentSlots[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
            }
            else if (handEquipmentSlots[i].leftHandSlot02 && playerInventory.weaponsInLeftHandSlots[1] != null)
            {
                handEquipmentSlots[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
            }
        }
    }

    public void SelectRightHandSlot01()
    {
        rightHandSlot01Selected = true;
    }

    public void SelectRightHandSlot02()
    {
        rightHandSlot02Selected = true;
    }

    public void SelectLeftHandSlot01()
    {
        leftHandSlot01Selected = true;
    }

    public void SelectLeftHandSlot02()
    {
        leftHandSlot02Selected = true;
    }

    public void DeselectRightHandSlot01()
    {
        rightHandSlot01Selected = false;
    }
}

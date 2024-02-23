using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotsUI : MonoBehaviour
{
    public Image leftWeaponIcon;
    public Image rightWeaponIcon;

    public void UpdateWeaponIcon(bool isLeftWeapon, WeaponItem weapon)
    {
        if (isLeftWeapon)
        {
            if (weapon != null && weapon.itemIcon != null)
            {
                leftWeaponIcon.sprite = weapon.itemIcon;
                leftWeaponIcon.enabled = true;
            } else {
                leftWeaponIcon.sprite = null;
                leftWeaponIcon.enabled = false;
            }
        }
        else
        {
            if (weapon && weapon.itemIcon != null)
            {
                rightWeaponIcon.sprite = weapon.itemIcon;
                rightWeaponIcon.enabled = true;
            } else {
                rightWeaponIcon.sprite = null;
                rightWeaponIcon.enabled = false;
            }
        }
    }
}

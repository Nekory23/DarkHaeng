using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public class EnemyWeaponSlotManager : MonoBehaviour
    {
        public WeaponItem rightItem;
        public WeaponItem leftItem;

        WeaponHolderSlot rightSlot;
        WeaponHolderSlot leftSlot;

        DamageCollider rightDamageCollider;
        DamageCollider leftDamageCollider;

        private void Awake()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

            foreach (WeaponHolderSlot weaponHolderSlot in weaponHolderSlots) {
                if (weaponHolderSlot.isLeftHandSlot)
                    leftSlot = weaponHolderSlot;
                if (weaponHolderSlot.isRightHandSlot)
                    rightSlot = weaponHolderSlot;
            }
        }

        private void Start()
        {
            LoadBothWeapons();
        }

        public void LoadBothWeapons()
        {
            if (rightItem != null) {
                rightSlot.LoadWeaponModel(rightItem);
                rightDamageCollider = rightSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
                
            if (leftItem != null) {
                leftSlot.LoadWeaponModel(leftItem);
                leftDamageCollider = leftSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
                
        }

        public void OpenDamageCollider()
        {
            rightDamageCollider.EnableDamageCollider();
            //leftDamageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            rightDamageCollider.DisableDamageCollider();
            //leftDamageCollider.DisableDamageCollider();
        }

        #region Handle Weapon's Stamina Drain
        public void DrainStaminaLightAttack()
        {
        }

        public void DrainStaminaHeavyAttack()
        {
        }
        #endregion
    }
}

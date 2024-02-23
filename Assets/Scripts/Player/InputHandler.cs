using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input = false;
        public bool a_Input = false;
        public bool y_input = false;
        public bool rb_Input = false;
        public bool rt_Input = false;
        public bool jump_Input = false;
        public bool inventory_Input = false;
        public bool lockOn_Input = false;
        public bool right_Stick_Input = false;
        public bool left_Stick_Input = false;

        public bool d_PadUp = false;
        public bool d_PadDown = false;
        public bool d_PadLeft = false;
        public bool d_PadRight = false;

        public bool rollFlag = false;
        public bool twoHandFlag = false;
        public bool sprintFlag = false;
        public bool comboFlag = false;
        public bool lockOnFlag = false;
        public bool inventoryFlag = false;
        public float rollInputTimer = 0;

        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        PlayerStats playerStats;
        WeaponSlotManager weaponSlotManager;
        CameraHandler cameraHandler;
        UIManager uiManager;
        Vector2 movementInput;
        PlayerAnimatorHandler animHandler;
        Vector2 cameraInput;

        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            uiManager = FindObjectOfType<UIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            animHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => left_Stick_Input = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Input = true;

                inputActions.PlayerActions.A.performed += i => a_Input = true;
                inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
                inputActions.PlayerActions.Roll.performed += i => b_Input = true;
                inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
                inputActions.PlayerActions.RB.performed += i => rb_Input = true;
                inputActions.PlayerActions.RT.performed += i => rt_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                // inputActions.PlayerActions.Y.performed += i => y_input = true;

                inputActions.PlayerQuickSlots.DPadUp.performed += i => d_PadUp = true;
                inputActions.PlayerQuickSlots.DPadDown.performed += i => d_PadDown = true;
                inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_PadLeft = true;
                inputActions.PlayerQuickSlots.DPadRight.performed += i => d_PadRight = true;
            }
            inputActions.Enable();
        }

        public void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            HandleMoveInput(delta);
            CameraInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleQuickSlotsInput();
            HandleInventoryInput();
            HandleLockOnInput();
        }

        private void HandleMoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        }

        private void CameraInput(float delta)
        {
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleRollInput(float delta)
        {
            if (b_Input)
            {
                rollInputTimer += delta;

                if (playerStats.currentStamina <= 0)
                {
                    sprintFlag = false;
                    b_Input = false;
                }
                if (moveAmount > 0.5f && playerStats.currentStamina > 0)
                {
                    sprintFlag = true;
                }
            }
            else
            {
                sprintFlag = false;
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    rollFlag = true;
                }
                rollInputTimer = 0;
            }
        }

        private void HandleAttackInput(float delta)
        {
            if (rb_Input)
            {
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightHandWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (playerManager.isInteracting)
                        return;
                    if (playerManager.canDoCombo)
                        return;
                    animHandler.anim.SetBool("isUsingRightHand", true);
                    playerAttacker.HandleLightAttack(playerInventory.rightHandWeapon);
                }
            }
            if (rt_Input)
            {
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightHandWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (playerManager.isInteracting)
                        return;
                    if (playerManager.canDoCombo)
                        return;

                    animHandler.anim.SetBool("isUsingRightHand", true);
                    playerAttacker.HandleHeavyAttack(playerInventory.rightHandWeapon);
                }
            }
        }

        private void HandleQuickSlotsInput()
        {
            if (d_PadRight)
            {   
                playerInventory.ChangeRightWeapon();
            }
            else if (d_PadLeft && playerInventory.rightHandWeapon.weaponType != WeaponItem.WeaponType.TwoHanded)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }

        private void HandleInventoryInput()
        {
            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;
                if (inventoryFlag)
                {
                    uiManager.OpenSelectWindow();
                    uiManager.UpdateUI();
                    uiManager.hudWindow.SetActive(false);
                }
                else
                {
                    uiManager.CloseSelectWindow();
                    uiManager.CloseAllInventoryWindows();
                    uiManager.hudWindow.SetActive(true);
                }
            }
        }

        private void HandleLockOnInput()
        {
            if (lockOn_Input && lockOnFlag == false)
            {
                lockOn_Input = false;
                cameraHandler.HandleLockOn();

                if (cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currenLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOn_Input && lockOnFlag == true)
            {
                lockOn_Input = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
            }
            if (lockOnFlag && right_Stick_Input)
            {
                right_Stick_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currenLockOnTarget = cameraHandler.rightLockTarget;
                }
            }
            else if (lockOnFlag && left_Stick_Input)
            {
                left_Stick_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currenLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            cameraHandler.SetCameraHeight();
        }
    }
} // namespace Player

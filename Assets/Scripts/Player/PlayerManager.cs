using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerManager : CharacterManager
    {
        Player.InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        Player.PlayerLocomotion playerLocomotion;
        InteractableUI interactableUI;
        PlayerStats playerStats;
        PlayerAnimatorHandler animatorHandler;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractabmeGameObject;

        public bool isInteracting;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isInvulnerable;

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            inputHandler = GetComponent<Player.InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerStats = GetComponent<PlayerStats>();
            playerLocomotion = GetComponent<Player.PlayerLocomotion>();
            interactableUI = FindObjectOfType<InteractableUI>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        }

        void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");
            isUsingLeftHand = anim.GetBool("isUsingLeftHand");
            isUsingRightHand = anim.GetBool("isUsingRightHand");
            isInvulnerable = anim.GetBool("isInvulnerable");
            anim.SetBool("isInAir", isInAir);

            inputHandler.TickInput(delta);
            animatorHandler.canRotate = anim.GetBool("canRotate");
            playerLocomotion.HandleRollingAndSprinting();
            playerLocomotion.HandleJumping();

            playerStats.RegainStamina();
            CheckForInteractableObject();
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRotation(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            isSprinting = inputHandler.b_Input;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.d_PadUp = false;
            inputHandler.d_PadDown = false;
            inputHandler.d_PadLeft = false;
            inputHandler.d_PadRight = false;
            inputHandler.a_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.inventory_Input = false;

            float delta = Time.deltaTime;
            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }

            if (isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
            else
            {
                playerLocomotion.inAirTimer = 0;
            }
        }

        public void CheckForInteractableObject()
        {
            RaycastHit hit;
            Vector3 origin = transform.position;
            origin.y += 2f;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers) ||
                Physics.SphereCast(origin, 0.3f, transform.forward, out hit, 2.5f, cameraHandler.ignoreLayers))
            {
                if (hit.collider.CompareTag("Interactable"))
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        string interactableText = interactable.interactableText;

                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);
                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (itemInteractabmeGameObject != null && inputHandler.a_Input)
                {
                    itemInteractabmeGameObject.SetActive(false);
                }
            }
        }
    }
} // namespace Player

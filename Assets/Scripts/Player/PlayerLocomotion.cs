using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerLocomotion : MonoBehaviour
    {
        CameraHandler cameraHandler;
        Player.PlayerManager playerManager;
        Transform cameraObject;
        Player.InputHandler inputHandler;
        PlayerStats playerStats;
        public Vector3 moveDirection;

        [HideInInspector]
        public Transform m_transform;
        [HideInInspector]
        public Player.PlayerAnimatorHandler animatorHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Ground & Air Detection Stats")]
        [SerializeField]
        float groundDetectionRayStartPoint = 0.5f;
        [SerializeField]
        float minimumDistanceNeededToBeginFall = 1f;
        [SerializeField]
        float groundDirectionRayDistance = 0.2f;
        LayerMask ignoreForGroundCheck;

        public float inAirTimer;

        [Header("Movement Stats")]
        [SerializeField]
        float walkigSpeed = 2;
        [SerializeField]
        float moveSpeed = 5;
        [SerializeField]
        float rotationSpeed = 10;
        [SerializeField]
        float sprintSpeed = 7;
        [SerializeField]
        float fallingSpeed = 45;
        [SerializeField]
        float leapingVelocity = 3;

        [Header("Stamina Consumption Stats")]
        [SerializeField]
        int sprintStaminaConsumption = 1;
        [SerializeField]
        int rollStaminaConsumption = 10;

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            playerStats = GetComponent<PlayerStats>();
            playerManager = GetComponent<Player.PlayerManager>();
            animatorHandler = GetComponentInChildren<Player.PlayerAnimatorHandler>();
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<Player.InputHandler>();
        }

        void Start()
        {
            m_transform = transform;
            cameraObject = Camera.main.transform;
            animatorHandler.Initialize();

            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        public void HandleRotation(float delta)
        {
            if (animatorHandler.canRotate)
            {
                if (inputHandler.lockOnFlag)
                {
                    if (inputHandler.sprintFlag || inputHandler.rollFlag)
                    {
                        Vector3 targetDirection = Vector3.zero;
                        targetDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                        targetDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                        targetDirection.Normalize();
                        targetDirection.y = 0;

                        if (targetDirection == Vector3.zero)
                            targetDirection = m_transform.forward;

                        Quaternion tr = Quaternion.LookRotation(targetDirection);
                        Quaternion targetRotation = Quaternion.Slerp(m_transform.rotation, tr, rotationSpeed * Time.deltaTime);
                        transform.rotation = targetRotation;
                    }
                    else
                    {
                        Vector3 rotationDirection = moveDirection;
                        rotationDirection = cameraHandler.currenLockOnTarget.transform.position - transform.position;
                        rotationDirection.y = 0;
                        rotationDirection.Normalize();

                        Quaternion tr = Quaternion.LookRotation(rotationDirection);
                        Quaternion targetRotation = Quaternion.Slerp(m_transform.rotation, tr, rotationSpeed * Time.deltaTime);
                        transform.rotation = targetRotation;
                    }
                }
                else
                {
                    Vector3 targetDirection = Vector3.zero;
                    float moveOverride = inputHandler.moveAmount;

                    targetDirection = cameraObject.forward * inputHandler.vertical;
                    targetDirection += cameraObject.right * inputHandler.horizontal;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if (targetDirection == Vector3.zero)
                        targetDirection = m_transform.forward;
                    float rs = rotationSpeed;

                    Quaternion tr = Quaternion.LookRotation(targetDirection);
                    Quaternion targetRotation = Quaternion.Slerp(m_transform.rotation, tr, rs * delta);

                    m_transform.rotation = targetRotation;
                }
            }

        }

        public void HandleMovement(float delta)
        {
            if (inputHandler.rollFlag)
                return;

            if (playerManager.isInteracting)
                return;
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = moveSpeed;

            if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
                playerStats.DrainStamina(sprintStaminaConsumption);
            }
            else
            {
                if (inputHandler.moveAmount < 0.5f)
                {
                    moveDirection *= walkigSpeed;
                    playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    playerManager.isSprinting = false;
                }
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            if (inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting);
            }
            else
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0f, playerManager.isSprinting);
            }
        }

        public void HandleRollingAndSprinting()
        {
            if (animatorHandler.anim.GetBool("isInteracting"))
                return;
            if (playerStats.currentStamina <= 0)
                return;
            if (inputHandler.rollFlag)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;

                if (inputHandler.moveAmount > 0)
                {
                    animatorHandler.PlayTargetAnimation("Rolling", true);
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    m_transform.rotation = rollRotation;
                    playerStats.DrainStamina(rollStaminaConsumption);
                }
            }
        }

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = m_transform.position;
            origin.y += groundDetectionRayStartPoint;

            if (playerManager.isInAir)
            {
                rigidbody.AddForce(moveDirection * leapingVelocity);
                rigidbody.AddForce(-Vector3.up * fallingSpeed * inAirTimer);
            }
            Vector3 dir = moveDirection;
            dir.Normalize();
            origin += dir * groundDirectionRayDistance;

            targetPosition = m_transform.position;

            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if (playerManager.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        animatorHandler.PlayTargetAnimation("Land", true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Empty", false);
                        inAirTimer = 0;
                    }
                    playerManager.isInAir = false;
                }
            }
            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if (playerManager.isInAir == false)
                {
                    if (playerManager.isInteracting == false)
                    {
                        animatorHandler.PlayTargetAnimation("Falling", true);
                    }

                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (moveSpeed / 2);
                    playerManager.isInAir = true;
                }
            }

            if (playerManager.isInteracting || inputHandler.moveAmount > 0)
            {
                m_transform.position = Vector3.Lerp(m_transform.position, targetPosition, delta / 0.1f);
            }
            else
            {
                m_transform.position = targetPosition;
            }
        }

        public void HandleJumping()
        {
            if (playerStats.currentStamina <= 0)
                return;
            if (playerManager.isInteracting)
                return;
            if (inputHandler.jump_Input)
            {
                if (inputHandler.moveAmount > 0)
                {
                    moveDirection = cameraObject.forward * inputHandler.vertical;
                    moveDirection += cameraObject.right * inputHandler.horizontal;
                    animatorHandler.PlayTargetAnimation("Jump", true);
                    moveDirection.y = 0;
                    Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    m_transform.rotation = jumpRotation;
                }
            }
        }

        #endregion
    }
}  // namespace Player

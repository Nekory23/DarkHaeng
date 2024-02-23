using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    Player.InputHandler inputHandler;
    Player.PlayerManager playerManager;
    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform pivotTransform;
    private Transform m_transform;

    private Vector3 cameraTransformPosition;
    public LayerMask ignoreLayers;
    public LayerMask environmentLayers;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    public static CameraHandler singleton;

    public float lookSpeed = 0.1f;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;

    private float targetPosition;
    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;
    public float minPivot = -35f;
    public float maxPivot = 35f;

    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f;
    public float minCollisionOffset = 0.2f;
    public float lockedPivotPosition = 2.25f;
    public float unlockedPivotPosition = 1.65f;

    public CharacterManager currenLockOnTarget;
    List<CharacterManager> availableTargets = new List<CharacterManager>();
    public CharacterManager nearestLockOnTarget;
    public CharacterManager leftLockTarget;
    public CharacterManager rightLockTarget;
    public float maximumLockOnDistance = 30f;

    private void Awake()
    {
        singleton = this;
        m_transform = transform;
        defaultPosition = cameraTransform.localPosition.z;
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 11);
        inputHandler = FindObjectOfType<Player.InputHandler>();
        playerManager = FindObjectOfType<Player.PlayerManager>();
    }

    private void Start()
    {
        environmentLayers = LayerMask.NameToLayer("Environment");
    }

    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp(m_transform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
        m_transform.position = targetPosition;
        HandleCameraCollision(delta);
    }

    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
    {
        if (inputHandler.lockOnFlag == false && currenLockOnTarget == null)
        {
            lookAngle += (mouseXInput * lookSpeed) / delta;
            pivotAngle -= (mouseYInput * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            m_transform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;
            targetRotation = Quaternion.Euler(rotation);
            pivotTransform.localRotation = targetRotation;
        }
        else
        {
            Vector3 dir = currenLockOnTarget.lockOnTransform.transform.position - transform.position;
            dir.Normalize();
            dir.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = targetRotation;

            dir = currenLockOnTarget.lockOnTransform.transform.position - pivotTransform.position;
            dir.Normalize();

            targetRotation = Quaternion.LookRotation(dir);
            Vector3 eulerAngle = targetRotation.eulerAngles;
            eulerAngle.y = 0;
            pivotTransform.localEulerAngles = eulerAngle;
        }
    }

    public void HandleCameraCollision(float delta)
    {
        targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - pivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(pivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
        {
            float dis = Vector3.Distance(pivotTransform.position, hit.point);
            targetPosition = -(dis - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minCollisionOffset)
        {
            targetPosition = -minCollisionOffset;
        }

        cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
        cameraTransform.localPosition = cameraTransformPosition;
    }

    public void HandleLockOn()
    {
        float shortestDistance = Mathf.Infinity;
        float shortestDistanceOfLeftTarget = -Mathf.Infinity;
        float shortestDistanceOfRightTarget = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(transform.position, 26f);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].GetComponent<CharacterManager>();

            if (character != null)
            {
                Vector3 lockTargetPosition = character.transform.position - targetTransform.position;
                float distance = Vector3.Distance(targetTransform.position, character.transform.position);
                float viewableAngle = Vector3.Angle(lockTargetPosition, cameraTransform.forward);
                RaycastHit hit;

                if (character.transform.root != targetTransform.transform.root &&
                    viewableAngle > -50 && viewableAngle < 50 &&
                    distance <= maximumLockOnDistance)
                {
                    if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                    {
                        Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position, Color.red);
                        if (hit.transform.gameObject.layer == environmentLayers)
                        {
                            continue;
                        }
                        else
                        {
                            availableTargets.Add(character);
                        }
                    }
                }
            }
        }

        for (int i = 0; i < availableTargets.Count; i++)
        {
            float distance = Vector3.Distance(targetTransform.position, availableTargets[i].transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestLockOnTarget = availableTargets[i];
            }

            if (inputHandler.lockOnFlag)
            {
                Vector3 relativeEnemyPosition = inputHandler.transform.InverseTransformPoint(availableTargets[i].transform.position);
                var distantceFromLeftTarget = relativeEnemyPosition.x;
                var distantceFromRightTarget = relativeEnemyPosition.x;

                if (relativeEnemyPosition.x <= 0.00 && distantceFromLeftTarget > shortestDistanceOfLeftTarget &&
                    availableTargets[i] != currenLockOnTarget)
                {
                    shortestDistanceOfLeftTarget = distantceFromLeftTarget;
                    leftLockTarget = availableTargets[i];
                }
                else if (relativeEnemyPosition.x >= 0.00 && distantceFromRightTarget < shortestDistanceOfRightTarget &&
                    availableTargets[i] != currenLockOnTarget)
                {
                    shortestDistanceOfRightTarget = distantceFromRightTarget;
                    rightLockTarget = availableTargets[i];
                }
            }
        }
    }

    public void ClearLockOnTargets()
    {
        availableTargets.Clear();
        nearestLockOnTarget = null;
        currenLockOnTarget = null;
    }

    public void SetCameraHeight()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 newLockedPosition = new Vector3(0f, lockedPivotPosition, 0f);
        Vector3 newUnlockedPosition = new Vector3(0f, unlockedPivotPosition, 0f);

        if (currenLockOnTarget != null)
        {
            pivotTransform.transform.localPosition = Vector3.SmoothDamp(pivotTransform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
        }
        else
        {
            pivotTransform.transform.localPosition = Vector3.SmoothDamp(pivotTransform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
        }
    }
}

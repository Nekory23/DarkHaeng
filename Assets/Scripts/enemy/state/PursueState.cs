using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class PursueState : State
    {
        [Header("State management")]
        public State combatState;
        public RotateTowardsTarget rotateTowardsTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float distanceFromTarget = Vector3.Distance(transform.position, enemyManager.currentTarget.transform.position);
            float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

            enemyManager.GetNavMeshAgent().transform.localPosition = Vector3.zero;
            enemyManager.GetNavMeshAgent().transform.localRotation = Quaternion.identity;
            HandleRotationTowardsTarget(enemyManager);

            if (enemyManager.isInterracting)
                return this;

            if (viewableAngle > 65f || viewableAngle < -65f)
                return rotateTowardsTargetState;

            if (enemyManager.isPerformingAction)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            if (distanceFromTarget > enemyManager.maximumAggroRadius)
                enemyAnimatorManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
        
            if (distanceFromTarget <= enemyManager.maximumAggroRadius)
                return combatState;
            return this;
        }

        void HandleRotationTowardsTarget(EnemyManager enemyManager)
        {
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;

                direction.y = 0;
                direction.Normalize();
                if (direction == Vector3.zero)
                    direction = transform.forward;

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.GetNavMeshAgent().desiredVelocity);
                Vector3 targetVelocity = enemyManager.GetRigidbody().velocity;

                enemyManager.GetNavMeshAgent().enabled = true;
                enemyManager.GetNavMeshAgent().SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.GetRigidbody().velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.GetNavMeshAgent().transform.rotation,
                                                                   enemyManager.rotationSpeed / Time.deltaTime);
            }
        }
    }
}
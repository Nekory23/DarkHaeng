using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class AttackState : State
    {

        [Header("State management")]
        public CombatState combatState;
        public RotateTowardsTarget rotateTowardsTargetState;
        public PursueState pursueState;

        [Header("Attacks")]
        public EnemyAttackAction currentAttack;
        public bool hasPerformedAction = false;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.position, enemyManager.transform.position);

            RotateTowardsWhileAttacking(enemyManager);
            if (distanceFromTarget > currentAttack.maximumDistanceNeededToAttack)
            {
                return pursueState;
            }

            if (!hasPerformedAction)
            {
                AttackTarget(enemyAnimatorManager);
            }

            return rotateTowardsTargetState;
        }

        private void AttackTarget(AI.EnemyAnimatorManager enemyAnimatorManager)
        {
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimations, true);
            hasPerformedAction = true;
            currentAttack = null;
        }

        void RotateTowardsWhileAttacking(EnemyManager enemyManager)
        {
            if (enemyManager.canRotate && enemyManager.isInterracting)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;

                direction.y = 0;
                direction.Normalize();
                if (direction == Vector3.zero)
                    direction = transform.forward;

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }
    }
}
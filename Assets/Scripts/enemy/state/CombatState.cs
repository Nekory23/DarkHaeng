using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public class CombatState : State
    {
        [Header("State management")]
        public AttackState attackState;
        public EnemyAttackAction[] enemyAttacks;
        public PursueState pursueState;

        public bool randomDestination = false;
        float verticalMovement = 0f;
        float horizontalMovement = 0f;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.position, enemyManager.transform.position);
            enemyAnimatorManager.anim.SetFloat("Vertical", verticalMovement, 0.2f, Time.deltaTime);
            enemyAnimatorManager.anim.SetFloat("Horizontal", horizontalMovement, 0.2f, Time.deltaTime);
            attackState.hasPerformedAction = false;

            if (enemyManager.isInterracting)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0f);
                enemyAnimatorManager.anim.SetFloat("Horizontal", 0f);
                return this;
            }
            if (distanceFromTarget > enemyManager.maximumAggroRadius)
                return pursueState;

            if (!randomDestination)
            {
                randomDestination = true;
                DecideCirclingAction(enemyAnimatorManager);

            }

            HandleRotationTowardsTarget(enemyManager);

            #region Handle Switch state
            if (enemyManager.GetCurrentRecoveryTime() <= 0 && attackState.currentAttack != null)
            {
                randomDestination = false;
                return attackState;
            }
            else
            {
                GetNewAttack(enemyManager);
            }
            return this;
            #endregion
        }

        private void GetNewAttack(EnemyManager enemyManager)
        {
            List<EnemyAttackAction> usableAttacks = new List<EnemyAttackAction>();
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            distanceFromTarget = Vector3.Distance(transform.position, enemyManager.currentTarget.transform.position);

            for (int i = 0; i != enemyAttacks.Length; ++i)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                        usableAttacks.Add(enemyAttackAction);
            }

            if (usableAttacks.Count == 0)
                return;
            attackState.currentAttack = usableAttacks[Random.Range(0, usableAttacks.Count)];
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

        private void DecideCirclingAction(AI.EnemyAnimatorManager enemyAnimatorManager)
        {
            WalkAroundTargets(enemyAnimatorManager);
        }

        void WalkAroundTargets(AI.EnemyAnimatorManager enemyAnimatorManager)
        {
            verticalMovement = 0.5f;

            horizontalMovement = Random.Range(-1f, 1f);

            if (horizontalMovement <= 1f && horizontalMovement > 1f)
            {
                horizontalMovement = 0.5f;
            }
            else if (horizontalMovement < 0f && horizontalMovement >= -1f)
            {
                horizontalMovement = -0.5f;
            }
            print("Horizontal: " + horizontalMovement + " Vertical: " + verticalMovement );
        }
    }
}

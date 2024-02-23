using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class RotateTowardsTarget : State
    {
        public CombatState combatState;

        public override State Tick(AI.EnemyManager enemyManager, AI.EnemyStats enemyStats, AI.EnemyAnimatorManager enemyAnimatorManager)
        {
            enemyAnimatorManager.anim.SetFloat("Vertical", 0);
            enemyAnimatorManager.anim.SetFloat("Horizontal", 0);

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

            if (enemyManager.isInterracting)
                return this;

            if ((viewableAngle >= 100 && viewableAngle <= 180)
                    || (viewableAngle <= -101 && viewableAngle >= -180))
            {
                enemyAnimatorManager.PlayTargetAnimationWithRootMotion("U Turn", true);
                return combatState;
            }
            else if (viewableAngle <= -45 && viewableAngle >= -100)
            {
                enemyAnimatorManager.PlayTargetAnimationWithRootMotion("Right Turn", true);
                return combatState;
            }
            else if (viewableAngle >= 45 && viewableAngle <= 100)
            {
                enemyAnimatorManager.PlayTargetAnimationWithRootMotion("Left Turn", true);
                return combatState;
            }
            return combatState;
        }
    }
}
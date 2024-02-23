using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
    [CreateAssetMenu(menuName = "AI/Enemy Actions/Attack")]
    public class EnemyAttackAction : EnemyAction
    {
        public int attackScore = 3;
        public float recoveryTime = 2;

        public float minimumAttackAngle = -35f;
        public float maximumAttackAngle = 35f;

        public float minimumDistanceNeededToAttack = 0;
        public float maximumDistanceNeededToAttack = 3f;
    }
}
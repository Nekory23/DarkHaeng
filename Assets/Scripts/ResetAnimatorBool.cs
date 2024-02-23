using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    Dictionary<string, bool> booleans = new Dictionary<string, bool> {
        { "isInteracting", false },
        { "canDoCombo", false},
        { "isUsingRightHand", false},
        { "isUsingLeftHand", false},
        { "isInvulnerable", false},
        { "canRotate", true },
        { "isRotatingWithRootMotion", false },
    };

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var boolean in booleans)
            animator.SetBool(boolean.Key, boolean.Value);
    }
}

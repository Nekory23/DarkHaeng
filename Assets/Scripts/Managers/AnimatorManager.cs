using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers {
    public class AnimatorManager : MonoBehaviour
    {
        public Animator anim;
        public bool canRotate;

        public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("canRotate", false);
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnimation, 0.2f);
        }

        public void PlayTargetAnimationWithRootMotion(string targetAnimation, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isRotatingWithRootMotion", true);
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnimation, 0.2f);
        }
    }
}
